using futboleandoEntities.Menu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public class MenuService
    {
        private ObservableCollection<MenuCLS> listamenu;
        public MenuService()
        {
            listamenu = new ObservableCollection<MenuCLS>
            {
                new MenuCLS{ idmenu=1 , nombreopcion="Usuario" , nombreicono=""},
                new MenuCLS{ idmenu=2 , nombreopcion="Jugador" , nombreicono=""},
                new MenuCLS{ idmenu=3 , nombreopcion="Equipo" , nombreicono=""},
                new MenuCLS{ idmenu=4 , nombreopcion="Campo" , nombreicono=""},
                new MenuCLS{ idmenu=1000 , nombreopcion="Cerrar Sesion" , nombreicono=""}
            };
        }

        public async Task<ObservableCollection<MenuCLS>> listarMenu()
        {
            return listamenu;
        }

    }
}
