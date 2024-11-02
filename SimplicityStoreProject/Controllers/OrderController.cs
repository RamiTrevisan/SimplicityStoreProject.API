using Application.Models.OrderDetailDto;
using Application.Models.OrderDTOs;
using Application.Models.ProductDTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SimplicityStoreProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IOrderDetailRepository _ordersDetailRepository;

        private readonly IProductsRepository _productsRepository;


        public OrderController(IOrderRepository ordersRepository, IUsersRepository usersRepository, IProductsRepository productsRepository, IOrderDetailRepository ordersDetailRepository)

        {
            _ordersRepository = ordersRepository;
            _usersRepository = usersRepository;
            _productsRepository = productsRepository;
            _ordersDetailRepository = ordersDetailRepository;

        }

        [HttpGet("Admin")]
        [Authorize]
        public ActionResult<List<OrderDto>> GetOrders()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes permisos suficientes");
            }

            var orders = _ordersRepository.GetAllOrders();

            if (orders == null || !orders.Any())
            {
                return NotFound("No hay Orden en el sistema");
            }

            var ordersDtos = orders.Select(order => OrderDto.Create(order)).ToList();

            return Ok(ordersDtos);
        }

        [HttpGet("User")]
        [Authorize]
        public ActionResult<List<OrderDto>> GetOrderUser()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            var orders = _ordersRepository.GetAllOrders();

            orders = orders.Where(order => order.UserId == userId).ToList();

            if (orders == null || !orders.Any())
            {
                return NotFound("No hay Orden en el sistema");
            }

            var ordersDtos = orders.Select(order => OrderDto.Create(order)).ToList();
            return Ok(ordersDtos);
        }

        [HttpGet("{id}")]
        [Authorize]

        public ActionResult<OrderDto> GetOrders(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);                                  

            var Order = _ordersRepository.GetOrderById(id);

            if (Order == null)
            {
                return NotFound("No existe esa Orden");
            }

            if (user.Role != "Admin" && Order.UserId != userId)
            {
                return BadRequest("No tienes los permiso para ver esa Orden");
            }

            return Ok(OrderDto.Create(Order));

        }
        [HttpPost]
        [Authorize]
        public ActionResult<OrderDto> CreateProduct([FromBody] OrderCreateDto orderCreate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            // pasamos el UserCreateDto a la entidad User para poderla guardar en base de datos
            Order newOrder = OrderCreateDto.ToOrder(orderCreate);

            _ordersRepository.AddOrder(newOrder, userId);
            _ordersRepository.SaveChanges();

            return Ok(OrderDto.Create(newOrder));


        }

        [HttpDelete]
        [Authorize]

        public ActionResult DeleteOrder(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            var Order = _ordersRepository.GetOrderById(id);

            if (Order == null)
            {
                return NotFound("No existe esa Orden");
            }
            if (user.Role != "Admin")
            {
                if (Order.UserId != userId)
                {
                    return BadRequest("No tienes permiso para borrar esa Orden");
                }
            }         

            var orderDetails = _ordersRepository.GetOrderDetailsByOrderId(id);
            foreach (var orderDetail in orderDetails)
            {
                _productsRepository.AddStock(orderDetail.ProductId, orderDetail.Quantity);
            }

            _productsRepository.SaveChanges();

            foreach (var orderDetail in orderDetails)
            {
                _ordersDetailRepository.DeleteDetailOrder(orderDetail);
            }

            _ordersDetailRepository.SaveChanges();
            _ordersRepository.DeleteOrder(Order);
            _ordersRepository.SaveChanges();

            return Ok("Orden eliminada correctamente");

        }
    }
}
