using Application.Models.UserDTOs;
using Infrastructure.Data;
using Domain.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Entities;

namespace SimplicityStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;

        public UserController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        [HttpPost]

        public ActionResult<UserDto> Register([FromBody] UserCreateDto userCreate)
        {

            if (userCreate.Password.Length < 5)
            {
                return BadRequest("La contraseña debe tener más de 5 caracteres.");

            }

            if (_usersRepository.UserNameExists(userCreate.Username))
            {
                return BadRequest("El usuario ya existe.");
            }



            if (_usersRepository.EmailExists(userCreate.Email))
            {
                return BadRequest("El email ya existe.");
            }

            if (!_usersRepository.IsValidEmail(userCreate.Email))
            {
                return BadRequest("El email no tiene un formato válido.");
            }


            User newUser = UserCreateDto.ToUser(userCreate);

            _usersRepository.AddUser(newUser);
            _usersRepository.SaveChanges();


            return Ok("Usuario creado correctamente");


        }

        [HttpPut]
        [Authorize]

        public ActionResult<UserDto> PutUser(int id, UserUpdateDto userUpdate)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");


            var user = _usersRepository.GetUser(userId);



            var userEdit = _usersRepository.GetUser(id);

            if (userEdit == null)
            {
                return NotFound("El id del usuario a editar no existe.");
            }


            if (userEdit.Id != userId && user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos para editar este usuario");
            }

            if (!_usersRepository.IsValidEmail(userUpdate.Email))
            {
                return BadRequest("El email no tiene un formato válido.");
            }

            var userToEdit = _usersRepository.GetUser(id);

            userToEdit.Email = userUpdate.Email;

            _usersRepository.SaveChanges();


            return Ok("Usuario Editado correctamente.");

        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _usersRepository.GetUser(userId);

   

            var userToDelete = _usersRepository.GetUser(id);

            if (userToDelete == null)
            {
                return NotFound("El id del usuario a eliminar no existe.");
            }

            if (userToDelete.Id != userId && user.Role != "Admin")
            {
                return BadRequest("No tienes los permisos suficientes.");
            }

            _usersRepository.DeleteUser(userToDelete);
            _usersRepository.SaveChanges();

            return Ok("Usuario eliminado correctamente");
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<UserDto>> GetUserAll()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");


            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                return NotFound("No tienes los permisos para obtener todos los usuarios.");
            }

            var users = _usersRepository.GetUsers();

            if (users == null)
            {
                return NotFound();
            }
            var userDtos = users.Select(user => UserDto.Create(user)).ToList();

            return userDtos;
        }

        [HttpGet("{id}")]
        [Authorize]

        public ActionResult<UserDto> GetUser(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

            var user = _usersRepository.GetUser(userId);

            if (user.Role != "Admin")
            {
                if (user.Id != id)
                {
                    return NotFound("No tenes los permisos para obtener este usuario");
                }
            }



            var GetUser = _usersRepository.GetUser(userId);

            if (GetUser == null)
            {
                return NotFound("El id del usuario no existe.");
            }

            return UserDto.Create(GetUser);
        }

        [HttpGet("session")]
        [Authorize]
        public ActionResult<UserDto> GetUserSession()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var user = _usersRepository.GetUser(userId);
            return UserDto.Create(user);
        }

       

    }
}
