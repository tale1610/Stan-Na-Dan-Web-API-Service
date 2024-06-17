using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;

namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpoljniSaradnikController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveSpoljneSaradnike")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveSpoljneSaradnike()
        {
            (bool isError, var spoljniSaradnici, ErrorMessage? error) = DataProvider.VratiSveSpoljneSaradnike();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(spoljniSaradnici);
        }

        [HttpGet]
        [Route("VratiSveSpoljneSaradnikeAgenta/{mbrAgenta}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveSpoljneSaradnikeAgenta(string mbrAgenta)
        {
            (bool isError, var spoljniSaradnici, ErrorMessage? error) = DataProvider.VratiSveSpoljneSaradnikeAgenta(mbrAgenta);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(spoljniSaradnici);
        }

        [HttpGet]
        [Route("VratiSpoljnogSaradnika/{mbr}/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSpoljnogSaradnika(string mbr, int id)
        {
            (bool isError, var spoljni, ErrorMessage? error) = DataProvider.VratiSpoljnogSaradnika(mbr, id);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(spoljni);
        }

        [HttpPost]
        [Route("DodajSpoljnogSaradnika/{mbrAgenta}/{idSpoljnog}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajSpoljnogSaradnika([FromBody] SpoljniSaradnikView spoljni, string mbrAgenta, int idSpoljnog)
        {
            var data = DataProvider.DodajNovogSpoljnogSaradnika(spoljni, mbrAgenta, idSpoljnog);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodat novi spoljni saradnik {spoljni.Ime} {spoljni.Prezime} agentu sa maticnim brojem: {mbrAgenta}");
        }


        [HttpPut]
        [Route("IzmeniSpoljnogSaradnika/{mbrAgenta}/{idSpoljnog}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniSpoljnogSaradnika(string mbrAgenta, int idSpoljnog, [FromBody] SpoljniSaradnikView spoljni)
        {
            var data = DataProvider.IzmeniSpoljnogSaradnika(mbrAgenta, idSpoljnog, spoljni);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno izmenjen spoljni saradnik {spoljni.Ime} {spoljni.Prezime}");
        }


        [HttpDelete]
        [Route("ObrisiSpoljnogSaradnika/{mbrAgenta}/{idSpoljnog}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ObrisiSpoljnogSaradnika(string mbrAgenta, int idSpoljnog)
        {
            var data = DataProvider.ObrisiSpoljnogSaradnika(mbrAgenta, idSpoljnog);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(200, $"Uspešno obrisan spoljni saradnik sa id-em {idSpoljnog} agenta sa maticnim brojem: {mbrAgenta}");
        }
    }
}
