using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services.Filters
{
   internal class OrderFunctionFilter : IOrderFilter
   {
      private Func<IQueryable<OrderEntity>, IQueryable<OrderEntity>> func;

      public OrderFunctionFilter(Func<IQueryable<OrderEntity>, IQueryable<OrderEntity>> func)
      {
         ArgumentNullException.ThrowIfNull(func, nameof(func));

         this.func = func;
      }

      public IQueryable<OrderEntity> Filter(IQueryable<OrderEntity> orders) => func(orders);
   }
}