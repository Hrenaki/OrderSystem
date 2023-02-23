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

		public Result CreateUpdateOrderItem(OrderItemEntity orderItem)
		{
			var result = new Result() { Success = false };

         var item = dbContext.OrderItems.Include(item => item.OrderEntity)
                                        .FirstOrDefault(item => item.Id == orderItem.Id);
			if(item != null)
			{
				return UpdateOrderItem(item, orderItem);
			}

         if (dbContext.OrderItems.Any(item => item.Name == orderItem.Name &&
															item.OrderEntityId == orderItem.OrderEntityId))
			{
				result.Message = $"Can't add order item which name equals to order number {orderItem.Name}";
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

		public Result DeleteOrderItem(int id)
		{
			var result = new Result() { Success = false };

			var orderItem = dbContext.OrderItems.FirstOrDefault(item => item.Id == id);
			if (orderItem == null)
			{
				result.Message = $"Order item with id: {id} doesn't exists";
				return result;
			}

			try
			{
				dbContext.OrderItems.Remove(orderItem);
				dbContext.SaveChanges();

				result.Success = true;
			}
			catch(Exception e)
			{
				result.Message = e.Message;
			}

			return result;
		}

		public List<OrderItemEntity> GetOrderItems()
		{
			return dbContext.OrderItems.ToList();
		}

		private Result UpdateOrderItem(OrderItemEntity oldItem, OrderItemEntity newItem)
		{
         var result = new Result() { Success = false };

			if(newItem.Name == oldItem.OrderEntity.Number)
			{
				result.Message = $"Can't set order item name equal to order number: '{newItem.Name}'";
				return result;
			}

         oldItem.Name = newItem.Name;
         oldItem.OrderEntityId = newItem.OrderEntityId;
         oldItem.Quantity = newItem.Quantity;
         oldItem.Unit = newItem.Unit;

			try
			{
				dbContext.OrderItems.Update(oldItem);
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