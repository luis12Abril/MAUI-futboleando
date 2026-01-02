using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace futboleandoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        public CiudadController()
        {


        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok("Ciudad Controller funcionando");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }





    }
}
