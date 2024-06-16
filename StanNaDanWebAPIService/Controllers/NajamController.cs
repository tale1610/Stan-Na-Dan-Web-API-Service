using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NajamController : ControllerBase
    {
        [HttpPost]
        [Route("KreirajNajam/{mbrAgenta}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> KreirajNajam(string mbrAgenta, int idNekretnine, [FromBody] NajamView p)
        {
            var data = await DataProvider.KreirajNajamAsync(p, idNekretnine, mbrAgenta);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno kreiran najam.");
        }

        [HttpPost]
        [Route("DodajNajam/{mbrAgenta}/{idNekretnine}/{idSpoljnogSaradnika}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajNajam(string mbrAgenta, int idNekretnine, [FromBody] NajamView p, int? idSpoljnogSaradnika = null)
        {
            var data = await DataProvider.DodajNajamAsync(p, idNekretnine, mbrAgenta, idSpoljnogSaradnika);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat najam.");
        }

        [HttpGet]
        [Route("VratiSveNajmove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiSveNajmove(int idSobe, int idNekretnine)
        {
            (bool isError, var najmovi, ErrorMessage? error) = await DataProvider.VratiSveNajmoveAsync();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(najmovi);
        }

        [HttpGet]
        [Route("VratiSveNajmoveNekretnine/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiSveNajmoveNekretnine(int idNekretnine)
        {
            (bool isError, var najmovi, ErrorMessage? error) = await DataProvider.VratiSveNajmoveNekretnineAsync(idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(najmovi);
        }

        [HttpGet]
        [Route("VratiNajam/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiNajam(int idNajma)
        {
            (bool isError, var najam, ErrorMessage? error) = await DataProvider.VratiNajamAsync(idNajma);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(najam);
        }

        [HttpPut]
        [Route("IzmeniNajam/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniNajam(int idNajma, NajamView o)
        {
            (bool isError, var najam, ErrorMessage? error) = await DataProvider.IzmeniNajamAsync(idNajma, o);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (najam == null)
            {
                return BadRequest("Najam nije validan.");
            }

            return Ok($"Uspešno ažuriran najam.");
        }

        [HttpDelete]
        [Route("ObrisiNajam/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiNajam(int idNajma)
        {
            var data = await DataProvider.ObrisiNajamAsync(idNajma);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan najam.");
        }
    }
}
