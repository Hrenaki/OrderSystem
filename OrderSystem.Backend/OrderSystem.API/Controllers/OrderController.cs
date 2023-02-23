﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.API.Models.Orders;
using OrderSystem.Core.Services;
using OrderSystem.Data.Entities;

namespace OrderSystem.API.Controllers
{
    [ApiController]
    [Route("/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IProviderService providerService;

        public OrderController(IOrderService orderService, IProviderService providerService)
        {
            ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
            ArgumentNullException.ThrowIfNull(providerService, nameof(providerService));

            this.orderService = orderService;
            this.providerService = providerService;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<OrderResponse> GetOrder(int id)
        {
            var order = orderService.GetOrderById(id);
            if (order is null)
                return NotFound($"Can\'t find order with id: {id}");

            var orderItems = order.OrderItemEntities.Select(item => new OrderItemModel()
            {
                Name = item.Name,
                Quantity = item.Quantity,
                Unit = item.Unit
            }).ToArray();

            var detailedOrder = new OrderDetailedModel()
            {
                Number = order.Number,
                Date = order.Date,
                ProviderName = order.ProviderEntity.Name,
                Items = orderItems,
            };

            var orderResponse = new OrderResponse()
            {
                Order = detailedOrder
            };
            return Ok(orderResponse);
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] CreateOrderModel order)
        {
            if (orderService.GetOrders().Any(o => o.Number == order.Number && o.ProviderEntity.Name == order.Number))
                return BadRequest("Order already exists");

            if (!providerService.GetProviders().Any(provider => provider.Name == order.ProviderName))
                return NotFound($"Provider '{order.ProviderName}' doesn't exist");

            var provider = providerService.GetProviders().First(provider => provider.Name == order.ProviderName);
            var orderEntity = new OrderEntity()
            {
                Number = order.Number,
                Date = order.Date,
                ProviderEntityId = provider.Id,
                OrderItemEntities = order?.Items?.Select(item => new OrderItemEntity()
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Unit = item.Unit
                }).ToList()!
            };

            var result = orderService.CreateOrder(orderEntity);
            return result.Success ? NoContent() : BadRequest(result.Message);
        }
    }
}