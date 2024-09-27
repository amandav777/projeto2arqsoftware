using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Projeto2.Models;
using Projeto2.Services;
using projeto2.Controllers;

namespace Projeto2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet("{idPaciente}")]
        public async Task<ActionResult<Paciente>> ObterPacientePorId(int idPaciente)
        {
            var paciente = await _pacienteService.ObterPacientePorId(idPaciente);

            if (paciente == null)
            {
                return NotFound("Paciente não encontrado.");
            }

            return Ok(paciente);
        }
    }
}
