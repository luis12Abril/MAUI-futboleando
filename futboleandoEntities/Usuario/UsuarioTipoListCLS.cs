using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Usuario
{
    public class UsuarioTipoListCLS
    {
        public int idusuario { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int idtipousuario { get; set; }
        public int visitas { get; set; }
        public int visitascel { get; set; }
        public string nombretipousuario { get; set; } = string.Empty;
    }
}
