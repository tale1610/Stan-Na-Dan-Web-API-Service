using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VlasnikController : ControllerBase
    {
        [HttpDelete]
        [Route("ObrisiVlasnika")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiVlasnika([FromQuery] string? jmbg = null, [FromQuery] string? pib = null)
        {
            if (string.IsNullOrEmpty(jmbg) && string.IsNullOrEmpty(pib))
            {
                return BadRequest("Morate uneti JMBG ili PIB.");
            }
            var data = DataProvider.ObrisiVlasnika(jmbg, pib);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan vlasnik. JMBG: {jmbg}");
        }
    }
}
