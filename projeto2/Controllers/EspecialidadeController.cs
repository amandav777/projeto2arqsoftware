using Microsoft.AspNetCore.Mvc;
using projeto2.Controllers;

namespace Projeto2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadeController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;

        public EspecialidadeController(IAgendamentoService agendamentoService, IPacienteService pacienteService, IMedicoService medicoService)
        {
            _agendamentoService = agendamentoService;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        [HttpPost("/especialidades/{nomeEspecialidade}/agendamento")]
        public async Task<ActionResult<Agendamento>> CriarAgendamentoPorEspecialidade(string nomeEspecialidade, [FromBody] AgendamentoRequest request)
        {
            try
            {
                var paciente = await _pacienteService.ObterPacientePorId(request.IdPaciente);
                if (paciente == null)
                {
                    return NotFound("Paciente não encontrado.");
                }

                var medico = await _medicoService.ObterMedicoAleatorioPorEspecialidade(nomeEspecialidade);
                if (medico == null)
                {
                    return NotFound("Nenhum médico disponível encontrado para essa especialidade.");
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
}
