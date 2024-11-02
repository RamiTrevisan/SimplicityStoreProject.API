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
    public class ProductDto
    {

        public int Id { get; set; }

        public bool Available { get; set; }

        public string? Name { get; set; }


        public string? Description { get; set; }

        public int Stock { get; set; } = 0;

        [Required]

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public ProductCategoriesDto Category { get; set; }
        public static ProductDto Create(Product product)
        {
            var dto = new ProductDto();
            dto.Id = product.Id;
            dto.Available = product.Available;
            dto.Name = product.Name;
            dto.Description = product.Description;
            dto.Stock = product.Stock;
            dto.Price = product.Price;
            dto.CategoryId = product.CategoryId;
            dto.Category = ProductCategoriesDto.Create(product.Category);

            return dto;
        }
    }
}
