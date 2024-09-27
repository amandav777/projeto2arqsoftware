using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Projeto2.Models;
using Projeto2.Services;
using projeto2.Controllers;

namespace Projeto2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet("{idMedico}")]
        public async Task<ActionResult<Medico>> ObterMedico(int idMedico)
        {
            var medico = await _medicoService.ObterMedicoPorId(idMedico);
            if (medico == null)
            {
                return NotFound("Médico não encontrado.");
            }
            return Ok(medico);
        }
    }
}
