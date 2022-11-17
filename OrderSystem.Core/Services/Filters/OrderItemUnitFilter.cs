using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services.Filters
{
	internal class OrderItemUnitFilter : IOrderFilter
	{
		private readonly string[] orderItemUnits;

		public OrderItemUnitFilter(IEnumerable<string> units)
		{
			ArgumentNullException.ThrowIfNull(units, nameof(units));

			orderItemUnits = units.ToArray();
		}

		public IQueryable<OrderEntity> Filter(IQueryable<OrderEntity> orders)
		{
			return orders.Where(order =>
				order.OrderItemEntities.Any(orderItem => orderItemUnits.Contains(orderItem.Unit)));
		}
	}
}