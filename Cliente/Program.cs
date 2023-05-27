using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClienteJWT
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Autenticación de usuario...");

            // Datos de inicio de sesión del usuario
            var usuario = "juan";
            var contrasenia = "perez";

            // Construir objeto JSON con los datos de inicio de sesión
            var datosInicioSesion = new
            {
                Usuario = usuario,
                Contrasenia = contrasenia
            };

            // Convertir el objeto a una cadena JSON
            var jsonDatosInicioSesion = JsonSerializer.Serialize(datosInicioSesion);

            using (var httpClient = new HttpClient())
            {
                // URL de la API de autenticación
                var urlAutenticacion = "https://localhost:7174/api/autenticacion";

                // Realizar una solicitud POST para autenticar al usuario
                var respuesta = await httpClient.PostAsync(urlAutenticacion,
                    new StringContent(jsonDatosInicioSesion, Encoding.UTF8, "application/json"));

                if (respuesta.IsSuccessStatusCode)
                {
                    // Obtener el token de acceso del cuerpo de la respuesta
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<Token>(contenido).accessToken;

                    Console.WriteLine("Usuario autenticado correctamente.");
                    Console.WriteLine("Token de acceso: " + token);

                    // Hacer una solicitud al servidor protegido utilizando el token de acceso
                    await AccederServidorProtegido(token);
                }
                else
                {
                    Console.WriteLine("Error al autenticar al usuario.");
                    Console.WriteLine("Código de estado HTTP: " + (int)respuesta.StatusCode);
                }
            }

            Console.ReadKey();
        }

        static async Task AccederServidorProtegido(string token)
        {
            using (var httpClient = new HttpClient())
            {
                // URL del servidor protegido
                var urlServidorProtegido = "https://localhost:7149/api/Recurso";

                // Agregar el token de acceso en la cabecera de autorización
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Realizar una solicitud GET al servidor protegido
                var respuesta = await httpClient.GetAsync(urlServidorProtegido);

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    Console.WriteLine("Respuesta del servidor protegido: " + contenido);
                }
                else
                {
                    Console.WriteLine("Error al acceder al servidor protegido.");
                    Console.WriteLine("Código de estado HTTP: " + (int)respuesta.StatusCode);
                }
            }
        }
    }

    class Token
    {
        public string accessToken { get; set; }
    }
}