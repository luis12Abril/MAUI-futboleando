using futboleando.Pages.Ciudad;
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
        public event Func<Task> OnChange;
        public CiudadService() 
        {
            CiudadListCLS primerItem = new CiudadListCLS { idciudad = 0, nombreciudad = "-- Todos --", descripcion = "Descripcion de Ciudad C" };
            listaciudad = new ObservableCollection<CiudadListCLS>()
            {
                new CiudadListCLS { idciudad = 1, nombreciudad = "Cd. Obregón", descripcion = "Descripcion de Ciudad A" },
                new CiudadListCLS { idciudad = 2, nombreciudad = "Hermosillo", descripcion = "Descripcion de Ciudad B" }
            };

        }

        public void NotificarChange()
        {
            OnChange?.Invoke();
        }

        public async Task<ObservableCollection<CiudadListCLS>> listarciudad()
        {
            return listaciudad;
        }


        public async Task<int> guardarCiudad(CiudadFormCLS oCiudadFormCLS)
        {
            try
            {
                listaciudad.Add(new CiudadListCLS
                {
                    idciudad = listaciudad.Count + 1,
                    nombreciudad = oCiudadFormCLS.nombreciudad,
                    descripcion = oCiudadFormCLS.descripcion
                });
                return 1;
            }
            catch(Exception ex)
            {
                return 0;
            }
           
        }
    }
}
