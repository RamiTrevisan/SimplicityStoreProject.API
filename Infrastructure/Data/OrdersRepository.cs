using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class OrdersRepository: IOrderRepository
    {
        private readonly SimplicityStoreContext _context;

        public OrdersRepository(SimplicityStoreContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order,int userId)
        {
            order.UserId = userId;
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public bool DeleteOrder(Order order)
        {
         
            _context.Orders.Remove(order);
            return SaveChanges();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product).ThenInclude(f => f.Category).ToList();
        }

        public Order? GetOrderById(int id)
        {
            return _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product).ThenInclude(f => f.Category).FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        }

        public IEnumerable<Order> GetOrdersByUserId(int idUser)
        {
            return _context.Orders.Include(o => o.OrderDetails).Where(o => o.UserId == idUser).ToList();
        }

        public bool OrderExists(int idOrder)
        {
            return _context.Orders.Any(o => o.Id == idOrder);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(Order order, List<OrderDetail> orderDetails )
        {
            var existingOrder = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == order.Id);

            if (existingOrder != null)
            {
                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                // Delete existing order details
                foreach (var existingDetail in existingOrder.OrderDetails.ToList())
                {
                    _context.OrderDetails.Remove(existingDetail);
                }

                // Add updated order details
                foreach (var detail in orderDetails)
                {
                    existingOrder.OrderDetails.Add(detail);
                }

                SaveChanges();
            }
        }

        }
}

