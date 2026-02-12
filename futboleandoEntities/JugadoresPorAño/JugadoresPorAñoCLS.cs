using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace futboleandoEntities.JugadoresPorAño
{
    // Clase para el resumen por año
    public class JugadoresPorAñoCLS
    {
        [JsonPropertyName("año")]
        public int Anio { get; set; }
        [JsonIgnore]
        public int año
        {
            get => Anio;
            set => Anio = value;
        }
        public int cantidad { get; set; }
    }

    // Clase para equipos (para el picker)
    public class EquipoSimpleCLS
    {
        public int idequipo { get; set; }
        public string? nombre { get; set; }
    }
}
