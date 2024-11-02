using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Email { get; set; }

        public string Role { get; set; } = "client";

        public DateTime CreatedDate { get; private set; } = DateTime.Now;
    }
}
