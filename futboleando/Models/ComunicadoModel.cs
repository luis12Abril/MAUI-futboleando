using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using futboleando.Generic;
using futboleandoEntities.Comunicado;

namespace futboleando.Models
{
    public class ComunicadoModel : BaseBinding
    {
        private ComunicadoFormCLS _comunicadoFormCLS;
        public event Func<Task> OnChange;

        public ComunicadoFormCLS oComunicadoFormCLS
        {
            get { return _comunicadoFormCLS; }
            set { SetValue(ref _comunicadoFormCLS, value); }
        }
    }
}
