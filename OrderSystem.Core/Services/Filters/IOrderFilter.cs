using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services.Filters
{
   public interface IOrderFilter
   {
      IQueryable<OrderEntity> Filter(IQueryable<OrderEntity> orders);
   }
}