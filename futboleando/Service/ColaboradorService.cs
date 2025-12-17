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
        public ColaboradorService()
        {
            listacolaborador = new ObservableCollection<ColaboradorListCLS>
            {
                new ColaboradorListCLS { idcolaborador = 1, nombre = "Juan Antonio", appaterno = "Garcia", apmaterno = "Quintero", idciudad = 1, nombreciudad = "Cd. Obregón", fechanacimiento = new DateOnly(1978, 12, 15)},
                new ColaboradorListCLS { idcolaborador = 2, nombre = "María", appaterno = "Torres", apmaterno = "Rodriguez",  idciudad = 2 , nombreciudad = "Hermosillo", fechanacimiento = new DateOnly(1985, 5, 20)},
                new ColaboradorListCLS { idcolaborador = 3, nombre = "Ernesto", appaterno = "Miranda", apmaterno = "Estrada", idciudad = 1, nombreciudad = "Cd. Obregón", fechanacimiento = new DateOnly(1990, 3, 10) },
                new ColaboradorListCLS { idcolaborador = 4, nombre = "Marco", appaterno = "Casto", apmaterno = "Nuñes", idciudad = 2, nombreciudad = "Hermosillo", fechanacimiento = new DateOnly(1982, 11, 8) }
            };
        }

        public async Task<ObservableCollection<ColaboradorListCLS>> listarColaborador()
        {
            return listacolaborador;
        }

        public async Task<int> guardarColaborador(ColaboradorFormCLS oColaboradorFormCLS)
        {
            try
            {
                ColaboradorListCLS oColaboradorListCLS = new ColaboradorListCLS();
                oColaboradorListCLS.nombre = oColaboradorFormCLS.nombre;
                oColaboradorListCLS.appaterno = oColaboradorFormCLS.appaterno;
                oColaboradorListCLS.apmaterno = oColaboradorFormCLS.apmaterno;


                listacolaborador.Add(oColaboradorListCLS);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
