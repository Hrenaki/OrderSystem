using Microsoft.EntityFrameworkCore;
using OrderSystem.Data;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
	public class OrderItemService : IOrderItemService
	{
		private readonly ApplicationDbContext dbContext;

		public OrderItemService(ApplicationDbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

			this.dbContext = dbContext;
		}

		public Result AddOrderItem(OrderItemEntity orderItem)
		{
			var result = new Result() { Success = false };

			if(dbContext.OrderItems.Any(item => item.Name == orderItem.Name &&
															item.OrderEntityId == orderItem.OrderEntityId))
			{
				result.Message = "Order item with such (name, orderId) already exists";
            return result;
			}

			try
			{
				dbContext.OrderItems.Add(orderItem);
				dbContext.SaveChanges();

				result.Success = true;
			}
			catch(Exception e)
			{
				result.Message = e.Message;
			}

			return result;
		}

		public Result UpdateOrderItem(OrderItemEntity orderItem)
		{
         var result = new Result() { Success = false };

			var item = dbContext.OrderItems.Include(item => item.OrderEntity)
													 .FirstOrDefault(item => item.Id == orderItem.Id);

			if(item == null)
			{
				result.Message = $"Can't find order item with id = {orderItem.Id}";
				return result;
			}

			if(orderItem.Name == item.OrderEntity.Number)
			{
				result.Message = $"Can't set order item name equal to order number: '{orderItem.Name}'";
				return result;
			}

			item.Name = orderItem.Name;
			item.OrderEntityId = orderItem.OrderEntityId;
			item.Quantity = orderItem.Quantity;
			item.Unit = orderItem.Unit;

			try
			{
				dbContext.OrderItems.Update(item);
				dbContext.SaveChanges();

				result.Success = true;
			}
			catch(Exception e)
			{
				result.Message = e.Message;
			}

			return result;
      }
	}
}