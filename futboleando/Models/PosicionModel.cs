using futboleandoEntities.Equipo;

namespace futboleando.Models
{
    public class PosicionModel
    {
        public int Posicion { get; set; }
        public EquipoListCLS Equipo { get; set; }
        public string EquipoNombre { get; set; } = string.Empty;
        public int Jugados { get; set; }
        public int Ganados { get; set; }
        public int Perdidos { get; set; }
        public int Empatados { get; set; }
        public int EmpatadosGanados { get; set; }
        public int GolesAFavor { get; set; }
        public int GolesEnContra { get; set; }
        public int DifGoles { get; set; }
        public int PuntosExtras { get; set; }
        public int Puntos { get; set; }
    }
}
