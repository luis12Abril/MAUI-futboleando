using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Jugador;
using futboleandoEntities.Usuario;
using futboleandoEntities.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public UsuarioController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        // ✅ NUEVO ENDPOINT DE LOGIN CON SHA-256
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginCLS loginRequest)
        {
            try
            {
                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(loginRequest.nombreusuario) || string.IsNullOrWhiteSpace(loginRequest.contra))
                {
                    return Ok(new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = "Usuario y contraseña son requeridos"
                    });
                }

                // Convertir contraseña a SHA-256
                string passwordHash = ConvertirSHA256(loginRequest.contra);

                // ✅ Buscar usuario SIN DISTINGUIR mayúsculas/minúsculas
                var usuario = (from u in _bd.Usuarios
                              join t in _bd.Tipousuarios on u.Idtipousuario equals t.Idtipousuario
                              where u.Nombre.ToLower() == loginRequest.nombreusuario.ToLower()  // ✅ Case-insensitive
                                 && u.Contraseña == passwordHash
                                 && u.Habilitado == 1
                              select new
                              {
                                  u.Idusuario,
                                  u.Nombre,
                                  u.Idtipousuario,
                                  NombreTipoUsuario = t.Nombre,
                                  u.Visitascel
                              }).FirstOrDefault();

                if (usuario == null)
                {
                    return Ok(new LoginResponseCLS
                    {
                        exito = false,
                        mensaje = "Usuario o contraseña incorrectos"
                    });
                }

                // ✅ Incrementar VISITASCEL en 1
                var usuarioEntity = _bd.Usuarios.Find(usuario.Idusuario);
                if (usuarioEntity != null)
                {
                    usuarioEntity.Visitascel = (usuarioEntity.Visitascel ?? 0) + 1;
                    _bd.SaveChanges();
                }

                // Retornar respuesta exitosa
                return Ok(new LoginResponseCLS
                {
                    exito = true,
                    mensaje = "Login exitoso",
                    idusuario = usuario.Idusuario,
                    nombre = usuario.Nombre,
                    idtipousuario = usuario.Idtipousuario ?? 2,
                    nombretipousuario = usuario.NombreTipoUsuario
                });
            }
            catch (Exception ex)
            {
                return Ok(new LoginResponseCLS
                {
                    exito = false,
                    mensaje = $"Error en el servidor: {ex.Message}"
                });
            }
        }

        // ✅ Método para convertir texto a SHA-256
        private string ConvertirSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString().ToUpper();
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = _bd.Usuarios.Where(p => p.Idtipousuario == 3).Select(p => new UsuarioListCLS
                {
                    idusuario = p.Idusuario,
                    nombre = p.Nombre,
                    idtipousuario = p.Idtipousuario ?? 0, // Si es null, asigna 0
                    visitas = p.Visitas ?? 0,
                    visitascel = p.Visitascel ?? 0

                }).ToList();
                return Ok(consulta);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("ListarUsuariosPorTipo/{idtipo}")]
        public IActionResult ListarUsuariosPorTipo(int idtipo)
        {
            try
            {
                var consulta = (from u in _bd.Usuarios
                               join t in _bd.Tipousuarios on u.Idtipousuario equals t.Idtipousuario 
                               where u.Habilitado == 1 && u.Idtipousuario == idtipo
                    select new UsuarioTipoListCLS
                {
                    idusuario = u.Idusuario,
                    nombre = u.Nombre,
                    idtipousuario = u.Idtipousuario ?? 0, // Si es null, asigna 0
                    visitas = u.Visitas ?? 0,
                    visitascel = u.Visitascel ?? 0,
                    nombretipousuario = t.Nombre

                    }).ToList();

                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("listarusuariosmasvisitascelular")]
        public IActionResult ListarUsuariosMasVisitasCelular()
        {
            try
            {
                var consulta = _bd.Usuarios.Where(p => p.Visitascel > p.Visitas).Select(p => new UsuarioMasVisitasCelularListCLS
                {
                    idusuario = p.Idusuario,
                    nombre = p.Nombre,
                    idtipousuario = p.Idtipousuario ?? 0, // Si es null, asigna 0
                    visitas = p.Visitas ?? 0,
                    visitascel = p.Visitascel ?? 0,
                    fechaalta = (DateTime)p.Fechaalta

                }).ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ NUEVO ENDPOINT DE REGISTRO
        [HttpPost("Registrar")]
        public IActionResult Registrar([FromBody] RegistroRequestCLS registroRequest)
        {
            try
            {
                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(registroRequest.nombreusuario) || string.IsNullOrWhiteSpace(registroRequest.contra))
                {
                    return Ok(new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "Usuario y contraseña son requeridos"
                    });
                }

                // Validar longitud mínima de contraseña
                if (registroRequest.contra.Length < 4)
                {
                    return Ok(new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "La contraseña debe tener al menos 4 caracteres"
                    });
                }

                // ✅ Verificar si el usuario ya existe (sin distinguir mayúsculas/minúsculas)
                var usuarioExistente = _bd.Usuarios
                    .FirstOrDefault(u => u.Nombre.ToLower() == registroRequest.nombreusuario.ToLower());

                if (usuarioExistente != null)
                {
                    return Ok(new RegistroResponseCLS
                    {
                        exito = false,
                        mensaje = "El nombre de usuario ya está registrado. Por favor elige otro."
                    });
                }

                // ✅ Convertir contraseña a SHA-256
                string passwordHash = ConvertirSHA256(registroRequest.contra);

                // ✅ Crear nuevo usuario con los datos especificados
                var nuevoUsuario = new Usuario
                {
                    Nombre = registroRequest.nombreusuario,
                    Contraseña = passwordHash,
                    Idpersona = 1,
                    Idtipousuario = 2,
                    Token = null,  // Vacío
                    Habilitado = 1,
                    Visitas = 0,
                    Visitascel = 1,  // Ya tiene una visita desde celular (el registro)
                    Idarbitrocolegio = 0,
                    Fechaalta = DateTime.Now,  // Fecha actual
                    Origenalta = "CEL"
                };

                // Guardar en base de datos
                _bd.Usuarios.Add(nuevoUsuario);
                _bd.SaveChanges();

                // Retornar respuesta exitosa
                return Ok(new RegistroResponseCLS
                {
                    exito = true,
                    mensaje = "Usuario registrado exitosamente",
                    idusuario = nuevoUsuario.Idusuario,
                    nombre = nuevoUsuario.Nombre
                });
            }
            catch (Exception ex)
            {
                return Ok(new RegistroResponseCLS
                {
                    exito = false,
                    mensaje = $"Error en el servidor: {ex.Message}"
                });
            }
        }
    }
}
