using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IznajmljenaSobaController : ControllerBase
    {
        [HttpPost]
        [Route("DodajIznajmljenuSobu/{mbrAgenta}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajIznajmljenuSobu(string mbrAgenta, int idNekretnine, [FromQuery] List<int> idSoba, [FromBody] IznajmljenaSobaView iznajmljenaSobaView, [FromQuery] int? idSpoljnog = null)
        {
            if (iznajmljenaSobaView == null || idSoba == null || !idSoba.Any())
            {
                return BadRequest("Podaci nisu dobro uneti.");
            }

            var data = await DataProvider.DodajIznajmljenuSobuAsync(iznajmljenaSobaView,idNekretnine,idSoba,mbrAgenta,idSpoljnog);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, "Uspešno dodata iznajmljena soba.");
        }



        [HttpGet]
        [Route("VratiSveIznajmljeneSobe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiSveIznajmljeneSobe()
        {
            (bool isError, var sobe, ErrorMessage? error) = await DataProvider.VratiSveIznajmljeneSobeAsync();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sobe);
        }

        [HttpGet]
        [Route("VratiIznajmljenuSobu/{idSobe}/{idNekretnine}/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiIznajmljenuSobu(int idSobe, int idNekretnine, int idNajma)
        {
            (bool isError, var soba, ErrorMessage? error) = await DataProvider.VratiIznajmljenuSobuAsync(idSobe, idNekretnine, idNajma);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(soba);
        }

        [HttpDelete]
        [Route("ObrisiIznajmljenuSobu/{idSobe}/{idNekretnine}/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiIznajmljenuSobu(int idSobe, int idNekretnine, int idNajma)
        {
            var data = await DataProvider.ObrisiIznajmljenuSobuAsync(idSobe, idNekretnine, idNajma);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana iznajmljena soba.");
        }
    }
}
