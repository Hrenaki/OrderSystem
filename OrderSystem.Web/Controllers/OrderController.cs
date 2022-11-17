using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystem.Core.Services;
using OrderSystem.Core.Services.Filters;
using OrderSystem.Data.Entities;
using OrderSystem.Web.Models;
using System.Xml;

namespace OrderSystem.Web.Controllers
{
   [Route("orders")]
   public class OrderController : Controller
   {
      private readonly IOrderService orderService;
      private readonly IProviderService providerService;
      private readonly IOrderItemService orderItemService;

      private const string newOrderModalViewName = "NewOrderModal";
      private const string IndexViewName = "Index";
      private const string EditOrderViewName = "EditOrder";
      private const string EditOrderItemViewName = "EditOrderItemModal";

      public OrderController(IOrderService orderService,
                             IProviderService providerService,
                             IOrderItemService orderItemService)
      {
         ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
         ArgumentNullException.ThrowIfNull(providerService, nameof(providerService));
         ArgumentNullException.ThrowIfNull(orderItemService, nameof(orderItemService));

         this.orderService = orderService;
         this.providerService = providerService;
         this.orderItemService = orderItemService;
      }

      [HttpGet("get/{id:int}")]
      public IActionResult GetOrderById(int id)
      {
         var order = orderService.GetOrderById(id);
         if (order == null)
            return BadRequest();

         var editOrderViewModel = new EditOrderViewModel()
         {
            Id = order.Id,
            Number = order.Number,
            Date = order.Date,
            ProviderId = order.ProviderEntityId,
            ProviderNames = GetProviderNameSelectList(providerService)
         };

         var orderItemList = order.OrderItemEntities.Select(orderItem => new OrderItemViewModel()
         {
            Id = orderItem.Id,
            OrderId = orderItem.OrderEntityId,
            Name = orderItem.Name,
            Quantity = orderItem.Quantity,
            Unit = orderItem.Unit
         }).ToList();

         var viewModel = new EditOrderWithItemsViewModel()
         {
            OrderViewModel = editOrderViewModel,
            OrderItemsViewModel = new EditOrderItemsViewModel()
            {
               OrderId = order.Id,
               OrderItemViewModels = orderItemList
            }
         };

         return PartialView("EditOrderWithItems", viewModel);
      }

      [HttpPost("update")]
      public IActionResult UpdateOrder([FromForm] EditOrderViewModel model)
      {
         model.ProviderNames = GetProviderNameSelectList(providerService);

         if (!ModelState.IsValid)
            return PartialView(EditOrderViewName, model);

         var result = orderService.UpdateOrder(new OrderEntity()
         {
            Id = model.Id,
            Number = model.Number,
            Date = model.Date,
            ProviderEntityId = model.ProviderId
         });

         if (!result.Success)
            ModelState.AddModelError("", result.Message);

         return PartialView(EditOrderViewName, model);
      }

      [HttpGet("get")]
      public IActionResult GetOrders()
      {
         var orders = orderService.GetOrders();
         return PartialView(IndexViewName, new OrdersViewModel(orders));
      }

      [HttpPost("get")]
      public IActionResult GetFilteredOrders([FromForm] OrderFilterValuesViewModel filteringValues)
      {
         if (filteringValues == null)
            return BadRequest();

         var filteringObject = CreateOrderFilteringObject(filteringValues);
         var orders = orderService.GetOrders(filteringObject);

         return PartialView(IndexViewName, new OrdersViewModel(orders));
      }

      [HttpPost("new")]
      public IActionResult AddNewOrder([FromForm] NewOrderViewModel model)
      {
         model.ProviderNames = GetProviderNameSelectList(providerService);

         if (!ModelState.IsValid)
            return PartialView(newOrderModalViewName, model);

         if (IsOrderWithNumberAndProviderIdExists(model.Number, model.ProviderId))
         {
            ModelState.AddModelError("", $"Order with number: '{model.Number}' and providerId: '{model.ProviderId}' already exists");
            return PartialView(newOrderModalViewName, model);
         }

         var result = orderService.CreateOrder(new OrderEntity()
         {
            Number = model.Number,
            Date = model.Date,
            ProviderEntityId = model.ProviderId
         });

         if (!result.Success)
            ModelState.AddModelError("", result.Message);

         return PartialView(newOrderModalViewName, model);
      }

      [HttpPost("items/update")]
      public IActionResult CreateUpdateOrderItem([FromForm] OrderItemViewModel model)
      {
         if (!ModelState.IsValid)
            return PartialView(EditOrderItemViewName, model);

         var orderItem = new OrderItemEntity()
         {
            Id = model.Id,
            OrderEntityId = model.OrderId,
            Name = model.Name,
            Quantity = model.Quantity,
            Unit = model.Unit
         };

         var result = orderItemService.CreateUpdateOrderItem(orderItem);

         if (!result.Success)
            ModelState.AddModelError("", result.Message);

         return PartialView(EditOrderItemViewName, model);
      }

      [HttpDelete("items/delete/{id:int}")]
      public IActionResult DeleteOrderItem(int id)
      {
         var result = orderItemService.DeleteOrderItem(id);
         if (!result.Success)
            return BadRequest(result.Message);

         return Ok();
      }

      private bool IsOrderWithNumberAndProviderIdExists(string number, int providerId)
      {
         var filteringObject = new OrderFilteringObjectBuilder().AddNumberFilter(number)
                                                                .AddProviderIdFilter(providerId)
                                                                .Build();
         var orders = orderService.GetOrders(filteringObject);
         return orders.Any();
      }

      private static List<SelectListItem> GetProviderNameSelectList(IProviderService service)
      {
         var providers = service.GetProviders();
         return providers.Select(provider => new SelectListItem(provider.Name, provider.Id.ToString())).ToList();
      }

      private static OrderFilteringObject CreateOrderFilteringObject(OrderFilterValuesViewModel filteringValues)
      {
         var orderFilteringObjectBuilder = new OrderFilteringObjectBuilder();
         orderFilteringObjectBuilder.AddDateFilter(filteringValues.DateFrom, filteringValues.DateTo);

         if (filteringValues.SelectedOrderNumbers != null)
            orderFilteringObjectBuilder.AddNumberFilter(filteringValues.SelectedOrderNumbers.ToArray());

         if (filteringValues.SelectedProviderIds != null)
            orderFilteringObjectBuilder.AddProviderIdFilter(filteringValues.SelectedProviderIds.ToArray());

         if(filteringValues.SelectedOrderItemNames != null)
         {
            var orderItemNameFilter = OrderFilterFactory.CreateOrderItemNameFilter(filteringValues.SelectedOrderItemNames);
            orderFilteringObjectBuilder.AddCustomFilter(orderItemNameFilter);
         }

         if(filteringValues.SelectedOrderItemUnits != null)
         {
            var orderItemUnitFilter = OrderFilterFactory.CreateOrderItemUnitFilter(filteringValues.SelectedOrderItemUnits);
            orderFilteringObjectBuilder.AddCustomFilter(orderItemUnitFilter);
         }

         return orderFilteringObjectBuilder.Build();
      }
   }
}