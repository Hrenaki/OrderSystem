using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
   public class OrderEntity : Entity
   {
      [Column(TypeName = "nvarchar(max)")]
      public string Number { get; set; }

      [Column(TypeName = "datetime2(7)")]
      public DateTime Date { get; set; }

      public int UserId { get; set; }

      [Column("ProviderId")]
      [ForeignKey(nameof(ProviderEntity))]
      public int ProviderEntityId { get; set; }

      public virtual ProviderEntity ProviderEntity { get; set; }

      public List<OrderItemEntity> OrderItemEntities { get; set; } = new List<OrderItemEntity>();
   }
}