using Application.Models.OrderDetailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.OrderDTOs
{
    public class OrderUpdateDto
    {
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderDetailUpdateDto> OrderDetails { get; set; }
    }
}
