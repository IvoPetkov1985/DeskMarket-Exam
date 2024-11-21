namespace DeskMarket.Models
{
    public class ProductIndexViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public bool IsSeller { get; set; }

        public bool HasBought { get; set; }
    }
}
