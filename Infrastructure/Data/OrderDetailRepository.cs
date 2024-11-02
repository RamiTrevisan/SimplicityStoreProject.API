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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly SimplicityStoreContext _context;

        public OrderDetailRepository(SimplicityStoreContext context)
        {
            _context = context;
        }

        public void AddDetail(OrderDetail detail)
        {
            _context.OrderDetails.Add(detail);
            _context.SaveChanges();
        }

        public void DeleteDetailOrder(OrderDetail orderDetail)
        {
            _context.Remove(orderDetail);

        }

        public IEnumerable<OrderDetail> GetAllOrdersDetail()
        {
            return _context.OrderDetails.Include(c => c.Product).ThenInclude(f => f.Category);
        }

        public IEnumerable<OrderDetail> GetAllOrdersDetailUser(int idUser)
        {
            return _context.OrderDetails.Include(c => c.Product).ThenInclude(f => f.Category);
        }

        public OrderDetail? GetOrderDetailById(int id)
        {
            return _context.OrderDetails.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<OrderDetail> GetOrderDetailDetailsByOrderId(int orderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderDetail> GetOrdersDetailByUserId(int idUser)
        {
            throw new NotImplementedException();
        }

        public bool OrderDetailExists(int idOrder)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);

        }
    }
}
