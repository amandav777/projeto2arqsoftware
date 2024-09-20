namespace projeto2.Controllers
{
    public class Agendamento
    {
        public string Id { get; set; }
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }; 
        public DateTime? Data { get; set; }
    }
}
