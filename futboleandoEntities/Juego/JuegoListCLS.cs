using System;

namespace futboleandoEntities.Juego
{
    public class JuegoListCLS
    {
        public int idjuego { get; set; }
        public int idjornada { get; set; }
        public string nombrejornada { get; set; } = string.Empty;
        public int idequipo01 { get; set; }
        public string nombreequipo01 { get; set; } = string.Empty;
        public int? golesequipo01 { get; set; }
        public int idequipo02 { get; set; }
        public string nombreequipo02 { get; set; } = string.Empty;
        public int? golesequipo02 { get; set; }
        public DateTime? fhorario { get; set; }
        public int? idcampo { get; set; }
        public string nombrecampo { get; set; } = string.Empty;
        public int? idestatusjuego { get; set; }
        public string nombreestatusjuego { get; set; } = string.Empty;
        public int idtorneo { get; set; }
    }
}
