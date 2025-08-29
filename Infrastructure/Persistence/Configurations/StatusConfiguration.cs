using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Status");
            builder.HasKey(status => status.Id);

            builder.Property(status => status.Name)
                .IsRequired()
                .HasMaxLength(25);

            // Precarga de datos
            builder.HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "In progress" },
                new Status { Id = 3, Name = "Ready" },
                new Status { Id = 4, Name = "Delivery" },
                new Status { Id = 5, Name = "Closed" }
            );
        }

    }
}
