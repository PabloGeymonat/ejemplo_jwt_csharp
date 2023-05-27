using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServidorSeguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        [HttpPost]
        public IActionResult AutenticarUsuario([FromBody] Credenciales credenciales)
        {
            // Validar las credenciales del usuario (simulado aquí)
            if (credenciales.Usuario == "juan" && credenciales.Contrasenia == "perez")
            {
                // Generar un token JWT
                var token = GenerarTokenJwt(credenciales.Usuario);

                // Devolver el token de acceso en la respuesta
                return Ok(new { AccessToken = token });
            }

            return Unauthorized();
        }

        private string GenerarTokenJwt(String name)
        {
            // Clave secreta para firmar el token
            var claveSecreta = "clave_secreta_del_servidor_clave_secreta_del_servidor_clave_secreta_del_servidor_";

            // Crear los claims (información asociada al token)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,name)
            };

            // Crear la clave de seguridad usando la clave secreta
            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));

            // Generar el token JWT
            var token = new JwtSecurityToken(
                issuer: "https://servidor_seguridad",
                audience: "https://servidor_protegido",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(clave, SecurityAlgorithms.HmacSha256)
            );

            // Convertir el token en una cadena
            String tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }

    public class Credenciales
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
