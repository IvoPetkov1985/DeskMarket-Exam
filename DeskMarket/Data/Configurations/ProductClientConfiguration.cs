using DeskMarket.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeskMarket.Data.Configurations
{
    public class ProductClientConfiguration : IEntityTypeConfiguration<ProductClient>
    {
        public void Configure(EntityTypeBuilder<ProductClient> builder)
        {
            builder.HasKey(pc => new
            {
                pc.ProductId,
                pc.ClientId
            });

            builder.HasOne(pc => pc.Client)
                .WithMany()
                .HasForeignKey(pc => pc.ClientId);

            builder.HasOne(pc => pc.Product)
                .WithMany(pc => pc.ProductsClients)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
