using futboleando.Generic;
using futboleandoEntities.Equipo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Models
{
    public class EquipoModel:BaseBinding
    {
        private EquipoFormCLS _oEquipoFormCLS;
        public event Func<Task> OnChange;

        public EquipoFormCLS oEquipoFormCLS
        {
            get { return _oEquipoFormCLS; }
            set { SetValue(ref _oEquipoFormCLS, value); }
        }
    }
}
