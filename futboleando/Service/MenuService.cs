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
                new MenuCLS{ idmenu=1 , nombreopcion="Usuario" , nombreicono="👤"},
                new MenuCLS{ idmenu=2 , nombreopcion="Jugador" , nombreicono="🏃"},
                new MenuCLS{ idmenu=3 , nombreopcion="Equipo" , nombreicono="👥"},
                new MenuCLS{ idmenu=4 , nombreopcion="Campo" , nombreicono="🏟️"},
                new MenuCLS{ idmenu=6 , nombreopcion="Comunicados" , nombreicono="📢"},
                new MenuCLS{ idmenu=5 , nombreopcion="Ciudad" , nombreicono="🏙️"},
                new MenuCLS{ idmenu=20 , nombreopcion="Colaborador" , nombreicono="🤝"},
                new MenuCLS{ idmenu=99 , nombreopcion="Seleccionar Torneo" , nombreicono="🏆"},  // ✅ Nueva opción
                new MenuCLS{ idmenu=1000 , nombreopcion="Cerrar Sesión" , nombreicono="🚪"}
            };
        }

        public async Task<ObservableCollection<MenuCLS>> listarMenu()
        {
            return listamenu;
        }

    }
}
