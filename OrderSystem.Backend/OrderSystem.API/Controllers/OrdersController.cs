using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrdersResponse>> GetOrders([FromBody] OrdersRequest request)
        {
            var username = HttpContext.User.Claims.First(claim => claim.Type == JwtTokenClaimTypes.Username).Value;

            var currentUser = userService.GetUserByUsername(username);
            if (currentUser is null)
                return Unauthorized();

            var orders = orderService.GetOrders().Where(order => order.UserId == currentUser.Id);

            if (request.DateFrom.HasValue)
                orders = orders.Where(order => order.Date >= request.DateFrom);

            if(request.DateTo.HasValue)
                orders = orders.Where(order => order.Date <= request.DateTo);

            if (request.ProviderIds is not null && request.ProviderIds.Any())
                orders = orders.Where(order => request.ProviderIds.Contains(order.ProviderEntityId));

            var response = (await orders
                .Include(order => order.ProviderEntity)
                .Select(order => new { order.Id, order.Number, order.Date, ProviderName = order.ProviderEntity.Name })
                .ToArrayAsync())
                .Select(order => new OrderModel
                {
                    Id = order.Id,
                    Number = order.Number,
                    Date = order.Date,
                    ProviderName = order.ProviderName
                })
                .ToArray();

            return Ok(response);
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