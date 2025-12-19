using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class ColaboradorService
    {
        public ObservableCollection<ColaboradorListCLS> listacolaborador;
        private readonly CiudadService ciudadService;
        public event Func<int, Task> OnGet;
        public ColaboradorService(CiudadService _ciudadService)
        {
            ciudadService = _ciudadService;
            listacolaborador = new ObservableCollection<ColaboradorListCLS>
            {
                new ColaboradorListCLS { idcolaborador = 1, nombre = "Juan Antonio", appaterno = "Garcia", apmaterno = "Quintero", idciudad = 1, nombreciudad = "Cd. Obregón", fechanacimiento = new DateOnly(1978, 12, 15)},
                new ColaboradorListCLS { idcolaborador = 2, nombre = "María", appaterno = "Torres", apmaterno = "Rodriguez",  idciudad = 2 , nombreciudad = "Hermosillo", fechanacimiento = new DateOnly(1985, 5, 20)},
                new ColaboradorListCLS { idcolaborador = 3, nombre = "Ernesto", appaterno = "Miranda", apmaterno = "Estrada", idciudad = 1, nombreciudad = "Cd. Obregón", fechanacimiento = new DateOnly(1990, 3, 10) },
            };
        }

        public void NotificarGet(int id)
        {
            OnGet?.Invoke(id);
        }

        public async Task<ObservableCollection<ColaboradorListCLS>> listarColaborador()
        {
            return listacolaborador;
        }

        public async Task<int> guardarColaborador(ColaboradorFormCLS oColaboradorFormCLS)
        {
            try
            {
                var listaciudad = await ciudadService.listarCiudad();
                ColaboradorListCLS oColaboradorListCLS = new ColaboradorListCLS();
                oColaboradorListCLS.nombre = oColaboradorFormCLS.nombre;
                oColaboradorListCLS.appaterno = oColaboradorFormCLS.appaterno;
                oColaboradorListCLS.apmaterno = oColaboradorFormCLS.apmaterno;

                oColaboradorListCLS.nombreciudad = listaciudad.FirstOrDefault(x=> x.idciudad == oColaboradorFormCLS.idciudad).nombreciudad;

                listacolaborador.Add(oColaboradorListCLS);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<ColaboradorFormCLS> recuperarColaboradorPorId(int idcolaborador)
        {
            try
            {
                var listaciudad = await ciudadService.listarCiudad();
                ColaboradorFormCLS oColaboradorFormCLS = new ColaboradorFormCLS();
                ColaboradorListCLS oColaboradorListCLS = listacolaborador.FirstOrDefault(c => c.idcolaborador == idcolaborador);

                oColaboradorFormCLS.idcolaborador = oColaboradorListCLS.idcolaborador;
                oColaboradorFormCLS.nombre = oColaboradorListCLS.nombre;
                oColaboradorFormCLS.appaterno = oColaboradorListCLS.appaterno;
                oColaboradorFormCLS.edad = DateTime.Now.Year - oColaboradorListCLS.fechanacimiento.Year;
                oColaboradorFormCLS.nombreciudad = oColaboradorListCLS.nombreciudad;
                oColaboradorFormCLS.idciudad = oColaboradorListCLS.idciudad;
                oColaboradorFormCLS.idciudad = listaciudad.FirstOrDefault(x => x.nombreciudad == oColaboradorListCLS.nombreciudad).idciudad;
                return oColaboradorFormCLS;
            }
            catch (Exception ex)
            {
                return new ColaboradorFormCLS();
            }
        }

    }
}
