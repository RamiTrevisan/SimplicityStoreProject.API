using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.OrderDetailDto
{
    public class OrderDetailCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public static OrderDetail ToOrderDetail(OrderDetailCreateDto orderDetailCreate)
        {
            return new OrderDetail
            {
                ProductId = orderDetailCreate.ProductId,
                Quantity = orderDetailCreate.Quantity,
            };
        }
    }
}
