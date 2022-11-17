namespace OrderSystem.Web.Models
{
	public class EditOrderItemsViewModel
	{
      public int OrderId { get; set; }
      public List<OrderItemViewModel> OrderItemViewModels { get; set; }
   }
}