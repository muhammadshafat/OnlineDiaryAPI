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
        public async Task<IEnumerable<object>> GetParentChildren(string pid)
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
public async Task<ParentDto?> GetById(string id)
{
    using var conn = _db.CreateConnection();

    const string sql = "SELECT * FROM Parent WHERE pid = @Id";

    return await conn.QueryFirstOrDefaultAsync<ParentDto>(
        sql,
        new { Id = id }
    );
}


public async Task Add(ParentDto dto)
{
    using var conn = _db.CreateConnection();

    const string sql = @"
        INSERT INTO Parent (pid,pname, email, phoneno, password)
        VALUES (@pid,@Pname, @Email, @Phoneno, @Password);";

    await conn.ExecuteAsync(sql, new
    {
        dto.Pid,
        dto.Pname,
        dto.Email,
        dto.Phoneno,
        dto.Password
    });
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