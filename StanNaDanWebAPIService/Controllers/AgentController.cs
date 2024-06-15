using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;


namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveAgente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveAgente()
        {
            (bool isError, var agenti, ErrorMessage? error) = DataProvider.VratiSveAgente();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(agenti);
        }
    }
}
