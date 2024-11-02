using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Icon { get; set; }

        [Required]
        [MaxLength(250)]

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ICollection<Product>? Products { get; set; } // Esta propiedad define una relación uno a muchos entre

    }
}
