using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SajtoviNekretnineController : ControllerBase
    {
        [HttpPost]
        [Route("DodajSajtNekretnine/{idNekretnine}/{imeSajta}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajSajtNekretnine(int idNekretnine,  string imeSajta)
        {
            var data = await DataProvider.DodajSajtNekretnineAsync(idNekretnine, imeSajta);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat sajt.");
        }

        [HttpGet]
        [Route("VratiSveSajtoveNekretnine/{idNekretnina}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveSajtoveNekretnine(int idNekretnina)
        {
            (bool isError, var sajtovi, ErrorMessage? error) = DataProvider.VratiSveSajtoveNekretnine(idNekretnina);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sajtovi);
        }

        [HttpGet]
        [Route("VratiSajtNekretnine/{sajtNekretnine}/{idNekretnina}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiSajtNekretnine(string sajtNekretnine, int idNekretnina)
        {
            (bool isError, var sajt, ErrorMessage? error) = await DataProvider.VratiSajtNekretnineAsync(sajtNekretnine, idNekretnina);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sajt);
        }

        [HttpPut]
        [Route("IzmeniSajtNekretnine/{stariSajt}/{noviSajt}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniSajtNekretnine(string stariSajt, int idNekretnine, string noviSajt)
        {
            (bool isError, var sajt, ErrorMessage? error) = await DataProvider.IzmeniSajtNekretnineAsync(stariSajt, idNekretnine, noviSajt);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (sajt == null)
            {
                return BadRequest("Sajt nije validan.");
            }

            return Ok($"Uspešno ažuriran sajt.");
        }

        [HttpDelete]
        [Route("ObrisiSajtNekretnine/{sajt}/{idNretnine}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiSajtNekretnine(string sajt, int idNretnine)
        {
            var data = await DataProvider.ObrisiSajtNekretnineAsync(sajt, idNretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan sajt.");
        }
    }
}
