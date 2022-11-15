using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystem.Core.Services;
using OrderSystem.Data.Entities;
using OrderSystem.Web.Models;

namespace OrderSystem.Web.Controllers
{
   [Route("orders")]
   public class OrderController : Controller
   {
      private readonly IOrderService orderService;
      private readonly IProviderService providerService;

      private const string newOrderModalViewName = "NewOrderModal";
      private const string IndexViewName = "Index";
      private const string EditOrderViewName = "EditOrder";

      public OrderController(IOrderService orderService, IProviderService providerService)
      {
         ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
         ArgumentNullException.ThrowIfNull(providerService, nameof(providerService));

         this.orderService = orderService;
         this.providerService = providerService;
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
            OrderId = orderItem.OrderItemEntityId,
            Name = orderItem.Name,
            Quantity = orderItem.Quantity,
            Unit = orderItem.Unit
         }).ToList();

         var viewModel = new EditOrderWithItemsViewModel()
         {
            OrderViewModel = editOrderViewModel,
            OrderItemViewModels = orderItemList
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

         var filteringObjectBuilder = new OrderFilteringObjectBuilder();
         filteringObjectBuilder.AddDateFilter(filteringValues.DateFrom, filteringValues.DateTo);

         if (filteringValues.SelectedOrderNumbers != null)
            filteringObjectBuilder.AddNumberFilter(filteringValues.SelectedOrderNumbers.ToArray());

         if (filteringValues.SelectedProviderIds != null)
            filteringObjectBuilder.AddProviderIdFilter(filteringValues.SelectedProviderIds.ToArray());

         var orders = orderService.GetOrders(filteringObjectBuilder.Build());
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
   }
}