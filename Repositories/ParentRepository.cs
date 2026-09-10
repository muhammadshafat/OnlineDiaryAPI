using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class ParentRepository
    {
        private readonly SqlConnectionFactory _db;

        public ParentRepository(SqlConnectionFactory db)
        {
            _db = db;
        }
        public async Task<IEnumerable<object>> GetParentChildren(int pid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
        SELECT 
            *
        FROM Student s
        INNER JOIN Class c ON s.cid = c.cid
        WHERE s.pid = @Pid
    ";

            var result = await conn.QueryAsync(sql, new { Pid = pid });
            return result;
        }
        public async Task<IEnumerable<ParentDto>> GetAll()
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<ParentDto>("SELECT * FROM Parent");
        }

        public async Task<ParentDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<ParentDto>(
                "SELECT * FROM Parent WHERE pid=" + id);
        }

        public async Task Add(ParentDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Parent (pname,email,phoneno,password) VALUES ('"
                + dto.Pname + "','"
                + dto.Email + "','"
                + dto.Phoneno + "','"
                + dto.Password + "')";

            await conn.ExecuteAsync(sql);
        }

        public async Task Update(ParentDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Parent SET pname='" + dto.Pname +
                "', email='" + dto.Email +
                "', phoneno='" + dto.Phoneno +
                "', password='" + dto.Password +
                "' WHERE pid=" + dto.Pid;

            await conn.ExecuteAsync(sql);
        }

        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();
            await conn.ExecuteAsync("DELETE FROM Parent WHERE pid=" + id);
        }
    }
}