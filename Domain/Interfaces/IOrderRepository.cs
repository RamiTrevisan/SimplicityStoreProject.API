using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository
    {
        void AddOrder(Order order, int userId);
        bool DeleteOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order? GetOrderById(int id);
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        IEnumerable<Order> GetOrdersByUserId(int idUser);
        bool OrderExists(int idOrder);
        bool SaveChanges();
        void Update(Order order, List<OrderDetail> orderDetails);

    }
}
