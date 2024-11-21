using System.ComponentModel.DataAnnotations;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Models
{
    public class ProductFormModel
    {
        [Required]
        [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), ProductMinPrice, ProductMaxPrice, ConvertValueInInvariantCulture = true)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(ProductDescriptionMaxLength, MinimumLength = ProductDescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [RegularExpression(DateTimeRegex, ErrorMessage = DateTimeErrorMsg)]
        public string AddedOn { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
