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
                .HasColumnType("varchar(MAX)");

            builder.Property(orderItem => orderItem.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(orderItem => orderItem.Dish)
                .WithMany(dish => dish.OrderItems)
                .HasForeignKey(orderItem => orderItem.DishId);

            builder.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId);

            builder.HasOne(orderItem => orderItem.Status)
                .WithMany(status => status.OrderItems)
                .HasForeignKey(orderItem => orderItem.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
