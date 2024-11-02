using Application.Models.ProductDTOs;
using Application.Models.ProductCategoriesDTOs;

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimplicityStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IUsersRepository _usersRepository;

        public ProductsCategoryController(IProductCategoryRepository productCategoryRepository, IProductsRepository productsRepository, IUsersRepository usersRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productsRepository = productsRepository;
            _usersRepository = usersRepository;

        }

        [HttpGet]
        public ActionResult<List<ProductCategoriesDto>> GetProductCategoryAll()
        {
            var productsCategory = _productCategoryRepository.GetProductsCategory();

       
            var productsCategoryDtos = productsCategory.Select(productsCategory => ProductCategoriesDto.Create(productsCategory)).ToList();

            return productsCategoryDtos;
        }


        [HttpGet("{id}")]
        public ActionResult<ProductCategoriesDto> GetProductCategoryById(int id)
        {
            var productsCategory = _productCategoryRepository.GetProductsCategoryById(id);

            if (productsCategory == null)
            {
                return NotFound("No existe esa Categoria con esa id");
            }

            return Ok(ProductCategoriesDto.Create(productsCategory));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<ProductCategoriesDto> CreateProductCategory([FromBody] ProductCategoriesCreateDto productCategoryCreate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes.");
            }

            // pasamos el UserCreateDto a la entidad User para poderla guardar en base de datos
            ProductCategory newProductCategory = ProductCategoriesCreateDto.ToCategoryProduct(productCategoryCreate);

            _productCategoryRepository.AddProductsCategory(newProductCategory);
            _productCategoryRepository.SaveChanges();


            return Ok(ProductCategoriesDto.Create(newProductCategory));

        }

        [HttpPut]
        [Authorize]
        public ActionResult<ProductDto> PutProductCategory(int id, ProductCategoriesUpdateDto productCategoryUpdate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");


            var user = _usersRepository.GetUser(userId);


            var categoryRepository = _productCategoryRepository.GetProductsCategoryById(id);

            if (categoryRepository == null)
            {
                return NotFound("no existe ese product con el id");
            }

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes.");
            }


            categoryRepository.Name = productCategoryUpdate.Name;
            categoryRepository.Description = productCategoryUpdate.Description;
            categoryRepository.Icon = productCategoryUpdate.Icon;
            


            _productCategoryRepository.SaveChanges();


            return Ok("category product Editado");

        }


        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteProductCategory(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _usersRepository.GetUser(userId);



            var ProductToDelete = _productCategoryRepository.GetProductsCategoryById(id);

            if (ProductToDelete == null)
            {
                return NotFound("No existe el id");
            }

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes.");
            }

            _productCategoryRepository.DeleteProductsCategory(ProductToDelete);
            _productsRepository.SaveChanges();

            return Ok("ProductCategory  eliminado");
        }


    }
}
