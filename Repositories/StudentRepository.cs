using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class StudentRepository
    {
        private readonly SqlConnectionFactory _db;

        public StudentRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<StudentDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            var sql = "SELECT sid, sname, password, pid, cid FROM Student";

            return await conn.QueryAsync<StudentDto>(sql);
        }

        // GET BY ID
        public async Task<StudentDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            var sql = "SELECT sid, sname, password, pid, cid FROM Student WHERE sid = @id";

            return await conn.QueryFirstOrDefaultAsync<StudentDto>(sql, new { id });
        }

        // INSERT
        public async Task<int> Add(StudentDto s)
        {
            using var conn = _db.CreateConnection();

            var sql = @"
INSERT INTO Student (sname, password, pid, cid)
VALUES (@sname, @password, @pid, @cid)";

            return await conn.ExecuteAsync(sql, s);
        }

        // UPDATE
        public async Task<int> Update(StudentDto s)
        {
            using var conn = _db.CreateConnection();

            var sql = @"
UPDATE Student
SET sname = @sname,
    password = @password,
    pid = @pid,
    cid = @cid
WHERE sid = @sid";

            return await conn.ExecuteAsync(sql, s);
        }

        // DELETE
        public async Task<int> Delete(int id)
        {
            using var conn = _db.CreateConnection();

            var sql = "DELETE FROM Student WHERE sid = @id";

            return await conn.ExecuteAsync(sql, new { id });
        }
    }
}