using futboleandoAPIS.Models;
using futboleandoEntities;
using futboleandoEntities.Jugador;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using futboleandoEntities.Equipo;
using futboleandoEntities.Goleador;
using futboleandoEntities.JugadoresPorAño;
using Microsoft.EntityFrameworkCore;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;
        public JugadorController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        // ✅ Endpoint sin parámetro - devuelve todos los jugadores habilitados
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Habilitado == 1 
                                   && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"  // ✅ Excluir autogoles
                                   && e.Nombre.Trim() != "_SIN EQUIPO"  // ✅ También excluye jugadores sin equipo
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,
                                    nombreequipo = e.Nombre,
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    fnacimiento = (DateOnly)j.Fnacimiento,
                                    goles = j.Goles ?? 0
                                })
                                .OrderByDescending(j => j.fnacimiento)   // ✅ Ordenar por fecha de nacimiento  (los mas chicos primero)
                                .ThenBy(j => j.nombrecompleto)           // ✅ Desempate alfabético
                                .ToList();
                
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ Endpoint con parámetro de torneo - devuelve jugadores del torneo específico
        [HttpGet("PorTorneo/{idTorneo}")]
        public IActionResult GetPorTorneo(int idTorneo)
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idtorneo == idTorneo 
                                   && j.Habilitado == 1
                                   && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"  // ✅ Excluir autogoles
                                   && e.Nombre.Trim() != "_SIN EQUIPO"  // ✅ También excluye jugadores sin equipo
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno,
                                    apmaterno = j.Apmaterno,
                                    nombreequipo = e.Nombre,
                                    nombrecompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    fnacimiento = (DateOnly)j.Fnacimiento,
                                    goles = j.Goles ?? 0
                                })
                                .OrderByDescending(j => j.fnacimiento)  // ✅ Ordenar por fecha de nacimiento  (los mas chicos primero)
                                .ThenBy(j => j.nombrecompleto)    // ✅ Desempate alfabético
                                .ToList();
                
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var obj = _bd.Jugadors.FirstOrDefault(j => j.Idjugador == id);
                if (obj == null)
                {
                    return NotFound();
                }
                return Ok(new JugadorFormCLS
                {
                    idjugador = obj.Idjugador,
                    nombre = obj.Nombre,
                    appaterno = obj.Appaterno
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("listarjugadoresporequipo/{idequipo}")]
        public IActionResult ListarJugadoresPorEquipo(int idequipo)
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idequipo == idequipo && j.Habilitado == 1
                                select new JugadorListCLS
                                {
                                    idjugador = j.Idjugador,
                                    nombre = j.Nombre,
                                    appaterno = j.Appaterno
                                }).ToList();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ NUEVO: Endpoint para listar goleadores del torneo
        [HttpGet("Goleadores/{idTorneo}")]
        public IActionResult ListarGoleadores(int idTorneo)
        {
            try
            {
                var consulta = (from j in _bd.Jugadors
                                join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                                where j.Idtorneo == idTorneo 
                                   && j.Habilitado == 1
                                   && j.Goles > 0  // ✅ Solo jugadores con goles
                                   && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"  // ✅ Excluir autogoles
                                   && e.Nombre.Trim() != "_SIN EQUIPO"  // ✅ Excluir jugadores sin equipo
                                select new
                                {
                                    j.Idjugador,
                                    NombreCompleto = (j.Nombre.Trim() + " " + j.Appaterno.Trim() + " " + j.Apmaterno.Trim()).Trim(),
                                    j.Goles,
                                    j.Idequipo,
                                    NombreEquipo = e.Nombre
                                })
                                 .OrderByDescending(j => j.Goles)  // ✅ Primero por goles (mayor a menor)
                                 .ThenBy(j => j.NombreEquipo)      // ✅ Desempate: orden alfabético del equipo
                                 .ToList();

                // ✅ Calcular posición considerando empates
                var goleadoresConPosicion = new List<GoleadorCLS>();
                int posicionActual = 1;
                int? golesAnterior = null;

                for (int i = 0; i < consulta.Count; i++)
                {
                    var g = consulta[i];
                    
                    // Si los goles son diferentes al anterior, actualizamos la posición
                    if (golesAnterior.HasValue && g.Goles != golesAnterior.Value)
                    {
                        posicionActual = i + 1;  // La nueva posición es el índice + 1
                    }
                    
                    goleadoresConPosicion.Add(new GoleadorCLS
                    {
                        idjugador = g.Idjugador,
                        nombrecompleto = g.NombreCompleto,
                        goles = g.Goles,
                        idequipo = g.Idequipo,
                        nombreequipo = g.NombreEquipo,
                        posicion = posicionActual
                    });
                    
                    golesAnterior = g.Goles;
                }

                return Ok(goleadoresConPosicion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]JugadorFormCLS oJugadorFormCLS)
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var obJugador = _bd.Jugadors.FirstOrDefault(j => j.Idjugador == id);
                if (obJugador == null)
                {
                    return NotFound();
                }
                obJugador.Habilitado = 0;                
                _bd.SaveChanges();
                return Ok("Se elimino correctamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ NUEVO: Endpoint para obtener equipos del torneo (para el picker)
        [HttpGet("EquiposPorTorneo/{idTorneo}")]
        public IActionResult ListarEquiposPorTorneo(int idTorneo)
        {
            try
            {
                var equipos = (from e in _bd.Equipos
                              where e.Idtorneo == idTorneo 
                                 && e.Habilitado == 1
                                 && e.Nombre.Trim() != "_SIN EQUIPO"
                              orderby e.Nombre
                              select new EquipoSimpleCLS
                              {
                                  idequipo = e.Idequipo,
                                  nombre = e.Nombre
                              }).ToList();

                return Ok(equipos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ NUEVO: Endpoint para jugadores por año de nacimiento
        [HttpGet("JugadoresPorAño/{idTorneo}")]
        public IActionResult ListarJugadoresPorAño(int idTorneo, [FromQuery] int? idEquipo = null)
        {
            try
            {
                var query = from j in _bd.Jugadors
                           join e in _bd.Equipos on j.Idequipo equals e.Idequipo
                           where j.Idtorneo == idTorneo
                              && j.Habilitado == 1
                              && j.Nombre.Trim() != "GOL A FAVOR DEL EQUIPO"
                              && e.Nombre.Trim() != "_SIN EQUIPO"
                              && j.Fnacimiento != null
                           select new { j, e };

                // Filtrar por equipo si se especifica
                if (idEquipo.HasValue && idEquipo.Value > 0)
                {
                    query = query.Where(x => x.j.Idequipo == idEquipo.Value);
                }

                var jugadores = query.ToList();

                // Agrupar por año de nacimiento
                var jugadoresPorAño = jugadores
                    .GroupBy(x => x.j.Fnacimiento.Value.Year)
                    .Select(g => new JugadoresPorAñoCLS
                    {
                        año = g.Key,
                        cantidad = g.Count()
                    })
                    .OrderBy(x => x.año)
                    .ToList();

                return Ok(jugadoresPorAño);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
