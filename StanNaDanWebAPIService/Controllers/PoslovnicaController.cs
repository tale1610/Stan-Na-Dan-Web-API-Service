using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;


namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoslovnicaController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSvePoslovnice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSvePoslovnice()
        {
            (bool isError, var poslovnice, ErrorMessage? error) = DataProvider.VratiSvePoslovnice();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(poslovnice);
        }

        [HttpPost]
        [Route("DodajPoslovnicu")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajPoslovnicu([FromBody] PoslovnicaView p)
        {
            var data = await DataProvider.DodajPoslovnicuAsync(p);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata poslovnica na adresi: {p.Adresa}");
        }

        [HttpPut]
        [Route("IzmeniPoslovnicu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniPoslovnicu(int id, [FromBody] PoslovnicaView p)
        {
            (bool isError, var poslovnica, ErrorMessage? error) = await DataProvider.IzmeniPoslovnicuAsync(id, p);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (poslovnica == null)
            {
                return BadRequest("Prodavnica nije validna.");
            }

            return Ok($"Uspešno promenjena poslovnica na adresi: {poslovnica.Adresa}");
        }


        [HttpDelete]
        [Route("ObrisiPoslovnicu/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiPoslovnicu(int id)
        {
            var data = await DataProvider.ObrisiPoslovnicuAsync(id);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana prodavnica. ID: {id}");
        }


        [HttpGet]
        [Route("VratiPoslovnicu/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiPoslovnicu(int id)
        {
            (bool isError, var poslovnica, ErrorMessage? error) = await DataProvider.VratiPoslovnicuAsync(id);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(poslovnica);
        }




    }

   

}
