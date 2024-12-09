using System.ComponentModel.DataAnnotations;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Models
{
    public class ProductFormModel
    {
        [Required(ErrorMessage = ProductNameRequiredErrorMessage)]
        [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength, ErrorMessage = ProductNameStringLengthMessage)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue, ConvertValueInInvariantCulture = true)]
        public decimal Price { get; set; }

        [StringLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = AddedOnRequiredErrorMessage)]
        [RegularExpression(DateRegex, ErrorMessage = DateFormatErrorMessage)]
        public string AddedOn { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = [];
    }
}
