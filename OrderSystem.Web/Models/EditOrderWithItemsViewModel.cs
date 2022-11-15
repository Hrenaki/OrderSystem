namespace OrderSystem.Web.Models
{
	public class EditOrderWithItemsViewModel
	{
		public EditOrderViewModel OrderViewModel { get; set; }
		public List<OrderItemViewModel> OrderItemViewModels { get; set; }
	}
}