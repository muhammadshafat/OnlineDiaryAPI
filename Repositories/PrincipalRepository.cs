using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class PrincipalRepository
    {
        private readonly SqlConnectionFactory _db;

        public PrincipalRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<PrincipalDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT pid AS Pid,
                       pname AS Pname,
                       password AS Password,
                       email AS Email,
                       school AS school
                FROM Principal";

            return await conn.QueryAsync<PrincipalDto>(sql);
        }

        // GET BY ID
        public async Task<PrincipalDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT pid AS Pid,
                       pname AS Pname,
                       password AS Password,
                       email AS Email,
                       school AS school
                FROM Principal
                WHERE pid = @Pid";

            return await conn.QueryFirstOrDefaultAsync<PrincipalDto>(
                sql, new { Pid = id });
        }

        // INSERT
        public async Task Add(PrincipalDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                INSERT INTO Principal (pname, password, email, school)
                VALUES (@Pname, @Password, @Email, @school)";

            await conn.ExecuteAsync(sql, dto);
        }

        // UPDATE
        public async Task Update(PrincipalDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                UPDATE Principal
                SET pname = @Pname,
                    password = @Password,
                    email = @Email,
                    school = @school
                WHERE pid = @Pid";

            await conn.ExecuteAsync(sql, dto);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Principal WHERE pid = @Pid";

            await conn.ExecuteAsync(sql, new { Pid = id });
        }
    }
}