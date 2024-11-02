using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.UserDTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }

        public static UserDto Create(User user)
        {
            var dto = new UserDto();
            dto.Id = user.Id;
            dto.Username = user.UserName;
            dto.Password = user.Password;
            dto.Email = user.Email;
            dto.Role = user.Role;
            dto.DateTime = user.CreatedDate;
            return dto;
        }
    }
}
