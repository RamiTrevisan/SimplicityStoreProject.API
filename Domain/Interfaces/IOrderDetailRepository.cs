using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderDetailRepository
    {
        void AddDetail(OrderDetail detail);
        void DeleteDetailOrder(OrderDetail detail);
        IEnumerable<OrderDetail> GetAllOrdersDetail();
        OrderDetail? GetOrderDetailById(int id);
        IEnumerable<OrderDetail> GetOrderDetailDetailsByOrderId(int orderId);
        IEnumerable<OrderDetail> GetOrdersDetailByUserId(int idUser);
        bool OrderDetailExists(int idOrder);
        bool SaveChanges();
    }
}
