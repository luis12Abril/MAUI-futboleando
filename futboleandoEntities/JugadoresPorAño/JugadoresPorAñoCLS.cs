using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.JugadoresPorAño
{
    // Clase para el resumen por año
    public class JugadoresPorAñoCLS
    {
        public int año { get; set; }
        public int cantidad { get; set; }
    }

    // Clase para equipos (para el picker)
    public class EquipoSimpleCLS
    {
        public int idequipo { get; set; }
        public string? nombre { get; set; }
    }
}
