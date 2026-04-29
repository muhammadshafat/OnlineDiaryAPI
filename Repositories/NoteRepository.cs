using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class NoteRepository
    {
        private readonly SqlConnectionFactory _db;

        public NoteRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<NoteDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Note";

            return await conn.QueryAsync<NoteDto>(sql);
        }

        // GET BY ID
        public async Task<NoteDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Note WHERE Id = " + id;

            return await conn.QueryFirstOrDefaultAsync<NoteDto>(sql);
        }

        // INSERT
        public async Task Add(NoteDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Note (tid, sid, did) VALUES ("
                + dto.Tid + ", "
                + dto.Sid + ", "
                + dto.Did + ")";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(NoteDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Note SET tid=" + dto.Tid +
                ", sid=" + dto.Sid +
                ", did=" + dto.Did +
                " WHERE Id=" + dto.Id;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Note WHERE Id=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}