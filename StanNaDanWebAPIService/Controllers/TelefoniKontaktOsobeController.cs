using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelefoniKontaktOsobeController : ControllerBase
    {
        [HttpPost]
        [Route("DodajTelefonKontaktOsobe/{pib}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajTelefonKontaktOsobe(string pib, [FromBody] TelefoniKontaktOsobeView p)
        {
            var data = await DataProvider.DodajTelefonKontaktOsobeAsync(p, pib);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat telefon kontakt osobe.");
        }

        [HttpGet]
        [Route("VratiSveTelefoneKontaktOsobe/{pib}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveTelefoneKontaktOsobe(string pib)
        {
            (bool isError, var telefoni, ErrorMessage? error) = DataProvider.VratiSveTelefoneKontaktOsobe(pib);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(telefoni);
        }

        [HttpGet]
        [Route("VratiTelefonKontaktOsobe/{brojTelefona}/{pib}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiTelefonKontaktOsobe(string brojTelefona, string pib)
        {
            (bool isError, var telefon, ErrorMessage? error) = await DataProvider.VratiTelefonKontaktOsobeAsync(brojTelefona, pib);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(telefon);
        }

        [HttpPut]
        [Route("IzmeniTelefonKontaktOsobe/{pib}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniTelefonKontaktOsobe(string pib, TelefoniKontaktOsobeView o)
        {
            (bool isError, var telefon, ErrorMessage? error) = await DataProvider.IzmeniTelefonKontaktOsobeAsync(o, pib);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (telefon == null)
            {
                return BadRequest("Telefon nije validan.");
            }

            return Ok($"Uspešno ažuriran telefon.");
        }

        [HttpDelete]
        [Route("ObrisiTelefonKontaktOsobe/{brojTelefona}/{pib}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiTelefonKontaktOsobe(string brojTelefona, string pib)
        {
            var data = await DataProvider.ObrisiTelefonKontaktOsobeAsync(brojTelefona, pib);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan telefon kontakt osobe.");
        }
    }
}
