using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.OrderDTOs
{
    public class OrderCreateDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public static Order ToOrder(OrderCreateDto orderCreate)
        {
            return new Order
            {
                OrderDate = orderCreate.OrderDate,
            };
        }
    }
}
