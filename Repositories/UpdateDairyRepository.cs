using Dapper;
using Microsoft.AspNetCore.Mvc;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class UpdateDiaryRepository
    {
        private readonly SqlConnectionFactory _db;

        public UpdateDiaryRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // ✅ GET CLASSES BY TEACHER
        public async Task<IEnumerable<ClassDto>> GetClassesByTeacher(int tid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT DISTINCT 
                    c.cid AS cid,
                    c.cname AS cname,
                    c.section AS section,
                    c.school_id AS schoolId
                FROM class c
                INNER JOIN teaches t ON t.cid = c.cid
                WHERE t.tid = @Tid";   // ✅ FIXED (closed string)

            return await conn.QueryAsync<ClassDto>(sql, new { Tid = tid });
        }

        // ✅ GET SUBJECTS BY TEACHER + CLASS
        public async Task<IEnumerable<SubjectDto>> GetSubjectsByTeacher(int tid, int cid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT 
                    s.sid AS sid,
                    s.sname AS sname
                FROM teaches t
                INNER JOIN subject s ON t.sid = s.sid
                WHERE t.tid = @Tid AND t.cid = @Cid";   // ✅ FIXED

            return await conn.QueryAsync<SubjectDto>(sql, new { Tid = tid, Cid = cid });
        }

        // ✅ GET ALL
        public async Task<IEnumerable<UpdateDiaryDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM UpdateDiary";

            return await conn.QueryAsync<UpdateDiaryDto>(sql);
        }

        // ✅ GET BY ID (SAFE)
        public async Task<UpdateDiaryDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM UpdateDiary WHERE Id = @Id";

            return await conn.QueryFirstOrDefaultAsync<UpdateDiaryDto>(sql, new { Id = id });
        }

        // ✅ INSERT (SAFE)
        public async Task<string> UpdateDiary(UpdateDiaryDto dto)
        {
            using var conn = _db.CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction(); // ✅ IMPORTANT

            try
            {
                // STEP 1: Insert into Diary
                string diarySql = @"
            INSERT INTO Diary (ddate, description, category)
            VALUES (@Date, @Description, @Category);
            SELECT LAST_INSERT_ID();";

                var did = await conn.ExecuteScalarAsync<int>(diarySql, new
                {
                    Date = DateTime.Now,
                    Description = dto.description,
                    category = dto.Category
                }, transaction);

                if (did <= 0)
                    throw new Exception("Diary insert failed");

                // STEP 2: Insert into UpdateDiary
                string updateSql = @"
            INSERT INTO UpdateDiary (cid, tid, sid, did)
            VALUES (@Cid, @Tid, @Sid, @Did);";

                await conn.ExecuteAsync(updateSql, new
                {
                    Cid = dto.cid,
                    Tid = dto.tid,
                    Sid = dto.sid,
                    Did = did
                }, transaction);

                transaction.Commit();

                return "Diary created successfully";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Transaction failed: " + ex.Message);
            }
        }
        [HttpGet("view-diary/{tid}")]
        public async Task<IEnumerable<object>> ViewDiary(int tid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
       
        SELECT 
            d.did,
            d.ddate,
            d.description,
            d.category,
            c.cname,
            c.section,
            s.sname
        FROM Diary d
        INNER JOIN UpdateDiary ud ON d.did = ud.did
        INNER JOIN Class c ON ud.cid = c.cid  INNER JOIN Subject s ON ud.sid = s.sid
        WHERE ud.tid = @Tid
        ORDER BY d.ddate DESC";


            var result = await conn.QueryAsync(sql, new { Tid = tid });
            return result;
        }
        [HttpGet("view-diary/{tid}")]
        public async Task<IEnumerable<object>> parentViewDiary(int sid, string category)
        {
            using var conn = _db.CreateConnection();

            category = category?.Trim().ToLower();

            // ───────── HOMEWORK ─────────
            if (category == "homework")
            {
                string sql = @"
           SELECT 
    d.did,
    d.ddate,
    d.description,
    d.category,
    c.cname,
    s.sname
FROM Student st
INNER JOIN UpdateDiary ud ON st.cid = ud.cid
INNER JOIN Diary d ON ud.did = d.did
INNER JOIN Class c ON ud.cid = c.cid
INNER JOIN Subject s ON ud.sid = s.sid
WHERE st.sid = @Sid
  AND d.category = 'Homework'
AND d.ddate >= CURDATE()
AND d.ddate <  CURDATE() + INTERVAL 1 DAY
ORDER BY d.ddate DESC;";

                return await conn.QueryAsync(sql, new { Sid = sid });
            }

            // ───────── NOTE ─────────
            if (category == "note")
            {
                string sql = @"
            SELECT 
                d.description,
                d.category,
                s.sname
                t.tname
            FROM note n
            INNER JOIN diary d ON n.did = d.did
            INNER JOIN UpdateDiary ud ON ud.did = d.did
            INNER JOIN subject s ON s.sid = ud.sid
            inner JOIN teacher t ON t.tid = ud.tid
            WHERE n.sid = @Sid
              AND d.category = 'Note'
            ORDER BY d.ddate DESC";

                return await conn.QueryAsync(sql, new { Sid = sid });
            }

            return Enumerable.Empty<object>();
        }
  public async Task<int> AddNoteDiary(StdDiaryDto dto)
{
    using var conn = _db.CreateConnection();
    conn.Open();
    using var transaction = conn.BeginTransaction();

    try
    {
        string diarySql = @"
            INSERT INTO Diary (ddate, description, category)
            VALUES (@Date, @Description, @Category);
            SELECT LAST_INSERT_ID();";

        var did = await conn.ExecuteScalarAsync<int>(
            diarySql,
            new { Date = DateTime.Now, dto.Description, dto.Category },
            transaction);

        if (string.Equals(dto.Category, "note", StringComparison.OrdinalIgnoreCase))
        {
            await conn.ExecuteAsync(
                @"INSERT INTO Note (tid, sid, did) VALUES (@Tid, @Sid, @Did);",
                new { dto.Tid, Sid = dto.Sid_Student, Did = did },
                transaction);
        }

        if (string.Equals(dto.Category, "homework", StringComparison.OrdinalIgnoreCase))
        {
            await conn.ExecuteAsync(
                @"INSERT INTO UpdateDiary (cid, tid, sid, did) VALUES (@Cid, @Tid, @Sid, @Did);",
                new { dto.Cid, dto.Tid, dto.Sid, Did = did },
                transaction);
        }

        transaction.Commit();
        return did;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}}}