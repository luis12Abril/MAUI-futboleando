using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Colaborador
{
    public class ColaboradorFormCLS
    {
        public int idcolaborador { get; set; }
        public string nombre { get; set; }
        public string appaterno { get; set; }
        public string apmaterno { get; set; }
        public string telefono { get; set; }
        public int idciudad { get; set; }
        public string nombreciudad { get; set; }
        public DateOnly fechanacimiento { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public int edad { get; set; }
    }
}
