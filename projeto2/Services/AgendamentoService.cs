using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projeto2.DTOs;
using projeto2.Models;
using projeto2.Repositories;
using projeto2.Repositories.Data;
using projeto2.Services;

namespace AgendamentoService
{
    public class AgendamentoService
    {
        private readonly AppDbContext _context;
        private readonly PacienteService _pacienteService;
        private readonly MedicoService _medicoService;
        private readonly AgendamentoRepository _agendamentoRepository;
        private readonly DisponibilidadeService _disponibilidadeService;
        private readonly EmailService _emailService;


        public AgendamentoService(
           AppDbContext context,
           PacienteService pacienteService,
           MedicoService medicoService,
           AgendamentoRepository agendamentoRepository,
           DisponibilidadeService disponibilidadeService,
           EmailService emailService
        )
        {
            _context = context;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
            _agendamentoRepository = agendamentoRepository;
            _disponibilidadeService = disponibilidadeService;
            _emailService = emailService;
        }

        public async Task<Agendamento> CriarAgendamento(AgendamentoDTO agendamentoDTO)
        {
            
            Paciente paciente = await _pacienteService.GetPacienteOrThrowError(agendamentoDTO.PacienteId);

            
            Medico medico = await _medicoService.GetMedicoOrThrowError(agendamentoDTO.MedicoId);

            
            bool medicoDisponivel = await _disponibilidadeService.VerificaDisponibilidade(medico.Id, agendamentoDTO.DataConsulta);
            if (!medicoDisponivel)
            {
                throw new Exception("Médico indisponível para a data especificada.");
            }

            
            Agendamento agendamento = new Agendamento
            {
                Paciente = paciente,
                Medico = medico,
                Data = agendamentoDTO.DataConsulta,
                Status = true 
            };

            agendamento = await _agendamentoRepository.SaveAgendamento(agendamento);

          
            await _emailService.EnviarNotificacaoAgendamento(paciente.Email, agendamento);

            return agendamento;
        }
    )
}


