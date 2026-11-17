-- Run once against the Online Diary database before using tutor school requests.

-- Link each class to the principal record for its school. Keep the column nullable
-- while existing classes are mapped to their school IDs.
SET @add_school_id_column = IF(
    (SELECT COUNT(*) FROM information_schema.columns
      WHERE table_schema = DATABASE() AND table_name = 'Class' AND column_name = 'school_id') = 0,
    'ALTER TABLE Class ADD COLUMN school_id INT NULL AFTER section',
    'SELECT 1'
);
PREPARE add_school_id_stmt FROM @add_school_id_column;
EXECUTE add_school_id_stmt;
DEALLOCATE PREPARE add_school_id_stmt;

-- Upgrade databases that ran the earlier string-based migration by mapping
-- matching Class.school names to Principal.pid.
SET @has_legacy_school_column = (
    SELECT COUNT(*) FROM information_schema.columns
     WHERE table_schema = DATABASE() AND table_name = 'Class' AND column_name = 'school'
);
SET @backfill_school_ids = IF(
    @has_legacy_school_column > 0,
    'UPDATE Class c INNER JOIN Principal p ON TRIM(c.school) = TRIM(p.school) SET c.school_id = p.pid WHERE c.school_id IS NULL AND c.school IS NOT NULL AND TRIM(c.school) <> ''''',
    'SELECT 1'
);
PREPARE backfill_school_ids_stmt FROM @backfill_school_ids;
EXECUTE backfill_school_ids_stmt;
DEALLOCATE PREPARE backfill_school_ids_stmt;

-- If the database has exactly one named school, assign currently unlinked
-- classes to that school's principal record. Multiple schools require explicit
-- class-to-school assignments because the existing data cannot identify them.
SET @school_count = (
    SELECT COUNT(DISTINCT TRIM(school)) FROM Principal
     WHERE school IS NOT NULL AND TRIM(school) <> ''
);
SET @assign_single_school = IF(
    @school_count = 1,
    'UPDATE Class SET school_id = (SELECT MIN(pid) FROM Principal WHERE school IS NOT NULL AND TRIM(school) <> '''') WHERE school_id IS NULL',
    'SELECT 1'
);
PREPARE assign_single_school_stmt FROM @assign_single_school;
EXECUTE assign_single_school_stmt;
DEALLOCATE PREPARE assign_single_school_stmt;

SET @add_school_fk = IF(
    (SELECT COUNT(*) FROM information_schema.table_constraints
      WHERE constraint_schema = DATABASE() AND table_name = 'Class'
        AND constraint_name = 'FK_Class_Principal_School') = 0,
    'ALTER TABLE Class ADD CONSTRAINT FK_Class_Principal_School FOREIGN KEY (school_id) REFERENCES Principal(pid)',
    'SELECT 1'
);
PREPARE add_school_fk_stmt FROM @add_school_fk;
EXECUTE add_school_fk_stmt;
DEALLOCATE PREPARE add_school_fk_stmt;

CREATE TABLE IF NOT EXISTS TutorSchoolCourseRequest (
    id INT NOT NULL AUTO_INCREMENT,
    tuid INT NOT NULL,
    school_id INT NULL,
    school VARCHAR(255) NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    createdAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    INDEX IX_TutorSchoolCourseRequest_Tutor (tuid),
    INDEX IX_TutorSchoolCourseRequest_School (school)
);

-- Add and populate the principal ID for databases where the request table
-- already existed before school_id was added to the request payload.
SET @add_request_school_id = IF(
    (SELECT COUNT(*) FROM information_schema.columns
      WHERE table_schema = DATABASE() AND table_name = 'TutorSchoolCourseRequest'
        AND column_name = 'school_id') = 0,
    'ALTER TABLE TutorSchoolCourseRequest ADD COLUMN school_id INT NULL AFTER tuid',
    'SELECT 1'
);
PREPARE add_request_school_id_stmt FROM @add_request_school_id;
EXECUTE add_request_school_id_stmt;
DEALLOCATE PREPARE add_request_school_id_stmt;

UPDATE TutorSchoolCourseRequest r
JOIN (
    SELECT TRIM(school) AS school_name, MIN(pid) AS pid
    FROM Principal
    WHERE school IS NOT NULL AND TRIM(school) <> ''
    GROUP BY TRIM(school)
) p ON TRIM(r.school) = p.school_name
SET r.school_id = p.pid
WHERE r.school_id IS NULL;

SET @add_request_school_fk = IF(
    (SELECT COUNT(*) FROM information_schema.table_constraints
      WHERE constraint_schema = DATABASE() AND table_name = 'TutorSchoolCourseRequest'
        AND constraint_name = 'FK_TutorSchoolCourseRequest_Principal') = 0,
    'ALTER TABLE TutorSchoolCourseRequest ADD CONSTRAINT FK_TutorSchoolCourseRequest_Principal FOREIGN KEY (school_id) REFERENCES Principal(pid)',
    'SELECT 1'
);
PREPARE add_request_school_fk_stmt FROM @add_request_school_fk;
EXECUTE add_request_school_fk_stmt;
DEALLOCATE PREPARE add_request_school_fk_stmt;

CREATE TABLE IF NOT EXISTS TutorSchoolCourseRequestItem (
    id INT NOT NULL AUTO_INCREMENT,
    requestId INT NOT NULL,
    cid INT NOT NULL,
    sid INT NOT NULL,
    feePerMonth DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY UX_TutorSchoolCourseRequestItem (requestId, cid, sid),
    CONSTRAINT FK_TutorSchoolCourseRequestItem_Request
        FOREIGN KEY (requestId) REFERENCES TutorSchoolCourseRequest(id)
        ON DELETE CASCADE,
    INDEX IX_TutorSchoolCourseRequestItem_ClassSubject (cid, sid)
);

-- Rename the earlier hourly-named column while preserving stored values.
-- A numeric hourly-to-monthly conversion needs a teaching-hours schedule that
-- is not stored in this schema, so existing values are retained as entered.
SET @has_hourly_fee_column = (
    SELECT COUNT(*) FROM information_schema.columns
     WHERE table_schema = DATABASE() AND table_name = 'TutorSchoolCourseRequestItem'
       AND column_name = 'feePerHour'
);
SET @has_monthly_fee_column = (
    SELECT COUNT(*) FROM information_schema.columns
     WHERE table_schema = DATABASE() AND table_name = 'TutorSchoolCourseRequestItem'
       AND column_name = 'feePerMonth'
);
SET @rename_fee_column = IF(
    @has_hourly_fee_column > 0 AND @has_monthly_fee_column = 0,
    'ALTER TABLE TutorSchoolCourseRequestItem CHANGE COLUMN feePerHour feePerMonth DECIMAL(10,2) NOT NULL',
    'SELECT 1'
);
PREPARE rename_fee_column_stmt FROM @rename_fee_column;
EXECUTE rename_fee_column_stmt;
DEALLOCATE PREPARE rename_fee_column_stmt;
