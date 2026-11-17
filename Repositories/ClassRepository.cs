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

            const string sql = "SELECT cid AS Cid, cname AS Cname, section AS Section, school_id AS SchoolId FROM Class";

            return await conn.QueryAsync<ClassDto>(sql);
        }

        // GET BY ID
      public async Task<IEnumerable<ClassDto>> GetByschoolId(int id)
{
    using var conn = _db.CreateConnection();

    const string sql = @"
        SELECT cid AS Cid, cname AS Cname, section AS Section, school_id AS SchoolId
        FROM Class
        WHERE school_id = @Id";

    return await conn.QueryAsync<ClassDto>(sql, new { Id = id });
}   
        // INSERT
        public async Task Add(ClassDto dto)
        {
            using var conn = _db.CreateConnection();

            const string sql = @"
                INSERT INTO Class (cname, section, school_id)
                VALUES (@Cname, @Section, @SchoolId)";

            await conn.ExecuteAsync(sql, dto);
        }

        // UPDATE
        public async Task Update(ClassDto dto)
        {
            using var conn = _db.CreateConnection();

            const string sql = @"
                UPDATE Class
                SET cname = @Cname, section = @Section, school_id = COALESCE(@SchoolId, school_id)
                WHERE cid = @Cid";

            await conn.ExecuteAsync(sql, dto);
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
