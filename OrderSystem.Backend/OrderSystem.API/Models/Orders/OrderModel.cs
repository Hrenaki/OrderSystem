namespace OrderSystem.API.Models.Orders
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
    }
}