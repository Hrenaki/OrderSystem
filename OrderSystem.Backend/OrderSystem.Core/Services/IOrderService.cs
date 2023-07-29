using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
    public interface IOrderService
    {
        public OrderEntity? GetOrderById(int id);
        public Result CreateOrder(OrderEntity order);
        public Result UpdateOrder(OrderEntity order);
        public IQueryable<OrderEntity> GetOrders();
        public List<OrderEntity> GetOrders(OrderFilteringObject filteringObject);
    }
}