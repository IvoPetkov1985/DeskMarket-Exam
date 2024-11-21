using DeskMarket.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeskMarket.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category { Id = 1, Name = "Laptops" },
                new Category { Id = 2, Name = "Workstations" },
                new Category { Id = 3, Name = "Accessories" },
                new Category { Id = 4, Name = "Desktops" },
                new Category { Id = 5, Name = "Monitors" });
        }
    }
}
