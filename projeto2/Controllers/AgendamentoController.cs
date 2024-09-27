using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Projeto2.Models;
using Projeto2.Services;
using projeto2.Controllers;

namespace Projeto2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public AgendamentoController(IAgendamentoService agendamentoService, IPacienteService pacienteService, IMedicoService medicoService)
        {
            _agendamentoService = agendamentoService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        [HttpPost("/medicos/{idMedico}/agendamento")]
        public async Task<ActionResult<Agendamento>> CriarAgendamento(int idMedico, [FromBody] AgendamentoRequest request)
        {
            try
            {
                var medico = await _medicoService.ObterMedicoPorId(idMedico);
                var paciente = await _pacienteService.ObterPacientePorId(request.IdPaciente);

                if (medico == null || paciente == null)
                {
                    return NotFound("Médico ou paciente não encontrado.");
                }

                var agendamento = await _agendamentoService.CriarAgendamento(medico, paciente, request.DataConsulta);
                return Ok(agendamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class AgendamentoRequest
    {
        public int IdPaciente { get; set; }
        public DateTime DataConsulta { get; set; }
    }
}
