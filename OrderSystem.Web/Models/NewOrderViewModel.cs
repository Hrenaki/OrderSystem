using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OrderSystem.Web.Models
{
   public class NewOrderViewModel
   {
      [Required(ErrorMessage = $"{nameof(Number)} isn't set")]
      public string Number { get; set; }

      [Required(ErrorMessage = $"{nameof(Date)} isn't set")]
      [DataType(DataType.Date)]
      public DateTime Date { get; set; }

      [Required(ErrorMessage = $"{nameof(ProviderId)} isn't set")]
      public int ProviderId { get; set; }

      [ValidateNever]
      public List<SelectListItem> ProviderNames { get; set; }
   }
}