using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
   public class OrderViewModel
   {
      public int Id { get; set; }

      public string Number { get; set; }

      public DateTime Date { get; set; }

      public string ProviderName { get; set; }
   }
}