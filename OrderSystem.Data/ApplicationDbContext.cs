using Microsoft.EntityFrameworkCore;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data
{
   public class ApplicationDbContext : DbContext
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
      {
         Database.EnsureCreated();
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         //builder.Entity<OrderEntity>().HasAlternateKey(oe => new { oe.Number, oe.ProviderEntityId });
         base.OnModelCreating(builder);
      }
   }
}