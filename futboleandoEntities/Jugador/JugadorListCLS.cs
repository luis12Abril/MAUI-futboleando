using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Jugador
{
    public class JugadorListCLS
    {
        public int idjugador { get; set; }  
        public string nombre { get; set; } = string.Empty;
        public string appaterno { get; set; } = string.Empty;
        //public string apmaterno { get; set; } = string.Empty;
        //public string nombrecompleto { get; set; } = string.Empty;
        public string nombreequipo { get; set; } = string.Empty;
        //public DateOnly fechanacimiento { get; set; }
        //public int idequipo { get; set; }

        //public virtual Equipo? IdequipoNavigation { get; set; }

    }
}
