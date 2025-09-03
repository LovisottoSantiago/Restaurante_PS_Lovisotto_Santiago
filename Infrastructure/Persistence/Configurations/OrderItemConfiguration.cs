using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItem");
            builder.HasKey(orderItem => orderItem.OrderItemId);

            builder.Property(orderItem => orderItem.Quantity)
                .IsRequired();

            builder.Property(orderItem => orderItem.Notes)
                .IsRequired()
                .HasColumnType("varchar(MAX)");

            builder.Property(orderItem => orderItem.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(orderItem => orderItem.DishNavigation)
                .WithMany(dish => dish.OrderItems)
                .HasForeignKey(orderItem => orderItem.Dish)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(orderItem => orderItem.OrderNavigation)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.Order)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(orderItem => orderItem.StatusNavigation)
                .WithMany(status => status.OrderItems)
                .HasForeignKey(orderItem => orderItem.Status) 
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
