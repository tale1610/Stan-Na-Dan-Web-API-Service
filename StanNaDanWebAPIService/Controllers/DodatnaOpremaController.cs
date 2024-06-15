using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;
using StanNaDanLibrary.Entiteti;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DodatnaOpremaController : ControllerBase
    {
        [HttpPost]
        [Route("DodajDodatnuOpremu/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajDodatnuOpremu(int idNekretnine, [FromBody] DodatnaOpremaView n)
        {
            var data = DataProvider.DodajDodatnuOpremu(n, idNekretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata dodatna oprema.");
        }

        [HttpGet]
        [Route("VratiSvuDodatnuOpremuNekretnine/{idNekretnina}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSvuDodatnuOpremuNekretnine(int idNekretnina)
        {
            (bool isError, var dodatnaOprema, ErrorMessage? error) = DataProvider.VratiSvuDodatnuOpremuNekretnine(idNekretnina);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(dodatnaOprema);
        }
        
        [HttpGet]
        [Route("VratiDodatnuOpremu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiDodatnuOpremu([FromRoute] DodatnaOpremaId id)
        {
            (bool isError, var dodatnaOprema, ErrorMessage? error) = DataProvider.VratiDodatnuOpremu(id);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(dodatnaOprema);
        }


        [HttpPut]
        [Route("IzmeniDodatnuOpremu/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniDodatnuOpremu(int idNekretnine, DodatnaOpremaView o)
        {
            (bool isError, var dodatnaOprema, ErrorMessage? error) = DataProvider.IzmeniDodatnuOpremu(o, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (dodatnaOprema == null)
            {
                return BadRequest("Dodatna oprema nije validna.");
            }

            return Ok($"Uspešno ažurirana dodatna oprema.");
        }

        [HttpDelete]
        [Route("ObrisiDodatnuOpremu/{idOpreme}/{idNretnine}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiDodatnuOpremu(int idOpreme, int idNretnine)
        {
            var data = DataProvider.ObrisiDodatnuOpremu(idOpreme, idNretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana dodatna oprema.");
        }


    }
}
