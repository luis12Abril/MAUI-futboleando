using futboleandoAPIS.Models;
using futboleandoEntities.Juego;
using futboleandoEntities.Jornada;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JuegoController : ControllerBase
    {
        private readonly DbA85d0bFutboleandobdContext _bd;

        public JuegoController(DbA85d0bFutboleandobdContext bd)
        {
            _bd = bd;
        }

        // ? Listar juegos por torneo
        [HttpGet("PorTorneo/{idTorneo}")]
        public IActionResult GetJuegosPorTorneo(int idTorneo)
        {
            try
            {
                var juegos = (from j in _bd.Juegos
                             join jor in _bd.Jornada on j.Idjornada equals jor.Idjornada
                             join e1 in _bd.Equipos on j.Idequipo01 equals e1.Idequipo
                             join e2 in _bd.Equipos on j.Idequipo02 equals e2.Idequipo
                             join c in _bd.Campos on j.Idcampo equals c.Idcampo into campoGroup
                             from campo in campoGroup.DefaultIfEmpty()
                             join est in _bd.Estatusjuegos on j.Idestatusjuego equals est.Idestatusjuego into estatusGroup
                             from estatus in estatusGroup.DefaultIfEmpty()
                             where j.Idtorneo == idTorneo && j.Habilitado == 1
                             orderby jor.Idjornada descending, j.Fhorario descending
                             select new JuegoListCLS
                             {
                                 idjuego = j.Idjuego,
                                 idjornada = j.Idjornada ?? 0,
                                 nombrejornada = jor.Nombre ?? "",
                                 idequipo01 = j.Idequipo01 ?? 0,
                                 nombreequipo01 = e1.Nombre ?? "",
                                 golesequipo01 = j.Golesequipo01,
                                 idequipo02 = j.Idequipo02 ?? 0,
                                 nombreequipo02 = e2.Nombre ?? "",
                                 golesequipo02 = j.Golesequipo02,
                                 fhorario = j.Fhorario,
                                 idcampo = j.Idcampo,
                                 nombrecampo = campo != null ? campo.Nombre : "Sin asignar",
                                 idestatusjuego = j.Idestatusjuego,
                                 nombreestatusjuego = estatus != null ? estatus.Nombre : "Sin estatus",
                                 idtorneo = j.Idtorneo ?? 0
                             }).ToList();

                return Ok(juegos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ? Listar juegos por torneo y jornada
        [HttpGet("PorTorneoYJornada/{idTorneo}/{idJornada}")]
        public IActionResult GetJuegosPorTorneoYJornada(int idTorneo, int idJornada)
        {
            try
            {
                var juegos = (from j in _bd.Juegos
                             join jor in _bd.Jornada on j.Idjornada equals jor.Idjornada
                             join e1 in _bd.Equipos on j.Idequipo01 equals e1.Idequipo
                             join e2 in _bd.Equipos on j.Idequipo02 equals e2.Idequipo
                             join c in _bd.Campos on j.Idcampo equals c.Idcampo into campoGroup
                             from campo in campoGroup.DefaultIfEmpty()
                             join est in _bd.Estatusjuegos on j.Idestatusjuego equals est.Idestatusjuego into estatusGroup
                             from estatus in estatusGroup.DefaultIfEmpty()
                             where j.Idtorneo == idTorneo && j.Idjornada == idJornada && j.Habilitado == 1
                             orderby j.Fhorario
                             select new JuegoListCLS
                             {
                                 idjuego = j.Idjuego,
                                 idjornada = j.Idjornada ?? 0,
                                 nombrejornada = jor.Nombre ?? "",
                                 idequipo01 = j.Idequipo01 ?? 0,
                                 nombreequipo01 = e1.Nombre ?? "",
                                 golesequipo01 = j.Golesequipo01,
                                 idequipo02 = j.Idequipo02 ?? 0,
                                 nombreequipo02 = e2.Nombre ?? "",
                                 golesequipo02 = j.Golesequipo02,
                                 fhorario = j.Fhorario,
                                 idcampo = j.Idcampo,
                                 nombrecampo = campo != null ? campo.Nombre : "Sin asignar",
                                 idestatusjuego = j.Idestatusjuego,
                                 nombreestatusjuego = estatus != null ? estatus.Nombre : "Sin estatus",
                                 idtorneo = j.Idtorneo ?? 0
                             }).ToList();

                return Ok(juegos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ? Listar jornadas por torneo
        [HttpGet("JornadasPorTorneo/{idTorneo}")]
        public IActionResult GetJornadasPorTorneo(int idTorneo)
        {
            try
            {
                var jornadas = (from jor in _bd.Jornada
                               where jor.Idtorneo == idTorneo && jor.Habilitado == 1
                               orderby jor.Idjornada descending
                               select new JornadaListCLS
                               {
                                   idjornada = jor.Idjornada,
                                   nombre = jor.Nombre ?? "",
                                   idtorneo = jor.Idtorneo ?? 0
                               }).ToList();

                return Ok(jornadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
