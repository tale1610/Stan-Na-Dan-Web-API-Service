using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SobaController : ControllerBase
    {
        [HttpPost]
        [Route("DodajSobu/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajSobu(int idNekretnine, [FromBody] SobaView p)
        {
            var data = await DataProvider.DodajSobuAsync(p, idNekretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata soba.");
        }

        [HttpGet]
        [Route("VratiSveSobe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveTelefoneKontaktOsobe()
        {
            (bool isError, var sobe, ErrorMessage? error) = DataProvider.VratiSveSobe();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sobe);
        }

        [HttpGet]
        [Route("VratiSveSobeNekretnine/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveSobeNekretnine(int idNekretnine)
        {
            (bool isError, var sobe, ErrorMessage? error) = DataProvider.VratiSveSobeNekretnine(idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sobe);
        }
        
        [HttpGet]
        [Route("VratiSobu/{idSobe}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiSobu(int idSobe, int idNekretnine)
        {
            (bool isError, var soba, ErrorMessage? error) = await DataProvider.VratiSobuAsync(idSobe, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(soba);
        }

        [HttpDelete]
        [Route("ObrisiSobu/{idSobe}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiSobu(int idSobe, int idNekretnine)
        {
            var data = await DataProvider.ObrisiSobuAsync(idSobe, idNekretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana soba.");
        }
    }
}
