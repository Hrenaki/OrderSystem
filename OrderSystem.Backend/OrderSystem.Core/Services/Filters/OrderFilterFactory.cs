using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services.Filters
{
   public static class OrderFilterFactory
   {
      public static IOrderFilter CreateDateFilter(DateTime dateFrom, DateTime dateTo)
      {
         return new OrderDateFilter(dateFrom, dateTo);
      }

      public static IOrderFilter CreateNumberFilter(params string[] numbers)
      {
         return new OrderNumberFilter(numbers);
      }

      public static IOrderFilter CreateProviderNameFilter(params string[] names)
      {
         return new OrderProviderNameFilter(names);
      }

      public static IOrderFilter CreateProviderIdFilter(params int[] ids)
      {
         return new OrderProviderIdFilter(ids);
      }

      public static IOrderFilter CreateFunctionFilter(Func<IQueryable<OrderEntity>, IQueryable<OrderEntity>> func)
      {
         return new OrderFunctionFilter(func);
      }

      public static IOrderFilter CreateOrderItemNameFilter(IEnumerable<string> orderItemNames)
      {
         return new OrderItemNameFilter(orderItemNames);
      }

      public static IOrderFilter CreateOrderItemUnitFilter(IEnumerable<string> orderItemUnits)
      {
         return new OrderItemUnitFilter(orderItemUnits);
      }
   }
}