using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;
namespace OnlineDiaryAPI.Repositories
{
    public class GardenRepository
    {
        private readonly SqlConnectionFactory _db;

        public GardenRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<GardenDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Garden";

            return await conn.QueryAsync<GardenDto>(sql);
        }

        public async Task<int> Add(GardenDto garden)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            INSERT INTO Garden(GardenName, Location, Size)
            VALUES(@GardenName,@Location,@Size)";

            return await conn.ExecuteAsync(sql, garden);
        }
    }
}