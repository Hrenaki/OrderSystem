using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.API.Models.Orders;
using OrderSystem.Core.Services;

namespace OrderSystem.API.Controllers
{
    [ApiController]
    [Route("/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
            this.orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<OrdersResponse> GetOrders()
        {
            var orders = orderService.GetOrders()
                                     .Select(order => new OrderModel()
                                     {
                                        Number = order.Number,
                                        Date = order.Date,
                                        ProviderName = order.ProviderEntity.Name
                                     })
                                     .ToArray();
            return Ok(orders);
        }
    }
}