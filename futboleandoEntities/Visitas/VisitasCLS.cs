using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Visitas
{
    // Clase para resumen de visitas totales
    public class VisitasTotalesCLS
    {
        public int totalVisitasWeb { get; set; }
        public int totalVisitasApp { get; set; }
    }

    // Clase para detalle de visitas por usuario
    public class VisitaUsuarioCLS
    {
        public int idusuario { get; set; }
        public string? nombreusuario { get; set; }
        public int? idtipousuario { get; set; }
        public string? nombretipousuario { get; set; }
        public int visitasWeb { get; set; }
        public int visitasApp { get; set; }
        public int totalVisitas { get; set; }

        // ? Propiedades calculadas con formato de miles
        public string visitasWebFormateado => visitasWeb.ToString("N0");
        public string visitasAppFormateado => visitasApp.ToString("N0");
    }

    // Clase para tipos de usuario (picker)
    public class TipoUsuarioSimpleCLS
    {
        public int idtipousuario { get; set; }
        public string? nombre { get; set; }
    }
}
