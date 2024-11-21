namespace DeskMarket.Models
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string AddedOn { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public string Seller { get; set; } = string.Empty;

        public bool HasBought { get; set; }
    }
}
