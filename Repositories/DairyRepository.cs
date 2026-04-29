using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class DiaryRepository
    {
        private readonly SqlConnectionFactory _db;

        public DiaryRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<DiaryDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Diary";

            return await conn.QueryAsync<DiaryDto>(sql);
        }

        // GET BY ID
        public async Task<DiaryDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Diary WHERE did = " + id;

            return await conn.QueryFirstOrDefaultAsync<DiaryDto>(sql);
        }

        // INSERT
        public async Task Add(DiaryDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Diary (ddate, description, category) VALUES ('"
                + dto.Ddate.ToString("yyyy-MM-dd") + "', '"
                + dto.Description + "', '"
                + dto.Category + "')";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(DiaryDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Diary SET ddate='" + dto.Ddate.ToString("yyyy-MM-dd") +
                "', description='" + dto.Description +
                "', category='" + dto.Category +
                "' WHERE did=" + dto.Did;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Diary WHERE did=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}