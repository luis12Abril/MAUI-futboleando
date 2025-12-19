using futboleando.Generic;
using futboleando.Pages.Colaborador;
using futboleandoEntities.Colaborador;
using futboleandoEntities.Ciudad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Models
{
    public class ColaboradorModel : BaseBinding
    {
        private ColaboradorFormCLS _oColaboradorFormCLS;
        public ColaboradorFormCLS oColaboradorFormCLS
        {
            get { return _oColaboradorFormCLS; }
            set { SetValue(ref _oColaboradorFormCLS, value); }
        }


        private ObservableCollection<CiudadListCLS> _listaciudad;
        public ObservableCollection<CiudadListCLS> listaciudad
        {
            get { return _listaciudad; }
            set { SetValue(ref _listaciudad, value); }
        }



        private CiudadListCLS _opcionSeleccionadaCLS;
        public CiudadListCLS opcionSeleccionadaCLS
        {
            get { return _opcionSeleccionadaCLS; }
            set { SetValue(ref _opcionSeleccionadaCLS, value); }
        }
    }
}
