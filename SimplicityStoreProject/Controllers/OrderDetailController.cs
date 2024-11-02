using Application.Models.OrderDetailDto;
using Application.Models.OrderDTOs;
using Application.Models.UserDTOs;
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
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly IOrderDetailRepository _ordersDetailRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProductsRepository _productsRepository;



        public OrderDetailController(IOrderRepository ordersRepository, IOrderDetailRepository ordersDetailRepository, IUsersRepository usersRepository, IProductsRepository productsRepository )
        {
            _ordersRepository = ordersRepository;
            _ordersDetailRepository = ordersDetailRepository;
            _usersRepository = usersRepository;
            _productsRepository = productsRepository;
        }

        [HttpGet("Admin")]
        [Authorize]
        public ActionResult<List<OrderDetailsDto>> GetOrders()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes o no es tu orden");
            }

            var ordersDetail = _ordersDetailRepository.GetAllOrdersDetail();

            if (ordersDetail == null || !ordersDetail.Any())
            {
                return NotFound();
            }

            var ordersDetailDtos = ordersDetail.Select(order => OrderDetailsDto.Create(order)).ToList();

            return Ok(ordersDetailDtos);
        }

        [HttpGet("User")]
        [Authorize]
        public ActionResult<List<OrderDetailsDto>> GetOrdenDetailUser()
        {
            // Obtener el userId del token de usuario
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            // Obtener la información del usuario
            var user = _usersRepository.GetUser(userId);

            // Obtener todos los detalles de las órdenes
            var ordersDetail = _ordersDetailRepository.GetAllOrdersDetail();

            // Verificar si existen detalles de órdenes
            if (ordersDetail == null || !ordersDetail.Any())
            {
                return NotFound();
            }
            // Filtrar los detalles de las órdenes según el rol del usuario
            
            ordersDetail = ordersDetail.Where(order => _ordersRepository.GetOrderById(order.OrderId)?.UserId == userId).ToList();
            
            // Verificar si hay detalles de órdenes después del filtrado
            if (!ordersDetail.Any())
            {
                return BadRequest("No tienes permisos suficientes o no tienes órdenes.");
            }

            // Convertir los detalles de las órdenes a DTOs
            var ordersDetailDtos = ordersDetail.Select(order => OrderDetailsDto.Create(order)).ToList();

            return Ok(ordersDetailDtos);
        }

        [HttpGet("{id}")]
        [Authorize]

        public ActionResult<OrderDetailsDto> GetOrdersDetail(int id)
        {
            // Obtener el userId del token de usuario
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            // Obtener la información del usuario
            var user = _usersRepository.GetUser(userId);

            // Obtener los detalles de la orden
            var orderDetail = _ordersDetailRepository.GetOrderDetailById(id);

            // Verificar si los detalles de la orden existen
            if (orderDetail == null)
            {
                return NotFound("No existe o no es tu OrdenDetail");
            }

            // Obtener la orden asociada con los detalles
            var order = _ordersRepository.GetOrderById(orderDetail.OrderId);

            // Verificar si la orden existe
            if (order == null)
            {
                return NotFound("No existe esa orden");
            }

            // Verificar si el usuario es admin o si es el dueño de la orden
            if (user.Role != "Admin" && order.UserId != userId)
            {
                return BadRequest("No tienes los permisos suficientes o no es tu orden");
            }

            // Devolver los detalles de la orden
            return Ok(OrderDetailsDto.Create(orderDetail));
        }

        [HttpDelete]
        [Authorize]

        public ActionResult DeleteOrderDetail(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            var OrderDetail = _ordersDetailRepository.GetOrderDetailById(id);


            if (OrderDetail == null)
            {
                return NotFound("No existe esa OrdenDetail.");
            }

            var Order = _ordersRepository.GetOrderById(OrderDetail.OrderId);

            if (Order == null)
            {
                return NotFound("No existe esa Orden.");
            }

            if (Order.UserId != userId)
            {
                return BadRequest("No tienes los permisos suficientes.");
            }

            _productsRepository.AddStock(OrderDetail.ProductId,OrderDetail.Quantity);
            _productsRepository.SaveChanges();

            _ordersDetailRepository.DeleteDetailOrder(OrderDetail);
            _ordersDetailRepository.SaveChanges();


            return Ok("OrdenDetail eliminada Correctamente");        

        }

        [HttpPost]
        [Authorize]
        public ActionResult<OrderDetailsDto> CreateOrderDetail(int OrderId,[FromBody] OrderDetailCreateDto orderDeatailCreate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);


            var product = _productsRepository.GetProductById(orderDeatailCreate.ProductId);

            if (product == null)
            {
                return NotFound("no existe ese product con el id");

            }

            OrderDetail newOrderDetail = OrderDetailCreateDto.ToOrderDetail(orderDeatailCreate);

            var Order = _ordersRepository.GetOrderById(OrderId);

            if (Order == null)
            {
                return NotFound("no existe esa orden");
            }

            if (Order.UserId != userId)
            {
                return BadRequest("no es tu order");
            }

            if (product.Available == false) 
            {
                return BadRequest("el producto No esta disponible");
            }

            if (orderDeatailCreate.Quantity <= 0)
            {
                return BadRequest("Selecciona una cantidad");
            }

            if (product.Stock < orderDeatailCreate.Quantity)
            {
                return BadRequest("El producto no tiene suficiente Stock");
            }

            _productsRepository.ReducerStock(product.Id, orderDeatailCreate.Quantity);



            newOrderDetail.OrderId = OrderId;
            newOrderDetail.Price = product.Price * orderDeatailCreate.Quantity;


            _ordersDetailRepository.AddDetail(newOrderDetail);
            _productsRepository.SaveChanges();
            _ordersDetailRepository.SaveChanges();


            return Ok(OrderDetailsDto.Create(newOrderDetail));


        }


    }
}
