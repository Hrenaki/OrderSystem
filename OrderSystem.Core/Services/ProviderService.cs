using OrderSystem.Data;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
   public class ProviderService : IProviderService
   {
      private ApplicationDbContext dbContext;

      public ProviderService(ApplicationDbContext dbContext)
      {
         ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

         this.dbContext = dbContext;
      }

      public List<ProviderEntity> GetProviders()
      {
         return dbContext.Providers.ToList();
      }
   }
}