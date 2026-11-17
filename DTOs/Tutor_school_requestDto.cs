namespace OnlineDiaryAPI.DTOs
{
    public class Tutor_school_requestDto
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int SchoolId { get; set; }

        public string? School { get; set; }
        public string Status { get; set; } = "pending";

        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? ClassSection { get; set; }

        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }

        public decimal FeePerMonth { get; set; }

        public string? TutorName { get; set; }

        // Used when creating multiple course requests
        public List<TutorCourseDto> Courses { get; set; } = new();
    }

    public class TutorCourseDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public decimal FeePerMonth { get; set; }
    }
}