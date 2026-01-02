using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        public ColaboradorController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Colaborador Controller funcionando");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }   
    }
}
