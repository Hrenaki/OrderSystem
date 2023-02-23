using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystem.Core.Services;
using OrderSystem.Core.Services.Filters;
using OrderSystem.Data;
using OrderSystem.Data.Entities;
using OrderSystem.Web.Models;
using System.Diagnostics;
using System.Linq;

namespace OrderSystem.Web.Controllers
{
   public class HomeController : Controller
   {
      private readonly IOrderService orderService;
      private readonly IProviderService providerService;
      private readonly IOrderItemService orderItemService;

      public HomeController(IOrderService orderService,
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

      [HttpGet]
      public IActionResult Index()
      {
         var orderNumbers = GetOrderNumbersSelectList(orderService);
         var providerNames = GetProviderNamesSelectList(providerService);
         GetOrderItemNameAndUnitSelectList(orderItemService,
                                           out var orderItemNames,
                                           out var orderItemUnits);

         var filterValuesViewModel = new OrderFilterValuesViewModel()
         {
            DateFrom = DateTime.Now.AddMonths(-1),
            DateTo = DateTime.Now,
            OrderNumbers = orderNumbers,
            ProviderNames = providerNames,
            OrderItemNames = orderItemNames,
            OrderItemUnits = orderItemUnits
         };

         var viewModel = new HomeViewModel()
         {
            FilterValuesViewModel = filterValuesViewModel,
            OrderViewModel = new NewOrderViewModel()
            {
               Date = DateTime.Now,
               ProviderNames = providerNames
            }
         };
         return View(viewModel);
      }

      private static List<SelectListItem> GetOrderNumbersSelectList(IOrderService service)
      {
         var orders = service.GetOrders();
         return orders.Select(order => order.Number)
                      .Distinct()
                      .Select(number => new SelectListItem(number, number))
                      .ToList();
      }

      private static List<SelectListItem> GetProviderNamesSelectList(IProviderService service)
      {
         var providers = service.GetProviders();
         return providers.Select(provider => new SelectListItem(provider.Name, provider.Id.ToString()))
                         .ToList();
      }

      private static void GetOrderItemNameAndUnitSelectList(IOrderItemService service,
                                                            out List<SelectListItem> orderItemNames,
                                                            out List<SelectListItem> orderItemUnits)
      {
         var orderItems = service.GetOrderItems();
         orderItemNames = orderItems.Select(item => item.Name)
                                    .Distinct()
                                    .Select(name => new SelectListItem(name, name))
                                    .ToList();

         orderItemUnits = orderItems.Select(item => item.Unit)
                                    .Distinct()
                                    .Select(unit => new SelectListItem(unit, unit))
                                    .ToList();
      }
   }
}