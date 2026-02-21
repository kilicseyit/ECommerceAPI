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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.Sender)
                      .WithMany()
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(m => m.Receiver)
                      .WithMany()
                      .HasForeignKey(m => m.ReceiverId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
            // Product konfigürasyonu
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
                entity.Property(p => p.DiscountPrice)
                      .HasColumnType("decimal(18,2)");
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
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
                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}