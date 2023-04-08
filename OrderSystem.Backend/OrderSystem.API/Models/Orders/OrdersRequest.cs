namespace OrderSystem.API.Models.Orders
{
    public class OrdersRequest
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int[] ProviderIds { get; set; }
    }
}