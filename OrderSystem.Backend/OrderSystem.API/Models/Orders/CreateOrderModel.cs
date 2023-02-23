namespace OrderSystem.API.Models.Orders
{
    public class CreateOrderModel
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ProviderName { get; set; }

        public CreateOrderItemModel[] Items { get; set; }
    }

    public class CreateOrderItemModel
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}