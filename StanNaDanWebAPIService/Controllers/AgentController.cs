using Microsoft.AspNetCore.Mvc;
using StanNaDanLibrary;


namespace StanNaDanWebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentController : ControllerBase
    {
        [HttpGet]
        [Route("VratiSveAgente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiSveAgente()
        {
            (bool isError, var agenti, ErrorMessage? error) = DataProvider.VratiSveAgente();

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(agenti);
        }

        [HttpGet]
        [Route("VratiAgenta/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult VratiAgenta(string mbr)
        {
            (bool isError, var agent, ErrorMessage? error) = DataProvider.VratiAgenta(mbr);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            return Ok(agent);
        }


        [HttpPost]
        [Route("DodajAgenta/{idPoslovnice}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DodajAgenta(int idPoslovnice, [FromBody] AgentView agent)
        {
            var data = DataProvider.DodajNovogAgenta(idPoslovnice, agent);

            if (data.IsError)
            {
                return StatusCode(data.Error.StatusCode, data.Error.Message);
            }

            return StatusCode(201, $"Uspešno dodata novi agent {agent.Ime} {agent.Prezime}");
        }


        [HttpPut]
        [Route("IzmeniAgenta/{mbr}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult IzmeniAgenta(string mbr, [FromBody] AgentView agentView)
        {
            (bool isError, var agent, ErrorMessage? error) = DataProvider.IzmeniAgenta(mbr, agentView);

            if (isError)
            {
                return StatusCode(error?.StatusCode ?? 400, error?.Message);
            }

            if (agent == null)
            {
                return BadRequest("Agent nije validan.");
            }

            return Ok($"Uspešno promenjen agent: {agent.Ime} {agent.Prezime}");
        }

    }
}
