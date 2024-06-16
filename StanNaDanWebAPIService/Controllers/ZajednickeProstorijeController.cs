using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZajednickeProstorijeController : ControllerBase
    {
        [HttpPost]
        [Route("DodajZajednickuProstoriju/{idSobe}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DodajZajednickuProstoriju(int idSobe,int idNekretnine, [FromBody] ZajednickeProstorijeView p)
        {
            var data = await DataProvider.DodajZajednickuProstorijuAsync(p, idSobe, idNekretnine);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata zajednicka prostorija.");
        }

        [HttpGet]
        [Route("VratiSveZajednickeProstorijeSobe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveZajednickeProstorijeSobe(int idSobe, int idNekretnine)
        {
            (bool isError, var prostorije, ErrorMessage? error) = DataProvider.VratiSveZajednickeProstorijeSobe(idSobe, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(prostorije);
        }

        [HttpGet]
        [Route("VratiSveZajednickeProstorijeNajma/{idNajma}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveZajednickeProstorijeNajma(int idNajma)
        {
            (bool isError, var prostorije, ErrorMessage? error) = DataProvider.VratiSveZajednickeProstorijeNajma(idNajma);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(prostorije);
        }

        [HttpGet]
        [Route("VratiZajednickuProstoriju/{idSobe}/{idNekretnine}/{zajednickaProstorija}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> VratiZajednickuProstoriju(int idSobe, int idNekretnine, string zajednickaProstorija)
        {
            (bool isError, var prostorija, ErrorMessage? error) = await DataProvider.VratiZajednickuProstorijuAsync(idSobe, idNekretnine, zajednickaProstorija);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(prostorija);
        }

        [HttpPut]
        [Route("IzmeniZajednickuProstoriju/{idSobe}/{idNekretnine}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> IzmeniZajednickuProstoriju(int idSobe, int idNekretnine, ZajednickeProstorijeView o)
        {
            (bool isError, var prostorija, ErrorMessage? error) = await DataProvider.IzmeniZajednickuProstorijuAsync(o, idSobe, idNekretnine);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (prostorija == null)
            {
                return BadRequest("Zajednicka prostorija nije validna.");
            }

            return Ok($"Uspešno ažuriran zajednicke prostorije.");
        }

        [HttpDelete]
        [Route("ObrisiZajednickuProstoriju/{idSobe}/{idNekretnine}/{zajednickaProstorija}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObrisiZajednickuProstoriju(int idSobe, int idNekretnine, string zajednickaProstorija)
        {
            var data = await DataProvider.ObrisiZajednickuProstorijuAsync(idSobe, idNekretnine, zajednickaProstorija);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisana zajednicka prostorija.");
        }
    }
}
