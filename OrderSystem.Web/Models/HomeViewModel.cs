using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderSystem.Web.Models
{
   public class HomeViewModel
   {
      public OrderFilterValuesViewModel FilterValuesViewModel { get; set; }
      public NewOrderViewModel OrderViewModel { get; set; }
   }
}