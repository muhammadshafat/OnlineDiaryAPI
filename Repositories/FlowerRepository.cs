using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;
namespace OnlineDiaryAPI.Repositories
{
    public class FlowerRepository
    {
        private readonly SqlConnectionFactory _db;

        public FlowerRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FlowerDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Flower";

            return await conn.QueryAsync<FlowerDto>(sql);
        }

        public async Task<int> Add(FlowerDto flower)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            INSERT INTO Flower(FlowerName, Color, Price, GardenId)
            VALUES(@FlowerName,@Color,@Price,@GardenId)";

            return await conn.ExecuteAsync(sql, flower);
        }

        public async Task<IEnumerable<FlowerDto>> GetByPrice(decimal price)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Flower WHERE Price > @Price";

            return await conn.QueryAsync<FlowerDto>(sql, new { Price = price });
        }

        public async Task<IEnumerable<FlowerDto>> GetByColorAndPrice(string color, decimal price)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            SELECT * FROM Flower
            WHERE Color = @Color AND Price > @Price";

            return await conn.QueryAsync<FlowerDto>(sql,
                new { Color = color, Price = price });
        }

        public async Task<IEnumerable<FlowerDto>> GetRedOrYellow()
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            SELECT * FROM Flower
            WHERE Color='Red' OR Color='Yellow'";

            return await conn.QueryAsync<FlowerDto>(sql);
        }

        public async Task<IEnumerable<FlowerDto>> GetRedOrYellowAndPrice(decimal price)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
            SELECT * FROM Flower
            WHERE (Color='Red' OR Color='Yellow')
            AND Price > @Price";

            return await conn.QueryAsync<FlowerDto>(sql,
                new { Price = price });
        }
    }
}