using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        public IEnumerable<ProductCategory> GetProductsCategory();
        void DeleteProductsCategory(ProductCategory productToDelete);
        void Update(ProductCategory productCategory);
        ProductCategory? GetProductsCategoryById(int idProductsCategory);
        void AddProductsCategory(ProductCategory productCategory);
        bool SaveChanges();
        bool ProductsCategoryExists(string productsCategoryName);
    }
}
