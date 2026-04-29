using Dapper;
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

        // GET ALL
        public async Task<IEnumerable<UpdateDiaryDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM UpdateDiary";

            return await conn.QueryAsync<UpdateDiaryDto>(sql);
        }

        // GET BY ID
        public async Task<UpdateDiaryDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM UpdateDiary WHERE Id=" + id;

            return await conn.QueryFirstOrDefaultAsync<UpdateDiaryDto>(sql);
        }

        // INSERT
        public async Task Add(UpdateDiaryDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO UpdateDiary (cid, tid, sid, did) VALUES ("
                + dto.Cid + ", "
                + dto.Tid + ", "
                + dto.Sid + ", "
                + dto.Did + ")";

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM UpdateDiary WHERE Id=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}