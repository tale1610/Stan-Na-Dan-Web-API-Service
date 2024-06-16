using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FizickoLiceController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSvaFizickaLica")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSvaFizickaLica()
        {
            (bool isError, var lica, ErrorMessage? error) = DataProvider.VratiSvaFizickaLica();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(lica);
        }

        [HttpPost]
        [Route("DodajNovoFizickoLice")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajNovoFizickoLice([FromBody] FizickoLiceView p)
        {
            var data = DataProvider.DodajNovoFizickoLice(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodato fizicko lice. JMBG: {p.JMBG}");
        }

        [HttpGet]
        [Route("VratiFizickoLice/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiFizickoLice(string jmbg)
        {
            (bool isError, var lice, ErrorMessage? error) = DataProvider.VratiFizickoLice(jmbg);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(lice);
        }

        [HttpPut]
        [Route("IzmeniFizickoLice/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniFizickoLice(string jmbg, [FromBody] FizickoLiceView p)
        {
            (bool isError, var lice, ErrorMessage? error) = DataProvider.IzmeniFizickoLice(jmbg, p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (lice == null)
            {
                return BadRequest("Fizicko lice nije validno.");
            }

            return Ok($"Uspešno azurirano fizicko lice.");
        }

        [HttpDelete]
        [Route("ObrisiFizickoLice/{jmbg}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiFizickoLice(string jmbg)
        {
            var data = DataProvider.ObrisiFizickoLice(jmbg);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisano fizicko lice. JMBG: {jmbg}");
        }
    }
}
