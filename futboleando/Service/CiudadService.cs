using futboleandoEntities.Ciudad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class CiudadService
    {
        public ObservableCollection<CiudadListCLS> listaciudad { get; set; }
        public CiudadService() 
        {
            CiudadListCLS primerItem = new CiudadListCLS { idciudad = 0, nombreciudad = "-- Todos --", descripcion = "Descripcion de Ciudad C" };
            listaciudad = new ObservableCollection<CiudadListCLS>()
            {
                new CiudadListCLS { idciudad = 1, nombreciudad = "Cd. Obregón", descripcion = "Descripcion de Ciudad A" },
                new CiudadListCLS { idciudad = 2, nombreciudad = "Hermosillo", descripcion = "Descripcion de Ciudad B" }
            };

        }

        public ObservableCollection<CiudadListCLS> listarciudad()
        {
            return listaciudad;
        }

    }
}
