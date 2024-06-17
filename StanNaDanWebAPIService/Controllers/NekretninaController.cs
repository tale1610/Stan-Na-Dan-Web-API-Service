using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NekretninaController : ControllerBase
    {
        [HttpPost]
        [Route("DodajKucu/{idKvarta}/{idVlasnika}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajStan(int idKvarta, int idVlasnika, [FromBody] KucaView k)
        {
            var data = DataProvider.DodajNekretninu(idKvarta, idVlasnika, k, null);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata nekretnina.");
        }

        [HttpPost]
        [Route("DodajStan/{idKvarta}/{idVlasnika}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajStan(int idKvarta, int idVlasnika, [FromBody] StanView s)
        {
            var data = DataProvider.DodajNekretninu(idKvarta, idVlasnika, null, s);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata nekretnina.");
        }

        [HttpGet]
        [Route("VratiSveNekretnine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveNekretnine()
        {
            (bool isError, var pravnici, ErrorMessage? error) = DataProvider.VratiSveNekretnine();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(pravnici);
        }


        [HttpPut]
        [Route("IzmeniStan/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniSefa([FromBody] StanView p, int idNekretnine)
        {
            (bool isError, var stan, ErrorMessage? error) = DataProvider.IzmeniStan(p, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (stan == null)
            {
                return BadRequest("Stan nije validan.");
            }

            return Ok($"Uspešno ažuriran stan.");
        }

        [HttpPut]
        [Route("IzmeniKucu/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniKucu(int idNekretnine, [FromBody] KucaView p)
        {
            (bool isError, var kuca, ErrorMessage? error) = DataProvider.IzmeniKucu(p, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (kuca == null)
            {
                return BadRequest("Kuca nije validan.");
            }

            return Ok($"Uspešno ažuriran kuca.");
        }

        [HttpDelete]
        [Route("ObrisiNekretninu/{idNretnine}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiNekretninu(int idNretnine)
        {
            var data = DataProvider.ObrisiNekretninu(idNretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana nekretnina.");
        }

        [HttpGet]
        [Route("VratiNekretninu/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiNekretninu(int idNekretnine)
        {
            (bool isError, var nekretnina, ErrorMessage? error) = DataProvider.VratiNekretninu(idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(nekretnina);
        }

        [HttpGet]
        [Route("VratiSveNekretnineKvarta/{idKvarta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveNekretnineKvarta(int idKvarta)
        {
            (bool isError, var nekretnine, ErrorMessage? error) = DataProvider.VratiSveNekretnineKvarta(idKvarta);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(nekretnine);
        }
        
        [HttpGet]
        [Route("VratiSveNekretnineVlasnika/{idVlasnika}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveNekretnineVlasnika(int idVlasnika)
        {
            (bool isError, var nekretnine, ErrorMessage? error) = DataProvider.VratiSveNekretnineVlasnika(idVlasnika);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(nekretnine);
        }
    }
}
