using futboleando.Generic;
using futboleando.Pages.Colaborador;
using futboleandoEntities.Colaborador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Models
{
    public class ColaboradorModel :BaseBinding
    {
        private ColaboradorFormCLS _oColaboradorFormCLS;
        public ColaboradorFormCLS oColaboradorFormCLS
        {
            get { return _oColaboradorFormCLS; }
            set { SetValue(ref _oColaboradorFormCLS, value); }
        }
    }
}
