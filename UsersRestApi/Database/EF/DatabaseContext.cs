using Microsoft.EntityFrameworkCore;
using UsersRestApi.Database.Entities;

namespace UsersRestApi.Database.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SubCategoryEntity> SubCategories { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ImageEntity> ImageEntities { get; set; }
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

            modelBuilder.Entity<UserEntity>().HasKey(k => k.Id);
            modelBuilder.Entity<UserEntity>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<ImageEntity>().HasKey(k => k.ImageId);
            modelBuilder.Entity<ImageEntity>().Property(p => p.ImageId).ValueGeneratedOnAdd();
        }
    }
}
