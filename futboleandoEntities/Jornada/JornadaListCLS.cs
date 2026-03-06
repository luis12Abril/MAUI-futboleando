using System;

namespace futboleandoEntities.Jornada
{
    public class JornadaListCLS
    {
        public int idjornada { get; set; }
        public string nombre { get; set; } = string.Empty;
        public DateTime? finiciojornada { get; set; }
        public int idtorneo { get; set; }
    }
}
