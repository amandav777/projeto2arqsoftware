using System;
using System.Threading.Tasks;
using projeto2arqsoftware.DTOs;
using projeto2arqsoftware.Models;
using projeto2arqsoftware.Repositories;
using projeto2arqsoftware.Services;

namespace projeto2arqsoftware.Services
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
          
            if (agendamentoDTO.DataConsulta < DateTime.Now)
            {
                throw new Exception("A data da consulta não pode ser no passado.");
            }

            DateTime inicioConsulta = agendamentoDTO.DataConsulta.Date; 
            DateTime fimConsulta = inicioConsulta.AddDays(1).AddTicks(-1);

            Paciente paciente = await _pacienteService.GetPacienteOrThrowError(agendamentoDTO.PacienteId);
            Medico medico = await _medicoService.GetMedicoOrThrowError(agendamentoDTO.MedicoId);

            bool medicoDisponivel = await _disponibilidadeService.VerificaDisponibilidade(medico.Id, inicioConsulta);
            if (!medicoDisponivel)
            {
                throw new Exception("Médico indisponível para a data especificada.");
            }

            Agendamento agendamento = new Agendamento
            {
                Paciente = paciente,
                Medico = medico,
                Data = inicioConsulta,
                Status = true 
            };

            agendamento = await _agendamentoRepository.SaveAgendamento(agendamento);

            
            string subject = "Confirmação de Agendamento";
            string body = $"Olá {paciente.Nome}, seu agendamento foi confirmado para o dia {inicioConsulta:dd/MM/yyyy}.";

            _emailService.SendEmail(paciente.Email, subject, body);

            return agendamento;
        }
    }
}
