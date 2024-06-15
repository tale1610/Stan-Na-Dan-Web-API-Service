using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZaposleniController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveZaposlene")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveAgente()
        {
            (bool isError, var zaposleni, ErrorMessage? error) = DataProvider.VratiSveZaposlene();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(zaposleni);
        }

        [HttpGet]
        [Route("VratiZaposlenogAsync/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiZaposlenog(string mbr)
        {
            (bool isError, var kvartovi, ErrorMessage? error) = await DataProvider.VratiZaposlenogAsync(mbr);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(kvartovi);
        }

        [HttpDelete]
        [Route("ObrisiZaposlenog/{mbr}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiZaposlenog(string mbr)
        {
            var data = await DataProvider.ObrisiZaposlenogAsync(mbr);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan zaposleni. MBR: {mbr}");
        }
    }
}
