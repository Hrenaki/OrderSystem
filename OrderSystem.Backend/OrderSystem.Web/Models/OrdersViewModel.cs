using OrderSystem.Data.Entities;

namespace OrderSystem.Web.Models
{
   public class OrdersViewModel
   {
      public OrderViewModel[] Orders { get; init; }

      public OrdersViewModel(IEnumerable<OrderEntity> orderEntities)
      {
         Orders = orderEntities.Select(entity => new OrderViewModel()
         {
            Id = entity.Id,
            Number = entity.Number,
            Date = entity.Date,
            ProviderName = entity.ProviderEntity.Name
         }).ToArray();
      }
   }
}