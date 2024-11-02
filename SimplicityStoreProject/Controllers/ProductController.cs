using Application.Models.ProductCategoriesDTOs;
using Application.Models.ProductDTOs;
using Application.Models.UserDTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimplicityStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUsersRepository _usersRepository;


        public ProductController(IProductCategoryRepository productCategoryRepository, IProductsRepository productsRepository, IUsersRepository usersRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productsRepository = productsRepository;
            _usersRepository = usersRepository;

        }


        [HttpPost]
        [Authorize]
        public ActionResult<ProductDto> CreateProduct([FromBody] ProductCreateDto productCreate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos para crear productos");
            }

            var isCategoryProduct = _productCategoryRepository.GetProductsCategoryById(productCreate.CategoryId);

            if (isCategoryProduct == null)
            {
                return BadRequest("La categoría del producto no existe");
            }
            // pasamos el ProductCreateDto a la entidad User para poderla guardar en base de datos
            Product newProduct = ProductCreateDto.ToProduct(productCreate);

            _productsRepository.AddProduct(newProduct);
            _productsRepository.SaveChanges();

            // pasamos de entidad a ProductCreateDto para mostrarlo como respuesta

            return Ok(ProductDto.Create(newProduct));


        }

        [HttpPut]
        [Authorize]

        public ActionResult<ProductDto> PutUser(int id, ProductUpdateDto productUpdate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");


            var user = _usersRepository.GetUser(userId);


            var product = _productsRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound("No existe ese Producto con ese id");
            }    


            if ( user.Role != "Admin")
            {
                return BadRequest("No tenes los permisos para editar productos");
            }


            product.Name = productUpdate.Name;
            product.Description = productUpdate.Description;
            product.Stock = productUpdate.Stock;
            product.Price = productUpdate.Price;
            product.CategoryId = productUpdate.CategoryId;


            _productsRepository.SaveChanges();


            return Ok("product Editado correctamente");

        }


        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteProduct(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _usersRepository.GetUser(userId);

        

            var ProductToDelete = _productsRepository.GetProductById(id);

            if (ProductToDelete == null)
            {
                return NotFound("No existe ese Producto con ese id");

            }

            if ( user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes.");
            }

            _productsRepository.DeleteProduct(ProductToDelete);
            _productsRepository.SaveChanges();

            return Ok("Producto eliminado correctamente");
        }



        [HttpGet]
        public ActionResult<List<ProductDto>> GetProductAll()
        {
            var products = _productsRepository.GetProducts();

            var productsDtos = products.Select(products => ProductDto.Create(products)).ToList();

            return productsDtos;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var products = _productsRepository.GetProductById(id);

            if (products == null)
            {
                return NotFound("No existe ese Producto con ese id");
            }

            return Ok(ProductDto.Create(products));
        }


    }
}
