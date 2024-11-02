using Application.Models.ProductDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.OrderDetailDto
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }

        public decimal Price { get; set; }

        public static OrderDetailsDto Create(OrderDetail orderDetail)
        {
            return new OrderDetailsDto
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Product.Price * orderDetail.Quantity,
                Product =  ProductDto.Create(orderDetail.Product),
            };
        }
    }
}
