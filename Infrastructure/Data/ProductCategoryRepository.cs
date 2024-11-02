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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly SimplicityStoreContext _context;

        public ProductCategoryRepository (SimplicityStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductCategory> GetProductsCategory()
        {
            return _context.ProductCategories;
        }

        public ProductCategory? GetProductsCategoryById ( int idCategory )
        {
            return _context.ProductCategories.Where( pc =>  pc.Id == idCategory ).FirstOrDefault();
        }
        public ProductCategory? GetCategoryByName(string categoryName)
        {
            return _context.ProductCategories.Where(pc => pc.Name == categoryName).FirstOrDefault();
        }

        public void Update(ProductCategory category)
        {
            _context.ProductCategories.Update(category);
        }


        public void DeleteProductsCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Remove(productCategory);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool ProductsCategoryExists(string categoryName)
        {
            return _context.ProductCategories.Where(o => o.Name == categoryName).Any();
        }
        public void AddProductsCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
        }
    }
}
