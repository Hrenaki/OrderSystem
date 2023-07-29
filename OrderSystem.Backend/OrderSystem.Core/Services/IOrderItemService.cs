using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
	public interface IOrderItemService
	{
		public Result CreateUpdateOrderItem(OrderItemEntity orderItem);
		public Result DeleteOrderItem(int id);
		public List<OrderItemEntity> GetOrderItems();
	}
}