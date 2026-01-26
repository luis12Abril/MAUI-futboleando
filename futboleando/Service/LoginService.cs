using futboleandoEntities.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ✅ Método mejorado que consume el API
        public async Task<LoginResponseCLS> login(LoginCLS oLoginCLS)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Login", oLoginCLS);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseCLS>();
                    return result;
                }
                else
                {
                    return new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = "Error al conectar con el servidor"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponseCLS
                {
                    exito = false,
                    mensaje = $"Error de conexión: {ex.Message}"
                };
            }
        }

        // ✅ Método de registro que consume el API
        public async Task<RegistroResponseCLS> Registrar(RegistroRequestCLS oRegistroRequestCLS)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Registrar", oRegistroRequestCLS);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RegistroResponseCLS>();
                    return result;
                }
                else
                {
                    return new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "Error al conectar con el servidor"
                    };
                }
            }
            catch (Exception ex)
            {
                return new RegistroResponseCLS
                {
                    exito = false,
                    mensaje = $"Error de conexión: {ex.Message}"
                };
            }
        }
    }
}
