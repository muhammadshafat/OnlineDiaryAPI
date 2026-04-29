using Dapper;
using OnlineDiaryAPI.Data;

namespace OnlineDiaryAPI.Repositories
{
    public class AuthRepository
    {
        private readonly SqlConnectionFactory _db;

        public AuthRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // STUDENT LOGIN
        public async Task<object> StudentLogin(string name, string password)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "SELECT sid, sname FROM Student WHERE sname='"
                + name + "' AND password='" + password + "'";

            return await conn.QueryFirstOrDefaultAsync(sql);
        }

        // TEACHER LOGIN
        public async Task<object> TeacherLogin(string email, string password)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "SELECT tid, tname FROM Teacher WHERE email='"
                + email + "' AND password='" + password + "'";

            return await conn.QueryFirstOrDefaultAsync(sql);
        }

        // PARENT LOGIN
        public async Task<object> ParentLogin(string email, string password)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "SELECT pid, pname FROM Parent WHERE email='"
                + email + "' AND password='" + password + "'";

            return await conn.QueryFirstOrDefaultAsync(sql);
        }
    }
}