using Application.Models.OrderDetailDto;
using Application.Models.UserDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public ICollection<OrderDetailsDto> OrderDetails { get; set; }

        public static OrderDto Create(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                User = UserDto.Create(order.User),
                OrderDetails = order.OrderDetails.Select(od => OrderDetailsDto.Create(od)).ToList(),
            };
        }
    }
}
