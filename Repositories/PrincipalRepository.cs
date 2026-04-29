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

            string sql = "SELECT * FROM Principal";

            return await conn.QueryAsync<PrincipalDto>(sql);
        }

        // GET BY ID
        public async Task<PrincipalDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Principal WHERE pid=" + id;

            return await conn.QueryFirstOrDefaultAsync<PrincipalDto>(sql);
        }

        // INSERT
        public async Task Add(PrincipalDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Principal (pname, password, email) VALUES ('"
                + dto.Pname + "', '"
                + dto.Password + "', '"
                + dto.Email + "')";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(PrincipalDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Principal SET pname='" + dto.Pname +
                "', password='" + dto.Password +
                "', email='" + dto.Email +
                "' WHERE pid=" + dto.Pid;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Principal WHERE pid=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}