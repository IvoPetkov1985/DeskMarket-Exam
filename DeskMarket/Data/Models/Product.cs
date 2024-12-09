using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Data.Models
{
    [Comment("The product at the market with its properties")]
    public class Product
    {
        [Key]
        [Comment("Product identifier")]
        public int Id { get; set; }

        [Required]
        [MaxLength(ProductNameMaxLength)]
        [Comment("Product name")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [Comment("Some details about the product")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Comment("Product price")]
        public decimal Price { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        [Comment("Product image (optional)")]
        public string? ImageUrl { get; set; }

        [Required]
        [Comment("Seller (user) identifier")]
        public string SellerId { get; set; } = string.Empty;

        [ForeignKey(nameof(SellerId))]
        public IdentityUser Seller { get; set; } = null!;

        [Required]
        public DateTime AddedOn { get; set; }

        [Required]
        [Comment("Category identifier")]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        [Comment("Is this product available")]
        public bool IsDeleted { get; set; } = false;

        public IEnumerable<ProductClient> ProductsClients { get; set; } = [];
    }
}
