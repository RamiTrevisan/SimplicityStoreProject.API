using Application.Interfaces;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Models.UserDTOs;

namespace Infrastructure.Services
{
    public class AutenticacionService : ICustomAuthenticationService
    {
        private readonly IUsersRepository _userRepository;
        private readonly AutenticacionServiceOptions _options;

        public AutenticacionService(IUsersRepository userRepository, IOptions<AutenticacionServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        // El metodo toma una solicitud de autenticar y devuelve un token si es exitosa
        public string Autenticar(AuthenticationRequest authenticationRequest)
        {
            // Verifica que el nombre de usuario y la contraseña no estén vacíos.
            if (string.IsNullOrEmpty(authenticationRequest.UserName) || string.IsNullOrEmpty(authenticationRequest.Password))
                return null;

            // Llama al método Login del repositorio de usuarios para encontrar al usuario por nombre de usuario.

            var user = _userRepository.Login(authenticationRequest.UserName);

            // Si no se encuentra el usuario, devuelve null.

            if (user == null) return null;

            // Verifica que la contraseña proporcionada coincida con la contraseña almacenada en la base de datos
            // usando la funcion BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, user.Password);

            if (!isPasswordValid) return null;

            // crea el token
            // Crea una clave de seguridad simétrica a partir de la clave secreta.
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));
            // Crea las credenciales de firma usando la clave de seguridad y el algoritmo HMAC-SHA256.
            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            // crea una lista de claim para incluir en el token
            var claimsForToken = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("given_name", user.UserName),
                new Claim("given_role", user.Role)

            };

            // crea el token
            var jwtSecurityToken = new JwtSecurityToken(
              _options.Issuer,
              _options.Audience,
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddMinutes(1),
              credentials);

            // Convierte el token en una cadena que puede ser devuelta.
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }

        // Esta clase define las opciones de configuración para el servicio de autenticación.
        public class AutenticacionServiceOptions
        {
            public const string AutenticacionService = "AutenticacionService";

            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretForKey { get; set; }
        }
    }
}