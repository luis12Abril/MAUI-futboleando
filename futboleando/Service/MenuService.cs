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
        private ObservableCollection<MenuCLS> listamenuCompleta;

        public MenuService()
        {
            // Lista completa de opciones de menú
            listamenuCompleta = new ObservableCollection<MenuCLS>
            {
                new MenuCLS{ idmenu=3 , nombreopcion="Equipos" , nombreicono="👥"},
                new MenuCLS{ idmenu=2 , nombreopcion="Jugadores" , nombreicono="🏃"},
                new MenuCLS{ idmenu=7 , nombreopcion="Juegos" , nombreicono="⚽"},
                new MenuCLS{ idmenu=8 , nombreopcion="Posiciones" , nombreicono="🏆"},
                new MenuCLS{ idmenu=14 , nombreopcion="Últimos Cinco Juegos" , nombreicono="📋"},
                new MenuCLS{ idmenu=9 , nombreopcion="Goleadores" , nombreicono="⚽"},
                new MenuCLS{ idmenu=10 , nombreopcion="Jugadores Por Año" , nombreicono="📅"},
                new MenuCLS{ idmenu=6 , nombreopcion="Comunicados" , nombreicono="📢"},
                new MenuCLS{ idmenu=13 , nombreopcion="Próximos Cumpleañeros" , nombreicono="🎂"},
                new MenuCLS{ idmenu=11 , nombreopcion="Visitas App" , nombreicono="📊"},  // ✅ Solo para admin
                new MenuCLS{ idmenu=12 , nombreopcion="Visitas Torneos" , nombreicono="🏟️"},  // ✅ Solo para admin
                new MenuCLS{ idmenu=15 , nombreopcion="Contacto" , nombreicono="☎️"},
                new MenuCLS{ idmenu=99 , nombreopcion="Seleccionar Otro Torneo" , nombreicono="🏆"},
                new MenuCLS{ idmenu=1000 , nombreopcion="Cerrar Sesión" , nombreicono="🚪"}
            };

            listamenu = new ObservableCollection<MenuCLS>();
        }

        public async Task<ObservableCollection<MenuCLS>> listarMenu()
        {
            // Obtener el IdUsuario de las preferencias
            int idUsuario = Preferences.Get("IdUsuario", 0);

            listamenu.Clear();

            foreach (var menu in listamenuCompleta)
            {
                // La opción de visitas (id=11/12) solo se muestra si es administrador (IdUsuario = 1)
                if ((menu.idmenu == 11 || menu.idmenu == 12) && idUsuario != 1)
                {
                    continue; // Saltar esta opción para usuarios no administradores
                }

                listamenu.Add(menu);
            }

            return listamenu;
        }
    }
}
