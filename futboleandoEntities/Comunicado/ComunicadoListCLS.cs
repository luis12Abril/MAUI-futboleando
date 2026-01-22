using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Comunicado
{
    public class ComunicadoListCLS
    {
        public int? idcomunicado { get; set; }
        public string comunicadocorto { get; set; }
        public string comunicadolargo { get; set; }
        public DateOnly? fechacomunicado { get; set; }
        public string fechacomunicadoformateada { get; set; } = string.Empty; // ✅ NUEVA PROPIEDAD
        public int? idtorneo { get; set; }
        public int? habilitado { get; set; }


    }
}
