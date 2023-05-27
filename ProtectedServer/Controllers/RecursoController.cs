using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServidorProtegido.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursoController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult ObtenerRecursoProtegido()
        {
            var usuario = User.Identity.Name;

            return Ok("Hola, " + usuario + ". Este es un recurso protegido.");
        }
    }
}