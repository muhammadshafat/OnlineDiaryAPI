using Dapper;
using Microsoft.AspNetCore.Identity;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class AuthRepository
    {
        private readonly SqlConnectionFactory _db;
        private readonly IPasswordHasher<Tutor> _tutorHasher;

        public AuthRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // STUDENT LOGIN
        public async Task<object> PrincipalLogin(string name, string password)
        {
            using var conn = _db.CreateConnection();

            string sql =
                "SELECT pid, pname FROM principal WHERE email='"
                + name + "' AND password='" + password + "'";

            return await conn.QueryFirstOrDefaultAsync(sql);
        }

       public async Task<object?> TutorLogin(string email, string password)
{ {
            using var conn = _db.CreateConnection();

            string sql =
                "SELECT tuid, name FROM tutor WHERE email='"
                + email + "' AND password='" + password + "'";

            return await conn.QueryFirstOrDefaultAsync(sql);
        }}
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