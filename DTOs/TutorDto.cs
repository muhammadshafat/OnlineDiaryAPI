namespace OnlineDiaryAPI.DTOs
{
    public class Tutor
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        // Array to store multiple class IDs
        public int[] cid { get; set; } = Array.Empty<int>();
    }
}