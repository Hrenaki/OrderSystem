﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderSystem.API.Models.Orders;
using OrderSystem.Core.Services;
using OrderSystem.Data.Entities;

namespace OrderSystem.API.Controllers
{
    [ApiController]
    [Route("/order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IProviderService providerService;
        private readonly IUserService userService;

        public OrderController(IOrderService orderService, IProviderService providerService, IUserService userService)
        {
            this.orderService = orderService;
            this.providerService = providerService;
            this.userService = userService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<OrderResponse> GetOrder(int id)
        {
            var order = orderService.GetOrderById(id);
            if (order is null)
                return NotFound($"Can\'t find order with id: {id}");

            var orderItems = order.OrderItemEntities.Select(item => new OrderItemModel
            {
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit
            }).ToArray();

            var detailedOrder = new OrderDetailedModel
            {
                Id = order.Id,
                Number = order.Number,
                Date = order.Date,
                ProviderId = order.ProviderEntityId,
                ProviderName = order.ProviderEntity.Name,
                Items = orderItems,
            };

            var orderResponse = new OrderResponse
            {
                Order = detailedOrder
            };
            return Ok(orderResponse);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderModel order)
        {
            var username = HttpContext.User.Claims.First(claim => claim.Type == JwtTokenClaimTypes.Username).Value;

            var currentUser = userService.GetUserByUsername(username);
            if (currentUser is null)
                return Unauthorized();

            if (orderService.GetOrders().Any(o => o.Number == order.Number && o.ProviderEntity.Name == order.Number))
                return BadRequest("Order already exists");

            var provider = await providerService.GetProviders().FirstOrDefaultAsync(provider => provider.Id == order.ProviderId);
            if (provider is null)
                return NotFound($"Provider doesn't exist");

            var orderEntity = new OrderEntity
            {
                Number = order.Number,
                Date = order.Date,
                ProviderEntityId = provider.Id,
                UserId = currentUser.Id
            };

            var result = orderService.CreateOrder(orderEntity);
            return result.Success ? NoContent() : BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            var username = HttpContext.User.Claims.First(claim => claim.Type == JwtTokenClaimTypes.Username).Value;

            var currentUser = userService.GetUserByUsername(username);
            if (currentUser is null)
                return Unauthorized();

            var order = orderService.GetOrderById(request.Id);
            if (order is null)
                return BadRequest("Order doesn't exist");

            if (order.UserId != currentUser.Id && currentUser.Role != UserRole.Admin)
                return Forbid("Access denied");

            var result = orderService.UpdateOrder(new OrderEntity
            {
                Id = request.Id,
                Number = request.Number,
                Date = request.Date,
                ProviderEntityId = request.ProviderId,
                OrderItemEntities = request.Items.Select(i => new OrderItemEntity
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity,
                    Unit = i.Unit,
                    OrderEntityId = order.Id
                }).ToList()
            });

            return result.Success ? NoContent() : BadRequest(result.Message);
        }
    }
}