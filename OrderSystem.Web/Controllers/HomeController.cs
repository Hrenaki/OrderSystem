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

      public HomeController(IOrderService orderService, IProviderService providerService)
      {
         ArgumentNullException.ThrowIfNull(orderService, nameof(orderService));
         ArgumentNullException.ThrowIfNull(providerService, nameof(providerService));

         this.orderService = orderService;
         this.providerService = providerService;
      }

      [HttpGet]
      public IActionResult Index()
      {
         var orders = orderService.GetOrders();
         var orderNumbers = orders.DistinctBy(order => order.Number)
                                  .Select(order => new SelectListItem(order.Number, order.Number))
                                  .ToList();

         var providers = providerService.GetProviders();
         var distinctProviderNames = providers.DistinctBy(provider => provider.Name);
         var providerNames = distinctProviderNames
                                      .Select(provider => new SelectListItem(provider.Name, provider.Id.ToString()))
                                      .ToList();

         var filterValuesViewModel = new OrderFilterValuesViewModel()
         {
            DateFrom = DateTime.Now.AddMonths(-1),
            DateTo = DateTime.Now,
            OrderNumbers = orderNumbers,
            ProviderNames = providerNames
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
   }
}