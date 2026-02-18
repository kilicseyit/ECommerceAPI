using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product konfigürasyonu
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            // OrderItem konfigürasyonu
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(o => o.UnitPrice)
                      .HasColumnType("decimal(18,2)");
                entity.Ignore(o => o.TotalPrice);
            });

            // Order konfigürasyonu
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.TotalPrice)
                      .HasColumnType("decimal(18,2)");
            });
        }
    }
}