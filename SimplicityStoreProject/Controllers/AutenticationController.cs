using Application.Interfaces;
using Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AutenticationController(ICustomAuthenticationService autenticacionService)
        {
            _customAuthenticationService = autenticacionService;
        }

        [HttpPost("authenticate")] // Vamos a usar un POST ya que debemos enviar los datos para hacer el login
        public ActionResult<string> Autenticar(AuthenticationRequest authenticationRequest)
        {
            try
            {
                string token = _customAuthenticationService.Autenticar(authenticationRequest);

                if (token == null)
                {
                    return StatusCode(404, new { message = "Usuario o Contraseña incorrecta." });
                }
                return Ok(token);

            }

            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", details = ex.Message });
            }
        }
    }
}