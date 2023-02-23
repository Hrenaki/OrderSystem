using Microsoft.EntityFrameworkCore;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<ProviderEntity> Providers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProviderEntity>().HasData(
               new ProviderEntity() { Id = 1, Name = "Yandex" },
               new ProviderEntity() { Id = 2, Name = "Google" },
               new ProviderEntity() { Id = 3, Name = "Intel" }
            );
        }
    }
}