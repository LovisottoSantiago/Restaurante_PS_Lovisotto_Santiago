using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DeliveryTypeConfiguration : IEntityTypeConfiguration<DeliveryType>
    {
        public void Configure(EntityTypeBuilder<DeliveryType> builder)
        {
            builder.ToTable("DeliveryType");
            builder.HasKey(deliveryType => deliveryType.Id);

            builder.Property(deliveryType => deliveryType.Name)
                .IsRequired()
                .HasMaxLength(25);

            // Precarga de datos 
            builder.HasData(
                new DeliveryType { Id = 1, Name = "Delivery" },
                new DeliveryType { Id = 2, Name = "Take away" },
                new DeliveryType { Id = 3, Name = "Dine in" }
            );
        }

    }
}
