using System.Text.Json.Serialization;

namespace futboleandoEntities.Juego
{
    public class JuegoDetallesCLS
    {
        // Información del juego
        public int idjuego { get; set; }
        public string nombrejornada { get; set; } = string.Empty;
        public DateTime? fhorario { get; set; }
        public string nombreestatusjuego { get; set; } = string.Empty;
        
        // Equipo 1
        public int idequipo01 { get; set; }
        public string nombreequipo01 { get; set; } = string.Empty;
        public int? golesequipo01 { get; set; }
        public string fotoequipo01 { get; set; } = string.Empty;
        
        // Equipo 2
        public int idequipo02 { get; set; }
        public string nombreequipo02 { get; set; } = string.Empty;
        public int? golesequipo02 { get; set; }
        public string fotoequipo02 { get; set; } = string.Empty;
        
        // Campo y árbitro
        public string nombrecampo { get; set; } = string.Empty;
        public string ubicacioncampo { get; set; } = string.Empty;
        public string nombrearbitro { get; set; } = string.Empty;
        
        // Goles
        [JsonPropertyName("detalleGolesEquipo01")]
        public List<GolDetalleCLS> golesEquipo01 { get; set; } = new List<GolDetalleCLS>();
        [JsonPropertyName("detalleGolesEquipo02")]
        public List<GolDetalleCLS> golesEquipo02 { get; set; } = new List<GolDetalleCLS>();
    }
    
    public class GolDetalleCLS
    {
        public int idjugador { get; set; }
        public string nombrejugador { get; set; } = string.Empty;
        public int goles { get; set; }
        public int? habilitado { get; set; }
    }
}
