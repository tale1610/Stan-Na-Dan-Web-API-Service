using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KvartController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveKvartove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveKvartove()
        {
            (bool isError, var kvartovi, ErrorMessage? error) = DataProvider.VratiSveKvartove();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(kvartovi);
        }

        [HttpGet]
        [Route("VratiSveKvartovePoslovnice/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveKvartovePoslovnice(int id)
        {
            (bool isError, var kvartovi, ErrorMessage? error) = DataProvider.VratiSveKvartovePoslovnice(id);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(kvartovi);
        }

        [HttpGet]
        [Route("VratiKvart/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiKvart(int id)
        {
            (bool isError, var kvart, ErrorMessage? error) = DataProvider.VratiKvart(id);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(kvart);
        }

        [HttpPost]
        [Route("DodajNoviKvart/{idPoslovnice}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajNoviKvart(int idPoslovnice, [FromBody] KvartView p)
        {
            var data = DataProvider.DodajNoviKvart(idPoslovnice,p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat kvart. ID: {p.IdKvarta}");
        }

        [HttpPut]
        [Route("IzmeniKvart/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniKvart(int id, [FromBody] KvartView p)
        {
            (bool isError, var kvart, ErrorMessage? error) =  DataProvider.IzmeniKvart(id, p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (kvart == null)
            {
                return BadRequest("Kvart nije validan.");
            }

            return Ok($"Uspešno ažuriran kvart.");
        }

        [HttpDelete]
        [Route("ObrisiKvart/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiKvart(int id)
        {
            var data =  DataProvider.ObrisiKvart(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan kvart. ID: {id}");
        }
    }
}
