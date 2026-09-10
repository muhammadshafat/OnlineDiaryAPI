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

        // GET TEACHERS BY CLASS ID
        public async Task<IEnumerable<object>> GetTeachersByClass(int cid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
        SELECT DISTINCT 
            t.tid,
            t.tname
        FROM Teacher t
        INNER JOIN Teaches tc 
            ON tc.tid = t.tid
        WHERE tc.cid = @cid";
            return await conn.QueryAsync(sql, new { cid });
        }
        public async Task<IEnumerable<object>> GetsubjectByteacher(int cid, int tid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
        SELECT DISTINCT 
            s.sid,
            s.sname
        FROM subject s
        INNER JOIN Teaches tc 
            ON tc.sid = s.sid
        WHERE tc.cid = @cid and tc.tid=@tid";
            return await conn.QueryAsync(sql, new { cid, tid });
        }

        // GET BY ID
        public async Task<TeachesDto?> GetById(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Teaches WHERE Id = @id";

            return await conn.QueryFirstOrDefaultAsync<TeachesDto>(
                sql,
                new { id }
            );
        }

        // INSERT
        public async Task Add(TeachesDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                INSERT INTO Teaches (cid, tid, sid)
                VALUES (@Cid, @Tid, @Sid)";

            await conn.ExecuteAsync(sql, dto);
        }

        // UPDATE
        public async Task Update(TeachesDto dto)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                UPDATE Teaches
                SET 
                    cid = @Cid,
                    tid = @Tid,
                    sid = @Sid
                WHERE Id = @Id";

            await conn.ExecuteAsync(sql, dto);
        }

        // DELETE
        public async Task Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Teaches WHERE Id = @id";

            await conn.ExecuteAsync(sql, new { id });
        }
    }
}