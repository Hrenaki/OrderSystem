using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services.Filters
{
	internal class OrderItemNameFilter : IOrderFilter
	{
		private readonly string[] orderItemNames;

		public OrderItemNameFilter(IEnumerable<string> orderItemNames)
		{
         ArgumentNullException.ThrowIfNull(orderItemNames, nameof(orderItemNames));

			this.orderItemNames = orderItemNames.ToArray();
      }

		public IQueryable<OrderEntity> Filter(IQueryable<OrderEntity> orders)
		{
			return orders.Where(order => order.OrderItemEntities.Any(orderItem => orderItemNames.Contains(orderItem.Name)));
		}
	}
}