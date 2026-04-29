using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class TeacherRepository
    {
        private readonly SqlConnectionFactory _db;

        public TeacherRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // GET ALL
        public async Task<IEnumerable<TeacherDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Teacher";

            return await conn.QueryAsync<TeacherDto>(sql);
        }

        // GET BY ID
        public async Task<TeacherDto> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Teacher WHERE tid = " + id;

            return await conn.QueryFirstOrDefaultAsync<TeacherDto>(sql);
        }

        // INSERT
        public async Task Add(TeacherDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "INSERT INTO Teacher (tname, email, phoneno, password, cid) VALUES ('"
                + dto.Tname + "', '"
                + dto.Email + "', '"
                + dto.Phoneno + "', '"
                + dto.Password + "', "
                + (dto.Cid ?? 0) + ")";

            await conn.ExecuteAsync(sql);
        }

        // UPDATE
        public async Task Update(TeacherDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "UPDATE Teacher SET tname='" + dto.Tname +
                "', email='" + dto.Email +
                "', phoneno='" + dto.Phoneno +
                "', password='" + dto.Password +
                "', cid=" + (dto.Cid ?? 0) +
                " WHERE tid=" + dto.Tid;

            await conn.ExecuteAsync(sql);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Teacher WHERE tid=" + id;

            await conn.ExecuteAsync(sql);
        }
    }
}