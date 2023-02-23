using OrderSystem.Core.Services.Filters;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
   public class OrderFilteringObjectBuilder
   {
      private readonly List<IOrderFilter> filters = new List<IOrderFilter>();

      public OrderFilteringObjectBuilder AddDateFilter(DateTime dateFrom, DateTime dateTo) =>
         AddFilterAndReturnSelf(OrderFilterFactory.CreateDateFilter(dateFrom, dateTo));

      public OrderFilteringObjectBuilder AddNumberFilter(params string[] numbers) =>
         AddFilterAndReturnSelf(OrderFilterFactory.CreateNumberFilter(numbers));

      public OrderFilteringObjectBuilder AddProviderNameFilter(params string[] providerNames) =>
         AddFilterAndReturnSelf(OrderFilterFactory.CreateProviderNameFilter(providerNames));

      public OrderFilteringObjectBuilder AddProviderIdFilter(params int[] ids) =>
         AddFilterAndReturnSelf(OrderFilterFactory.CreateProviderIdFilter(ids));

      public OrderFilteringObjectBuilder AddFunctionFilter(Func<IQueryable<OrderEntity>, IQueryable<OrderEntity>> func) =>
         AddFilterAndReturnSelf(OrderFilterFactory.CreateFunctionFilter(func));

      public OrderFilteringObjectBuilder AddCustomFilter(IOrderFilter filter) =>
         AddFilterAndReturnSelf(filter);

      public OrderFilteringObject Build() => new OrderFilteringObject(filters);

      private OrderFilteringObjectBuilder AddFilterAndReturnSelf(IOrderFilter filter)
      {
         filters.Add(filter);
         return this;
      }
   }

   public class OrderFilteringObject
   {
      private readonly List<IOrderFilter> filters = new List<IOrderFilter>();

      public OrderFilteringObject(IEnumerable<IOrderFilter> filters)
      {
         this.filters.AddRange(filters);
      }

      public IQueryable<OrderEntity> Apply(IQueryable<OrderEntity> orders)
      {
         foreach (var filter in filters)
            orders = filter.Filter(orders);
         return orders;
      }
   }
}