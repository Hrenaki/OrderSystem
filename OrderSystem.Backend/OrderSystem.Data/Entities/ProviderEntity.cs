using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
   [Table("Provider")]
   public class ProviderEntity : Entity
   {
      public string Name { get; set; }

      public List<OrderEntity> OrderEntities { get; set; }
   }
}