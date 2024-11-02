using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order // pedidos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } 

        [Required]

        public int UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]

        public User? User { get; set; } 

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

  

    }
}
