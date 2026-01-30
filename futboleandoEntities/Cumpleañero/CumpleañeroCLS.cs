using System;

namespace futboleandoEntities.Cumpleañero
{
    public class CumpleañeroCLS
    {
        public int idjugador { get; set; }
        public string nombrecompleto { get; set; } = string.Empty;
        public DateOnly fechanacimiento { get; set; }
        public string nombreequipo { get; set; } = string.Empty;
        public int edad { get; set; }
        public bool esCumpleañosHoy { get; set; }
        public int diasParaCumpleaños { get; set; }
        
        // Propiedad calculada para mostrar la fecha o "HOY"
        public string fechaDisplay => esCumpleañosHoy ? "HOY" : fechanacimiento.ToString("dd/MMM").ToUpper();
    }
}
