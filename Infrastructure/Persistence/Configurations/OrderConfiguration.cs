using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(order => order.OrderId);

            builder.Property(order => order.DeliveryTo)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(order => order.Notes)
                .IsRequired()
                .HasColumnType("varchar(MAX)");

            builder.Property(order => order.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(order => order.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(order => order.UpdateDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(order => order.DeliveryTypeNavigation)
                .WithMany(deliveryType => deliveryType.Orders)
                .HasForeignKey(order => order.DeliveryType)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(order => order.OverallStatusNavigation)
                .WithMany(overallStatus => overallStatus.Orders)
                .HasForeignKey(order => order.OverallStatus)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
