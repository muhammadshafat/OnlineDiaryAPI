using Dapper;
using OnlineDiaryAPI.Data;
using OnlineDiaryAPI.DTOs;

namespace OnlineDiaryAPI.Repositories
{
    public class ComplaintRepository
    {
        private readonly SqlConnectionFactory _db;

        public ComplaintRepository(SqlConnectionFactory db)
        {
            _db = db;
        }

        // CREATE COMPLAINT
        public async Task<bool> CreateComplaint(ComplaintCreateDto dto)
        {
            using var conn = _db.CreateConnection();

            string status = "Pending";
            DateTime date = DateTime.Now;

            // STEP 1: Insert Complaint + get CID
            string insertComplaint = @"
        INSERT INTO Complaint 
        (cdate, status, category, description)
        OUTPUT INSERTED.Cid
        VALUES 
        (@Date, @Status, @Category, @Description);
    ";

            int newCid = await conn.ExecuteScalarAsync<int>(insertComplaint, new
            {
                Date = date,
                Status = status,
                dto.Category,
                dto.Description
            });

            // STEP 2: If NOT teacher (simple case)
            if (dto.Tid == null)
            {
                string fileSql = @"
            INSERT INTO FileMapping
            (sid, pid, uid, cid)
            VALUES 
            (@Sid, @Pid, NULL, @Cid);
        ";

                var rows = await conn.ExecuteAsync(fileSql, new
                {
                    dto.Sid,
                    dto.Pid,
                    Cid = newCid
                });

                return rows > 0;
            }
            else
            {
                // STEP 3: If teacher complaint → fetch UID from UpdateDiary
                string getUidSql = @"
        SELECT id 
        FROM teaches
        WHERE cid = @Cid AND sid = @Sid AND tid = @Tid;
    ";

                int? id = await conn.QueryFirstOrDefaultAsync<int?>(getUidSql, new
                {
                    Cid = dto.Cid,
                    dto.Sid,
                    dto.Tid
                });

                // STEP 4: Insert into FileMapping with UID
                string fileSqlTeacher = @"
        INSERT INTO FileMapping
        (sid, pid, uid, cid)
        VALUES 
        (@Sid, @Pid, @id, @Cid);
    ";

                var result = await conn.ExecuteAsync(fileSqlTeacher, new
                {
                    dto.Sid,
                    dto.Pid,
                    id = id,
                    Cid = newCid
                });

                return result > 0;
            }
        }
        // GET ALL PENDING COMPLAINTS (Admin/Principal)
        public async Task<IEnumerable<dynamic>> GetPendingComplaints()
        {
            using var conn = _db.CreateConnection();

            string query = @"
        SELECT 
            c.cid,
            c.cdate,
            c.status,
            c.category,
            c.description,
            s.sname,
            p.pname
        FROM Complaint c
        INNER JOIN FileMapping f ON c.cid = f.cid
        INNER JOIN Student s ON f.sid = s.sid
        INNER JOIN Parent p ON f.pid = p.pid
        WHERE c.status = 'Pending'
        AND f.uid IS NULL
        ORDER BY c.cid DESC
    ";

            return await conn.QueryAsync(query);
        }

        // GET PENDING COMPLAINTS FOR SPECIFIC TEACHER
        public async Task<IEnumerable<dynamic>> GetPendingComplaintsByTeacher(int tid)
        {
            using var conn = _db.CreateConnection();

            string query = @"
        SELECT 
            c.cid,
            c.cdate,
            c.status,
            c.category,
            c.description,
            s.sname AS StudentName,
            p.pname AS ParentName,
            t.tname AS TeacherName
        FROM Complaint c
        INNER JOIN FileMapping f ON c.cid = f.cid
        INNER JOIN Student s ON f.sid = s.sid
        INNER JOIN Parent p ON f.pid = p.pid
        INNER JOIN teaches tc ON f.uid = tc.id
        INNER JOIN Teacher t ON tc.tid = t.tid
        WHERE c.status = 'Pending'
          AND t.tid = @Tid
        ORDER BY c.cdate DESC;
    ";

            return await conn.QueryAsync(query, new { Tid = tid });
        }  // GET ALL COMPLAINTS
        public async Task<IEnumerable<ComplaintCreateDto>> GetAll()
        {
            using var conn = _db.CreateConnection();

            string sql = "SELECT * FROM Complaint";

            return await conn.QueryAsync<ComplaintCreateDto>(sql);
        }

        // GET BY STUDENT ID (optional useful API)
        public async Task<IEnumerable<ComplaintCreateDto>> GetByStudent(int sid)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                SELECT * 
                FROM Complaint 
                WHERE Sid = @Sid
                ORDER BY Cid DESC;
            ";

            return await conn.QueryAsync<ComplaintCreateDto>(sql, new { Sid = sid });
        }

        // DELETE COMPLAINT
        public async Task<bool> Delete(int id)
        {
            using var conn = _db.CreateConnection();

            string sql = "DELETE FROM Complaint WHERE Cid = @Id";

            var rows = await conn.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}