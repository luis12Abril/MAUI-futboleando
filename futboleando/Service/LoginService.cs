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

        // ✅ Método mejorado con mejor manejo de errores
        public async Task<LoginResponseCLS> login(LoginCLS oLoginCLS)
        {
            try
            {
                // Validación de conexión
                if (_httpClient.BaseAddress == null)
                {
                    return new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = "Error de configuración del servidor"
                    };
                }

                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Login", oLoginCLS);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponseCLS>();
                    return result ?? new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = "Error al procesar respuesta del servidor"
                    };
                }
                else
                {
                    return new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = $"Error del servidor (Código: {response.StatusCode})"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new LoginResponseCLS
                {
                    exito = false,
                    mensaje = "No se pudo conectar al servidor. Verifique su conexión a internet."
                };
            }
            catch (TaskCanceledException ex)
            {
                return new LoginResponseCLS
                {
                    exito = false,
                    mensaje = "La solicitud excedió el tiempo de espera. Intente nuevamente."
                };
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

        // ✅ Método de registro mejorado
        public async Task<RegistroResponseCLS> Registrar(RegistroRequestCLS oRegistroRequestCLS)
        {
            try
            {
                if (_httpClient.BaseAddress == null)
                {
                    return new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "Error de configuración del servidor"
                    };
                }

                var response = await _httpClient.PostAsJsonAsync("api/Usuario/Registrar", oRegistroRequestCLS);
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RegistroResponseCLS>();
                    return result ?? new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "Error al procesar respuesta del servidor"
                    };
                }
                else
                {
                    return new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = $"Error del servidor (Código: {response.StatusCode})"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                return new RegistroResponseCLS
                {
                    exito = false,
                    mensaje = "No se pudo conectar al servidor. Verifique su conexión a internet."
                };
            }
            catch (TaskCanceledException ex)
            {
                return new RegistroResponseCLS
                {
                    exito = false,
                    mensaje = "La solicitud excedió el tiempo de espera. Intente nuevamente."
                };
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
