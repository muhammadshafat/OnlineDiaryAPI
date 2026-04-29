using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class ClassRepository
    {
        private readonly SqlConnectionFactory _db;

        public ClassRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<ClassDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Class";

            return await conn.QueryAsync<ClassDto>(sql);
        }

        // GET BY ID
        public async Task<ClassDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Class WHERE cid = " + id;

            return await conn.QueryFirstOrDefaultAsync<ClassDto>(sql);
        }

        // INSERT
        public async Task Add(ClassDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = "INSERT INTO Class (cname, section) VALUES ('" 
                         + dto.Cname + "', '" + dto.Section + "')";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(ClassDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = "UPDATE Class SET cname='" + dto.Cname +
                         "', section='" + dto.Section +
                         "' WHERE cid=" + dto.Cid;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Class WHERE cid=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}