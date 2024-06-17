using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {

        [HttpPost]
        [Route("DodajParking/{idNekretnine}/{idParkinga}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajParking(int idNekretnine, int idParkinga, [FromBody] ParkingView p)
        {
            var data = await DataProvider.DodajParkingAsync(p, idNekretnine, idParkinga);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat parking.");
        }

        [HttpGet]
        [Route("VratiSveParkingeNekretnine/{idNekretnina}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveParkingeNekretnine(int idNekretnina)
        {
            (bool isError, var parkinzi, ErrorMessage? error) = DataProvider.VratiSveParkingeNekretnine(idNekretnina);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(parkinzi);
        }

        [HttpGet]
        [Route("VratiParking/{idNekretnine}/{idParkinga}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult>VratiParking(int idParkinga, int idNekretnine)
        {
            (bool isError, var parking, ErrorMessage? error) = await DataProvider.VratiParkingAsync(idParkinga, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(parking);
        }

        [HttpPut]
        [Route("IzmeniParking/{idNekretnine}/{idParkinga}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniParking(int idNekretnine, int idParkinga, ParkingView o)
        {
            (bool isError, var parking, ErrorMessage? error) = await DataProvider.IzmeniParkingAsync(o, idNekretnine, idParkinga);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (parking == false)
            {
                return BadRequest("Parking nije validan.");
            }

            return Ok($"Uspešno ažuriran parking.");
        }

        [HttpDelete]
        [Route("ObrisiParking/{idNretnine}/{idParkinga}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiParking(int idParkinga, int idNretnine)
        {
            var data = await DataProvider.ObrisiParkingAsync(idParkinga, idNretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan parking.");
        }
    }
}
