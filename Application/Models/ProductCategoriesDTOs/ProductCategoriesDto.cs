using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.ProductDTOs;
using Domain.Entities;

namespace Application.Models.ProductCategoriesDTOs
{
    public class ProductCategoriesDto
    {

        public int Id { get; set; }
        public string? Icon { get; set; }


        public string? Name { get; set; }

        public string? Description { get; set; }



        public static ProductCategoriesDto Create(ProductCategory productCategory)
        {
            var dto = new ProductCategoriesDto();
            dto.Id = productCategory.Id;
            dto.Icon = productCategory.Icon;
            dto.Name = productCategory.Name;
            dto.Description = productCategory.Description;


            return dto;
        }

    }
}
