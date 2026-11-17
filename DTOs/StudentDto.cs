namespace OnlineDiaryAPI.DTOs
{
    public class StudentDto
    {
        public int Sid { get; set; }

        public string Sname { get; set; } = "";

        public string Password { get; set; } = "";

        public string Pid { get; set; }

        public int? Cid { get; set; }
    }
}