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
        private readonly IUserService userService;

        public OrdersController(IOrderService orderService, IUserService userService)
        {
            this.orderService = orderService;
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<OrdersResponse> GetOrders()
        {
            var username = HttpContext.User.Claims.First(claim => claim.Type == JwtTokenClaimTypes.Username).Value;

            var currentUser = userService.GetUserByUsername(username);
            if (currentUser is null)
                return Unauthorized();

            var orders = orderService.GetOrders().Where(order => order.UserId == currentUser.Id)
                                     .Select(order => new OrderModel()
                                     {
                                        Number = order.Number,
                                        Date = order.Date,
                                        ProviderName = order.ProviderEntity.Name
                                     })
                                     .ToArray();
            return Ok(orders);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public ActionResult<OrderResponse> GetUserOrders()
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