using System.ComponentModel.DataAnnotations;
using static DeskMarket.Data.Common.DataConstants;

namespace DeskMarket.Data.DataModels
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
