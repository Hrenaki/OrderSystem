namespace OrderSystem.API.Models.Orders
{
    public class CreateOrderModel
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
    }
}