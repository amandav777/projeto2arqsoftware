using System.Threading.Tasks;
using projeto2arqsoftware.Models;
using projeto2arqsoftware.Repositories;

namespace projeto2arqsoftware.Services
{
    public interface IPacienteService
    {
        Task<Paciente> ObterPacientePorId(int idPaciente);
    }

    public class PacienteService : IPacienteService
    {
        private readonly PacienteRepository _pacienteRepository;

        public PacienteService(PacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<Paciente> ObterPacientePorId(int idPaciente)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(idPaciente);
            return paciente;
        }

    }
}
