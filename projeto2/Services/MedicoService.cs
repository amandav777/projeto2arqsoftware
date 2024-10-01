using System.Threading.Tasks;
using projeto2arqsoftware.Models;
using projeto2arqsoftware.Repositories;

namespace projeto2arqsoftware.Services
{
    public interface IMedicoService
    {
        Task<Medico> ObterMedicoPorId(int idMedico);

    }

    public class MedicoService : IMedicoService
    {
        private readonly MedicoRepository _medicoRepository;

        public MedicoService(MedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        public async Task<Medico> ObterMedicoPorId(int idMedico)
        {

            var medico = await _medicoRepository.GetByIdAsync(idMedico);
            return medico;
        }

        
    }
}
