using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PravnoLiceController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSvaPravnaLica")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSvaPravnaLica()
        {
            (bool isError, var pravnici, ErrorMessage? error) = DataProvider.VratiSvaPravnaLica();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(pravnici);
        }

        [HttpPost]
        [Route("DodajNovoPravnoLice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajNovoPravnoLice([FromBody] PravnoLiceView p)
        {
            var data = DataProvider.DodajNovoPravnoLice(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodato pravno lice.");
        }

        [HttpGet]
        [Route("VratiPravnoLice/{pib}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiPravnoLice(string pib)
        {
            (bool isError, var pravnoLice, ErrorMessage? error) = DataProvider.VratiPravnoLice(pib);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(pravnoLice);
        }

        [HttpPut]
        [Route("IzmeniPravnoLice/{pib}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniSefa(string mbr, [FromBody] PravnoLiceView p)
        {
            (bool isError, var pravnoLice, ErrorMessage? error) = DataProvider.IzmeniPravnoLice(mbr, p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (pravnoLice == null)
            {
                return BadRequest("Sef nije validan.");
            }

            return Ok($"Uspešno ažuriran sef.");
        }

        [HttpDelete]
        [Route("ObrisiPravnoLice/{pib}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiPravnoLice(string pib)
        {
            var data = DataProvider.ObrisiPravnoLice(pib);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisano pravno lice.");
        }
    }
}
