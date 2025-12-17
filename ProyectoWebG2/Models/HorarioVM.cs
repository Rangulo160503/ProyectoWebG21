namespace ProyectoWebG2.Models
{
    public class HorarioVM
    {
        public int IdHorario { get; set; }
        public int IdCurso { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
    }
}
