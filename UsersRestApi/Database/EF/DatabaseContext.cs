using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
using System.Collections.Generic;
using UsersRestApi.Database.Entities;

namespace UsersRestApi.Database.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }
        public DbSet<BaseUserEntity> Users { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<BuyerEntity> Buyers { get; set; }
        public DbSet<ImageEntity> Images { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<FavoritesEntity> Favorites { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderProductEntity> OrderProducts { get; set; }

        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductEntity>().HasKey(k => k.ProductId);
            modelBuilder.Entity<ProductEntity>().Property(p => p.ProductId).ValueGeneratedOnAdd();

            modelBuilder.Entity<CategoryEntity>().HasKey(k => k.CategoryId);
            modelBuilder.Entity<CategoryEntity>().Property(p => p.CategoryId).ValueGeneratedOnAdd();

            modelBuilder.Entity<SubCategoryEntity>().HasKey(k => k.SubCategoryId);
            modelBuilder.Entity<SubCategoryEntity>().Property(p => p.SubCategoryId).ValueGeneratedOnAdd();

            modelBuilder.Entity<BaseUserEntity>().HasKey(k => k.Id);
            modelBuilder.Entity<BaseUserEntity>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ImageEntity>().HasKey(k => k.ImageId);
            modelBuilder.Entity<ImageEntity>().Property(p => p.ImageId).ValueGeneratedOnAdd();

            modelBuilder.Entity<CartEntity>().HasKey(k => k.CartId);
            modelBuilder.Entity<CartEntity>().Property(p => p.CartId).ValueGeneratedOnAdd();

            modelBuilder.Entity<FavoritesEntity>().HasKey(k => k.FavouriteId);
            modelBuilder.Entity<FavoritesEntity>().Property(p => p.FavouriteId).ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderEntity>().HasKey(k => k.OrderId);
            modelBuilder.Entity<OrderEntity>().Property(p => p.OrderId).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderEntity>().Property(p => p.OrderStatus).HasDefaultValue("New");

            modelBuilder.Entity<OrderProductEntity>().HasKey(k => k.Id);
            modelBuilder.Entity<OrderProductEntity>().Property(p => p.Id).ValueGeneratedOnAdd();

        }
    }
}
