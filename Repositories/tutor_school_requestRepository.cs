
using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class Tutor_school_requestRepository
    {
        private readonly SqlConnectionFactory _db;

        public Tutor_school_requestRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // 1. INSERT MULTIPLE COURSE REQUESTS
        public async Task<bool> CreateAsync(
            Tutor_school_requestDto requestDto)
        {
            if (requestDto == null)
                throw new ArgumentNullException(nameof(requestDto));

            if (requestDto.Courses == null ||
                !requestDto.Courses.Any())
            {
                throw new ArgumentException(
                    "At least one course is required.");
            }

            if (requestDto.TutorId <= 0 ||
                requestDto.SchoolId <= 0)
            {
                throw new ArgumentException(
                    "Valid TutorId and SchoolId are required.");
            }

            if (requestDto.Courses.Any(c =>
                c.ClassId <= 0 ||
                c.SubjectId <= 0 ||
                c.FeePerMonth <= 0))
            {
                throw new ArgumentException(
                    "Invalid class, subject or fee.");
            }

            using var conn = _db.CreateConnection();

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                string sql = @"
                    INSERT INTO Tutor_request_school
                    (
                        tuid,
                        subid,
                        cid,
                        school_id,
                        fee,
                        status
                    )
                    VALUES
                    (
                        @TutorId,
                        @SubjectId,
                        @ClassId,
                        @SchoolId,
                        @FeePerMonth,
                        @Status
                    )";

                foreach (var course in requestDto.Courses)
                {
                    await conn.ExecuteAsync(
                        sql,
                        new
                        {
                            TutorId = requestDto.TutorId,
                            SubjectId = course.SubjectId,
                            ClassId = course.ClassId,
                            SchoolId = requestDto.SchoolId,
                            FeePerMonth = course.FeePerMonth,
                            Status = "pending"
                        },
                        transaction
                    );
                }

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        // 2. GET REQUESTS BY SCHOOL ID
   public async Task<IEnumerable<Tutor_school_requestDto>>
    GetBySchoolIdAsync(int schoolId, string status)
{
    using var conn = _db.CreateConnection();

    string sql = @"
        SELECT
            tr.id AS Id,
            tr.tuid AS TutorId,
            tr.school_id AS SchoolId,
            tr.status AS Status,
            tr.cid AS ClassId,
            tr.subid AS SubjectId,
            tr.fee AS FeePerMonth,
            s.sname AS SubjectName,
            c.cname AS ClassName,
            c.section AS ClassSection,
            t.name AS TutorName
        FROM Tutor_request_school tr
        INNER JOIN subject s ON s.sid = tr.subid
        INNER JOIN class c ON c.cid = tr.cid
        INNER JOIN tutor t ON t.tuid = tr.tuid
        WHERE tr.school_id = @SchoolId
          AND tr.status = @Status
        ORDER BY tr.id DESC";

    return await conn.QueryAsync<Tutor_school_requestDto>(
        sql,
        new
        {
            SchoolId = schoolId,
            Status = status
        }
    );
}
public async Task<bool> UpdateSchoolTutorRequestStatusAsync(int id, string status)
{
    using var connection = _db.CreateConnection();

    const string sql = @"
        UPDATE Tutor_request_school
        SET status = @Status
        WHERE id = @Id;
    ";

    var rows = await connection.ExecuteAsync(sql, new
    {
        Id = id,
        Status = status
    });

    return rows > 0;
}
        // 3. GET REQUESTS BY TUTOR ID
        public async Task<IEnumerable<Tutor_school_requestDto>>
            GetByTutorIdAsync(int tutorId)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT
                    tr.id AS Id,
                    tr.tuid AS TutorId,
                    tr.school_id AS SchoolId,
                    tr.status AS Status,
                    tr.cid AS ClassId,
                    tr.subid AS SubjectId,
                    tr.fee AS FeePerMonth,
                    s.sname AS SubjectName,
                    c.cname AS ClassName,
                    c.section AS ClassSection,
                    t.name AS TutorName
                FROM subject s inner join Tutor_request_school tr on s.sid = tr.subid
                inner join class c on c.cid = tr.cid inner join tutor t on t.tuid = tr.tuid
               
                WHERE tr.tuid = @TutorId
                ORDER BY tr.id DESC";

            var rows = await conn.QueryAsync<Tutor_school_requestDto>(
                sql,
                new { TutorId = tutorId }
            );

            return rows;
        }
    }
}
