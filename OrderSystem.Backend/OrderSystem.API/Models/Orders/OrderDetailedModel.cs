using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystem.API.Models.Orders
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
    }

    public class OrderDetailedModel : OrderModel
    {
        public OrderItemModel[] Items { get; set; }
    }
}