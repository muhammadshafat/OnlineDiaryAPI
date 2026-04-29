namespace OnlineDiaryAPI.DTOs
{
    public class TeacherDto
    {
        public int Tid { get; set; }
        public string Tname { get; set; }
        public string Email { get; set; }
        public string? Phoneno { get; set; }
        public string Password { get; set; }
        public int? Cid { get; set; }
    }
}