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
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]

        public bool Available { get; set; } = true;

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string? Description { get; set; }

        public int Stock { get; set; } = 0;

        [Required]

        public decimal Price { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryId")]
        public ProductCategory? Category { get; set; }
        public int CategoryId { get; set; }

    }
}
