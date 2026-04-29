using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class SubjectRepository
    {
        private readonly SqlConnectionFactory _db;

        public SubjectRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<SubjectDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Subject";

            return await conn.QueryAsync<SubjectDto>(sql);
        }

        // GET BY ID
        public async Task<SubjectDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Subject WHERE sid = " + id;

            return await conn.QueryFirstOrDefaultAsync<SubjectDto>(sql);
        }

        // INSERT
        public async Task Add(SubjectDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Subject (sname) VALUES ('" + dto.Sname + "')";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(SubjectDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Subject SET sname='" + dto.Sname +
                "' WHERE sid=" + dto.Sid;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Subject WHERE sid=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}