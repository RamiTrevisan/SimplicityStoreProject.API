using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductsRepository
    {
        public IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsByCategoryId(int categoryId);
        void DeleteProduct(Product productToDelete);
        //void Update(Product product);
        Product? GetProductById(int idProduct);
        void AddProduct(Product product);

        void ReducerStock(int idProduct, int Quantity);

        void AddStock(int idProduct, int Quantity);


        
        bool SaveChanges();
        bool ProductExists(string productName);
    }
}
