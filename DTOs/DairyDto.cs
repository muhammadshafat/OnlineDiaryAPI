namespace OnlineDiaryAPI.DTOs
{
    public class DiaryDto
    {
        public int Did { get; set; }
        public DateTime Ddate { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}