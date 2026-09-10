namespace OnlineDiaryAPI.DTOs
{
    public class FlowerDto

    {
        public int FlowerId { get; set; }

        public string? FlowerName { get; set; }

        public string? Color { get; set; }

        public decimal Price { get; set; }

        public int GardenId { get; set; }
    }
}