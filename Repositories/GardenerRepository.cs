using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class GardenerRepository
    {
        private readonly SqlConnectionFactory _db;

        public GardenerRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GardenerDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Gardener";

            return await conn.QueryAsync<GardenerDto>(sql);
        }

        public async Task<int> Add(GardenerDto gardener)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            INSERT INTO Gardener(GardenerName, ExperienceYears, GardenId)
            VALUES(@GardenerName,@ExperienceYears,@GardenId)";

            return await conn.ExecuteAsync(sql, gardener);
        }
    }
}