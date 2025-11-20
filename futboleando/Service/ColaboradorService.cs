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
                new ColaboradorListCLS { idcolaborador = 1, nombre = "Juan Antonio", appaterno = "Garcia", apmaterno = "Quintero", idciudad = 1, nombreciudad = "Cd. Obregón"},
                new ColaboradorListCLS { idcolaborador = 2, nombre = "María", appaterno = "Torres", apmaterno = "Rodriguez",  idciudad = 2 , nombreciudad = "Hermosillo"},

                new ColaboradorListCLS { idcolaborador = 3, nombre = "Ernesto", appaterno = "Miranda", apmaterno = "Estrada", idciudad = 1, nombreciudad = "Cd. Obregón" },
                new ColaboradorListCLS { idcolaborador = 4, nombre = "Marco", appaterno = "Casto", apmaterno = "Nuñes", idciudad = 2, nombreciudad = "Hermosillo" }
            };
        }

        public ObservableCollection<ColaboradorListCLS> listarcolaborador()
        {
            return listacolaborador;
        }

    }
}
