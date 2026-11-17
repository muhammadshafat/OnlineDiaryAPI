using Dapper;
using Microsoft.AspNetCore.Identity;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class TutorRepository
    {
        private readonly SqlConnectionFactory _db;
        private readonly PasswordHasher<Tutor> _passwordHasher = new();

        public TutorRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL TUTORS
        public async Task<IEnumerable<Tutor>> GetAllAsync()
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT
                    tuid AS Id,
                    name AS Name,
                    phoneNo AS PhoneNumber,
                    email AS Email
                FROM Tutor";

            return await conn.QueryAsync<Tutor>(sql);
        }

        // GET TUTORS BY CLASS
        public async Task<IEnumerable<Tutor>> GetByClassAsync(int cid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT DISTINCT
                    t.tuid AS Id,
                    t.name AS Name,
                    t.phoneNo AS PhoneNumber,
                    t.email AS Email
                FROM Tutor t
                INNER JOIN Tutor_class tc ON t.tuid = tc.tuid
                WHERE tc.cid = @Cid";

            var list = (await conn.QueryAsync<Tutor>(sql, new { Cid = cid })).ToList();
            if (!list.Any())
            {
                // Fallback to all tutors if no specific mapping for this class
                return await GetAllAsync();
            }
            return list;
        }

        // GET TUTOR BY ID
        public async Task<Tutor?> GetByIdAsync(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT
                    tuid AS Id,
                    name AS Name,
                    phoneNo AS PhoneNumber,
                    email AS Email
                FROM Tutor
                WHERE tuid = @Id";

            return await conn.QueryFirstOrDefaultAsync<Tutor>(
                sql, new { Id = id });
        }

        // GET STUDENTS ASSIGNED TO TUTOR
        public async Task<IEnumerable<object>> GetStudentsByTutorAsync(int tuid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT DISTINCT 
                    s.sid, 
                    s.sname, 
                    s.pid, 
                    s.cid, 
                    c.cname, 
                    c.section, 
                    sub.sname AS subjectName, 
                    p.pname AS parentName
                FROM Tutor_Student_Subject tss
                INNER JOIN Student s ON tss.stid = s.sid
                LEFT JOIN Class c ON s.cid = c.cid
                LEFT JOIN Subject sub ON tss.subid = sub.sid
                LEFT JOIN Parent p ON s.pid = p.pid
                WHERE tss.tuid = @Tuid";

            return await conn.QueryAsync(sql, new { Tuid = tuid });
        }

        // CREATE TUTOR
       public async Task<Tutor> CreateAsync(Tutor dto)
{
    using var conn = _db.CreateConnection();
    conn.Open();
    using var transaction = conn.BeginTransaction();

    try
    {
        const string sql = @"
            INSERT INTO Tutor (name, phoneNo, email, password)
            VALUES (@Name, @PhoneNumber, @Email, @Password);
            SELECT LAST_INSERT_ID();";

        int id = await conn.ExecuteScalarAsync<int>(sql, new
        {
            dto.Name,
            dto.PhoneNumber,
            dto.Email,
            dto.Password          // stored as typed, no hashing
        }, transaction);

        const string sql1 = @"
            INSERT INTO Tutor_class (tuid, cid)
            VALUES (@Tuid, @Cid);";

        foreach (var classId in dto.cid ?? Array.Empty<int>())
        {
            await conn.ExecuteAsync(sql1, new { Tuid = id, Cid = classId }, transaction);
        }

        transaction.Commit();

        return new Tutor
        {
            Id = id,
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            cid = dto.cid
        };
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}        // UPDATE TUTOR
        public async Task<bool> UpdateAsync(int id, Tutor dto)
        {
            using var conn = _db.CreateConnection();

            string selectSql = @"
                SELECT *
                FROM Tutor
                WHERE Id = @Id";

            var tutor = await conn.QueryFirstOrDefaultAsync<Tutor>(
                selectSql, new { Id = id });

            if (tutor == null)
                return false;

            tutor.Name = dto.Name;
            tutor.PhoneNumber = dto.PhoneNumber;
            tutor.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                tutor.Password =
                    _passwordHasher.HashPassword(tutor, dto.Password);
            }

            string updateSql = @"
                UPDATE Tutor
                SET Name = @Name,
                    PhoneNumber = @PhoneNumber,
                    Email = @Email,
                    Password = @Password
                WHERE Id = @Id";

            int rows = await conn.ExecuteAsync(updateSql, new
            {
                Id = id,
                tutor.Name,
                tutor.PhoneNumber,
                tutor.Email,
                tutor.Password
            });

            return rows > 0;
        }

        // DELETE TUTOR
        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                DELETE FROM Tutor
                WHERE Id = @Id";

            int rows = await conn.ExecuteAsync(sql, new { Id = id });

            return rows > 0;
        }
    }
}