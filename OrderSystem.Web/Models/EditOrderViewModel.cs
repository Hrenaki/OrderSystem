using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Web.Models
{
	public class EditOrderViewModel : NewOrderViewModel
	{
		public int Id { get; set; }
	}
}
