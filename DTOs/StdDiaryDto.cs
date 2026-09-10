namespace OnlineDiaryAPI.DTOs
{
    public class StdDiaryDto
    {
        public string? Description { get; set; }
        public string? Category { get; set; }

        public int Tid { get; set; }
        public int Cid { get; set; }
        public int Sid { get; set; }


        public int? Sid_Student { get; set; }
    }
}