using Application.Models.ProductCategoriesDTOs;
using Application.Models.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.ProductDTOs
{
    public class ProductCreateDto
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        public int Stock { get; set; }

        [Range(1.0, double.MaxValue, ErrorMessage = "El precio debe ser al menos 1.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El ID de la categoría es obligatorio.")]
        public int CategoryId { get; set; }

        public static Product ToProduct(ProductCreateDto productCreate)
        {
            return new Product
            {
                Name = productCreate.Name,
                Description = productCreate.Description,
                Stock = productCreate.Stock,
                Price = productCreate.Price,
                CategoryId = productCreate.CategoryId,


            };
        }
    }
}
