using Microsoft.EntityFrameworkCore;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data
{
   public class ApplicationDbContext : DbContext
   {
      public DbSet<OrderEntity> Orders { get; set; }
      public DbSet<OrderItemEntity> OrderItems { get; set; }
      public DbSet<ProviderEntity> Providers { get; set; }

      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {
         Database.EnsureCreated();
      }
   }
}