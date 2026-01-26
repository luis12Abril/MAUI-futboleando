using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Login
{
    public class RegistroResponseCLS
    {
        public bool exito { get; set; }
        public string mensaje { get; set; }
        public int idusuario { get; set; }
        public string nombre { get; set; }
    }
}
