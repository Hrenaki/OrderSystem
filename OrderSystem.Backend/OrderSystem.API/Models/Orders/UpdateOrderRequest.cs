using System.Reflection;

namespace OrderSystem.API.Models.Orders
{
    public class UpdateOrderRequest
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ProviderId { get; set; }

        public UpdatedOrderItem[] Items { get; set; }
    }

    public class UpdatedOrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }
}