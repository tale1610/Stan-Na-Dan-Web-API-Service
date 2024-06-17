using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BrojeviTelefonaController : ControllerBase
    {
        [HttpPost]
        [Route("DodajBrojTelefona/{jmbg}/{brTelefona}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajBrojTelefona(string jmbg, string brTelefona)
        {
            var data = await DataProvider.DodajBrojTelefonaAsync(brTelefona, jmbg);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat broj telefona.");
        }

        [HttpGet]
        [Route("VratiSveBrojeveTelefona/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveBrojeveTelefona(string jmbg)
        {
            (bool isError, var brojevi, ErrorMessage? error) = DataProvider.VratiSveBrojeveTelefona(jmbg);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(brojevi);
        }

        [HttpGet]
        [Route("VratiBrojTelefona/{brojTelefona}/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiBrojTelefona(string brojTelefona, string jmbg)
        {
            (bool isError, var broj, ErrorMessage? error) = await DataProvider.VratiBrojTelefonaAsync(brojTelefona, jmbg);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(broj);
        }

        [HttpPut]
        [Route("IzmeniBrojTelefona/{stariBroj}/{noviBroj}/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniBrojTelefona(string jmbg, string stariBroj, string noviBroj)
        {
            (bool isError, var broj, ErrorMessage? error) = await DataProvider.IzmeniBrojTelefonaAsync(stariBroj, noviBroj, jmbg);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (broj == null)
            {
                return BadRequest("broj nije validan.");
            }

            return Ok($"Uspešno ažuriran broja.");
        }

        [HttpDelete]
        [Route("ObrisiBrojTelefona/{brojTelefona}/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiBrojTelefona(string brojTelefona, string jmbg)
        {
            var data = await DataProvider.ObrisiBrojTelefonaAsync(brojTelefona, jmbg);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan broj telefona.");
        }
    }
}
