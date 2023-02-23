namespace OrderSystem.API.Models.Orders
{
    public class OrderResponse
    {
        public OrderDetailedModel Order { get; set; }
        public string Error { get; set; }
    }
}