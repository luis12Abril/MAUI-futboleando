using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Goleador
{
    public class GoleadorCLS
    {
        public int idjugador { get; set; }
        public string? nombrecompleto { get; set; }
        public int? goles { get; set; }
        public int? idequipo { get; set; }
        public string? nombreequipo { get; set; }
        public int posicion { get; set; }
    }
}
