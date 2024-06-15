using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SefController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveSefove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveSefove()
        {
            (bool isError, var sef, ErrorMessage? error) = DataProvider.VratiSveSefove();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sef);
        }

        [HttpGet]
        [Route("VratiSefove/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSefa(string mbr)
        {
            (bool isError, var sef, ErrorMessage? error) =  DataProvider.VratiSefa(mbr);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(sef);
        }

        [HttpPost]
        [Route("DodajNovogSefa/{idPoslovnice}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajNoviKvart(int idPoslovnice, [FromBody] SefView p)
        {
            var data = DataProvider.DodajNovogSefa(idPoslovnice, p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat kvart. ID: {p.MBR}");
        }

        [HttpPut]
        [Route("IzmeniSefa/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniSefa(string mbr, [FromBody] SefView p)
        {
            (bool isError, var kvart, ErrorMessage? error) = DataProvider.IzmeniSefa(mbr, p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (kvart == null)
            {
                return BadRequest("Sef nije validan.");
            }

            return Ok($"Uspešno ažuriran sef.");
        }
    }
}
