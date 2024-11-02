using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.UserDTOs
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "El nombre de usuario no puede estar vacio")]
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static User ToUser(UserCreateDto userCreateDto)
        {
            return new User
            {
                UserName = userCreateDto.Username,
                Email = userCreateDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password),
            };
        }
    }
}
