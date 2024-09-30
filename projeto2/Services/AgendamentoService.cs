using System;
using System.Threading.Tasks;
using projeto2arqsoftware.DTOs;
using projeto2arqsoftware.Models;
using projeto2arqsoftware.Repositories;
using projeto2arqsoftware.Services;

namespace projeto2arqsoftware.Services
{
    public interface IAgendamentoService
    {
        Task<Agendamento> CriarAgendamento(Medico medico, Paciente paciente, DateTime dataConsulta);
    }

    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _context;
        private readonly IPacienteService _pacienteService;
        private readonly IMedicoService _medicoService;
        private readonly AgendamentoRepository _agendamentoRepository;
        private readonly DisponibilidadeService _disponibilidadeService;
        private readonly EmailService _emailService;

        public AgendamentoService(
            AppDbContext context,
            IPacienteService pacienteService,
            IMedicoService medicoService,
            AgendamentoRepository agendamentoRepository,
            DisponibilidadeService disponibilidadeService,
            EmailService emailService)
        {
            _context = context;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
            _agendamentoRepository = agendamentoRepository;
            _disponibilidadeService = disponibilidadeService;
            _emailService = emailService;
        }

        public async Task<Agendamento> CriarAgendamento(Medico medico, Paciente paciente, DateTime dataConsulta)
        {
            if (!paciente.Ativo)
            {
                throw new Exception("Paciente não está ativo.");
            }

            bool medicoDisponivel = await _disponibilidadeService.VerificaDisponibilidade(medico.Id, dataConsulta);
            if (!medicoDisponivel)
            {
                throw new Exception("Médico indisponível para a data especificada.");
            }

            var agendamento = new Agendamento
            {
                Medico = medico,
                Paciente = paciente,
                Data = dataConsulta,
                Status = true
            };

            agendamento = await _agendamentoRepository.SaveAgendamento(agendamento);

           
            string recipientEmail = paciente.Email;
            string subject = "Confirmação de Agendamento";
            string body = $"Seu agendamento foi confirmado para {dataConsulta.ToString("g")}.";
            _emailService.SendEmail(recipientEmail, subject, body);

            return agendamento;
        }
    }
}
