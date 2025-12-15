using futboleando.Generic;
using futboleando.Pages.Ciudad;
using futboleandoEntities.Ciudad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Models
{    public class CiudadModel : BaseBinding
    {
        private CiudadFormCLS _oCiudadFormCLS;         
        public CiudadFormCLS oCiudadFormCLS
        {
            get { return _oCiudadFormCLS; }
            set { SetValue(ref _oCiudadFormCLS, value); }
        }
    }
}
