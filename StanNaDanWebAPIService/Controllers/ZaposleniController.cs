using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZaposleniController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveAgente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveAgente()
        {
            (bool isError, var zaposleni, ErrorMessage? error) = DataProvider.VratiSveAgente();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(zaposleni);
        }

        [HttpGet]
        [Route("VratiAgenta/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiAgenta(string mbr)
        {
            (bool isError, var agent, ErrorMessage? error) = DataProvider.VratiAgenta(mbr);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(agent);
        }


    }
}
