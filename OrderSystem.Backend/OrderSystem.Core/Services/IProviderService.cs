using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
   public interface IProviderService
   {
      public IQueryable<ProviderEntity> GetProviders();
      public Result Create(ProviderEntity provider);
   }
}