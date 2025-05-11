using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Customer
            modelBuilder.Entity<Customer>()
                .Property(c => c.Password)
                .IsRequired();

            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .IsRequired();

            // Configure Orders relationships
            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Orders>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Product-ProductImages relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the many-to-many relationship between Customer and Product through Orders
            modelBuilder.Entity<Orders>()
                .HasKey(o => new { o.Id }); // Or use a composite key if preferred: o.CustomerId, o.ProductId

            // Seeding Data
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = 123456789,
                    Email = "john@example.com",
                    Password = PasswordHelper.HashPassword("123456") // Hash the password
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    PhoneNumber = 987654321,
                    Email = "jane@example.com",
                    Password = PasswordHelper.HashPassword("abcdef") // Hash the password
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "A powerful laptop",
                    Price = 1500
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "A latest-gen smartphone",
                    Price = 800
                }
            );

            modelBuilder.Entity<Orders>().HasData(
                new Orders
                {
                    Id = 1,
                    ProductId = 1,
                    CustomerId = 1
                },
                new Orders
                {
                    Id = 2,
                    ProductId = 2,
                    CustomerId = 2
                }
            );

            // Seed ProductImages with sample image data
            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage
                {
                    Id = 1,
                    ProductId = 1,
                    ImageData = new byte[] { 0x20, 0x20, 0x20 }, // Simple placeholder bytes
                    IsPrimary = true,
                    UploadDate = DateTime.UtcNow
                },
                new ProductImage
                {
                    Id = 2,
                    ProductId = 1,
                     ImageData = new byte[] { 0x20, 0x20, 0x20 }, // Simple placeholder bytes
                    IsPrimary = false,
                    UploadDate = DateTime.UtcNow
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }

            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }
    }
}