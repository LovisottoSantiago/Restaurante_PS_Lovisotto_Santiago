using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }        
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(category => category.Id);
                
                entity.Property(category => category.Name)
                    .IsRequired()
                    .HasMaxLength(25);
                
                entity.Property(category => category.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(category => category.Order)
                    .IsRequired()
                    .HasColumnName("Order");
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.ToTable("DeliveryType");
                entity.HasKey(deliveryType => deliveryType.Id);
                
                entity.Property(deliveryType => deliveryType.Name)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("Dish");
                entity.HasKey(dish => dish.DishId);

                entity.Property(dish => dish.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(dish => dish.Name)
                    .IsUnique();

                entity.Property(dish => dish.Description)
                    .IsRequired()
                    .HasColumnType("varchar(MAX)");

                entity.Property(dish => dish.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(dish => dish.Available)
                    .IsRequired();

                entity.Property(dish => dish.ImageUrl)
                    .HasColumnType("varchar(MAX)");

                entity.Property(dish => dish.CreateDate)
                    .HasDefaultValueSql("GETDATE()");
                
                entity.Property(dish => dish.UpdateDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(dish => dish.Category)
                    .WithMany(category => category.Dishes)
                    .HasForeignKey(dish => dish.CategoryId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(order => order.OrderId);

                entity.Property(order => order.DeliveryTo)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(order => order.Notes)
                    .HasColumnType("varchar(MAX)");

                entity.Property(order => order.Price)
                    .IsRequired()    
                    .HasColumnType("decimal(18,2)");

                entity.Property(order => order.CreateDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(order => order.UpdateDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(order => order.DeliveryType)
                    .WithMany(deliveryType => deliveryType.Orders)
                    .HasForeignKey(order => order.DeliveryTypeId);

                entity.HasOne(order => order.OverallStatus)
                    .WithMany(overallStatus => overallStatus.Orders)
                    .HasForeignKey(order => order.OverallStatusId);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItem");                
                entity.HasKey(orderItem => orderItem.OrderItemId);

                entity.Property(orderItem => orderItem.Quantity)
                    .IsRequired();
                
                entity.Property(orderItem => orderItem.Notes)
                    .HasColumnType("varchar(MAX)");

                entity.Property(orderItem => orderItem.CreateDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(orderItem => orderItem.Dish)
                    .WithMany(dish => dish.OrderItems)
                    .HasForeignKey(orderItem => orderItem.DishId);

                entity.HasOne(orderItem => orderItem.Order)
                    .WithMany(order => order.OrderItems)
                    .HasForeignKey(orderItem => orderItem.OrderId);

                entity.HasOne(orderItem => orderItem.Status)
                    .WithMany(status => status.OrderItems)
                    .HasForeignKey(orderItem => orderItem.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");
                entity.HasKey(status => status.Id);

                entity.Property(status => status.Name)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            // Precarga de datos (consigna)
            modelBuilder.Entity<DeliveryType>().HasData(
                new DeliveryType { Id = 1, Name = "Delivery" },
                new DeliveryType { Id = 2, Name = "Take away" },
                new DeliveryType { Id = 3, Name = "Dine in" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "In progress" },
                new Status { Id = 3, Name = "Ready" },
                new Status { Id = 4, Name = "Delivery" },
                new Status { Id = 5, Name = "Closed" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Entradas", Description = "Pequeñas porciones para abrir el apetito antes del plato principal.", Order = 1 },
                new Category { Id = 2, Name = "Ensaladas", Description = "Opciones frescas y livianas, ideales como acompañamiento o plato principal.", Order = 2 },
                new Category { Id = 3, Name = "Minutas", Description = "Platos rápidos y clásicos de bodegón: milanesas, tortillas, revueltos.", Order = 3 },
                new Category { Id = 4, Name = "Pastas", Description = "Variedad de pastas caseras y salsas tradicionales.", Order = 5 },
                new Category { Id = 5, Name = "Parrilla", Description = "Cortes de carne asados a la parrilla, servidos con guarniciones.", Order = 4 },
                new Category { Id = 6, Name = "Pizzas", Description = "Pizzas artesanales con masa casera y variedad de ingredientes.", Order = 7 },
                new Category { Id = 7, Name = "Sandwiches", Description = "Sandwiches y lomitos completos preparados al momento.", Order = 6 },
                new Category { Id = 8, Name = "Bebidas", Description = "Gaseosas, jugos, aguas y opciones sin alcohol.", Order = 8 },
                new Category { Id = 9, Name = "Cerveza Artesanal", Description = "Cervezas de producción artesanal, rubias, rojas y negras.", Order = 9 },
                new Category { Id = 10, Name = "Postres", Description = "Clásicos dulces caseros para cerrar la comida.", Order = 10 }
            );
        }


    }
}
