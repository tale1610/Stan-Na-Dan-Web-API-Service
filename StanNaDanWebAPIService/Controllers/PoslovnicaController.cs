using Microsoft.AspNetCore.Mvc;


namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoslovnicaController : ControllerBase
    {
        [HttpGet("VratiSvePoslovnice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> VratiSvePoslovnice()
        {
            var (isError, odeljenja, error) = await DataProvider.VratiSvaOdeljenjaAsync();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(odeljenja);
        }
    }
}
