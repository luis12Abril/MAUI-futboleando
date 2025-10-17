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
        public string nombre { get; set; }
        public string appaterno { get; set; }
        public string apmaterno { get; set; }
        public string combrecompleto { get; set; }
        public string nombreequipo { get; set; }
        public DateTime fechanacimiento { get; set; }
        //public virtual Equipo? IdequipoNavigation { get; set; }

    }
}
