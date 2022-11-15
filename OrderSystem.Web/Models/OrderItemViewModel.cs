using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
	public class OrderItemViewModel
	{
		[ValidateNever]
		public int Id { get; set; }

		[ValidateNever]
		public int OrderId { get; set; }

		[Required(ErrorMessage = $"{nameof(Name)} isn't set")]
		public string Name { get; set; }

		[Required(ErrorMessage = $"{nameof(Quantity)} isn't set")]
		public decimal Quantity { get; set; }

		[Required(ErrorMessage = $"{nameof(Unit)} isn't set")]
		public string Unit { get; set; }
	}
}