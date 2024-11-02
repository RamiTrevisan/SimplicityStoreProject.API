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
    public class ProductsRepository : IProductsRepository
    {
        private readonly SimplicityStoreContext _context;

        public ProductsRepository (SimplicityStoreContext context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetProducts() // todos los productos
        {
            return _context.Products.Include(p => p.Category);
        }

        public Product? GetProductById (int idProduct) // un producto especifico por id
        {
            return _context.Products.Include(p => p.Category).Where( p => p.Id == idProduct).FirstOrDefault();
        }

        public void AddProduct (Product product) // agregar un producto 
        {
            _context.Products.Add(product);
        }
        public void DeleteProduct ( Product productDelete  )
        {
            var product = _context.Products.FirstOrDefault( p => p.Id == productDelete.Id );
            if ( product != null )
            {
                _context.Products.Remove( productDelete );
            }
        }

        public bool ProductExists ( string ProductName  )
        {
            return _context.Products.Where( x => x.Name == ProductName ).Any();
        }

        public void UpdateProduct ( Product product )
        {
            _context.Products.Update( product );
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0 );
        }

        public IEnumerable<Product> GetProductsByCategoryId( int categoryId )
        {
            return _context.Products.Where(p => p.CategoryId == categoryId);
        }

        public void ReducerStock(int idProduct, int Quantity)
        {
            var product = _context.Products.Where(p => p.Id == idProduct).FirstOrDefault();
            product.Stock = product.Stock - Quantity;
        }

        public void AddStock(int idProduct, int Quantity)
        {
            var product = _context.Products.Where(p => p.Id == idProduct).FirstOrDefault();
            product.Stock = product.Stock + Quantity;
        }
    }
}
