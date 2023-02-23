namespace OrderSystem.API.Models.Orders
{
    public class OrdersResponse
    {
        public OrderModel[] Orders { get; set; }
        public string Error { get; set; }
    }
}