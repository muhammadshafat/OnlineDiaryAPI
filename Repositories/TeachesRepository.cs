using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class TeachesRepository
    {
        private readonly SqlConnectionFactory _db;

        public TeachesRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<TeachesDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Teaches";

            return await conn.QueryAsync<TeachesDto>(sql);
        }

        // GET BY ID
        public async Task<TeachesDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Teaches WHERE Id = " + id;

            return await conn.QueryFirstOrDefaultAsync<TeachesDto>(sql);
        }

        // INSERT
        public async Task Add(TeachesDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Teaches (cid, tid, sid) VALUES ("
                + dto.Cid + ", "
                + dto.Tid + ", "
                + dto.Sid + ")";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(TeachesDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Teaches SET cid=" + dto.Cid +
                ", tid=" + dto.Tid +
                ", sid=" + dto.Sid +
                " WHERE Id=" + dto.Id;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Teaches WHERE Id=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}