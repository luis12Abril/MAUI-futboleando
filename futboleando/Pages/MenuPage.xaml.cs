using futboleando.Pages.Ciudad;
using futboleando.Pages.Colaborador;
using futboleando.Pages.Cumpleañero;
using futboleando.Pages.Comunicado;
using futboleando.Pages.Contacto;
using futboleando.Pages.Juego;
using futboleando.Pages.Goleador;
using futboleando.Pages.Posiciones;
using futboleando.Pages.JugadoresPorAño;
using futboleando.Pages.Visitas;
using futboleando.Service;
using futboleandoEntities.Menu;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;

namespace futboleando.Pages;

public partial class MenuPage : ContentPage
{
    public ObservableCollection<MenuCLS> listamenu { get; set; }
    public MenuCLS oMenuCLS { get; set; }
    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;
    private CiudadService ciudadService;
    private ColaboradorService colaboradorService;

    private EquipoService equipoService;

    private ComunicadoService comunicadoService;

    public MenuPage(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService, CiudadService _ciudadService, ColaboradorService _colaboradorService,
        EquipoService _equipoService, ComunicadoService _comunicadoService)
    {
        InitializeComponent();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;
        ciudadService = _ciudadService;
        colaboradorService = _colaboradorService;

        equipoService = _equipoService;
        comunicadoService = _comunicadoService;

        listarMenus();

        BindingContext = this;
        //this.comunicadoService = comunicadoService;
    }

    public async Task listarMenus()
    {
        listamenu = await menuService.listarMenu();
    }

    private async void lstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
    {

        //DisplayAlert("Aviso", oMenuCLS.nombreopcion, "Salir");

        //CarreraService carreraService = MauiProgram.ServiceProvider.GetService<CarreraService>();
        //CursoService cursoService = MauiProgram.ServiceProvider.GetService<CursoService>();
        //LoginService loginService = MauiProgram.ServiceProvider.GetService<LoginService>();
        //PersonaService personaService = MauiProgram.ServiceProvider.GetService<PersonaService>();

        // Obtener el item seleccionado desde el evento
        var menuSeleccionado = e.Item as MenuCLS;
        if (menuSeleccionado == null) return;

        int idmenu = menuSeleccionado.idmenu;

        //int idmenu = oMenuCLS.idmenu;

        switch (idmenu)
        {
            //case 1:
            //    UsuarioPage oUsuarioPage = new UsuarioPage();
            //    App.Navigate.PushAsync(oUsuarioPage); break;
            case 2:
                JugadorPage oJugadorPage = new JugadorPage(jugadorService, equipoService);
                App.Navigate.PushAsync(oJugadorPage); break;

            case 3:
                EquipoPage oEquiposPage = new EquipoPage(equipoService);
                App.Navigate.PushAsync(oEquiposPage); break;

            case 5:
                CiudadPage oCiudadPage = new CiudadPage(ciudadService);
                App.Navigate.PushAsync(oCiudadPage); break;

            case 6:
                ComunicadoPage oComunicadoPage = new ComunicadoPage(comunicadoService);
                App.Navigate.PushAsync(oComunicadoPage); break;

            // ? NUEVO: Juegos
            case 7:
                var juegoService = MauiProgram.ServiceProvider.GetService<JuegoService>();
                JuegoPage oJuegoPage = new JuegoPage(juegoService);
                App.Navigate.PushAsync(oJuegoPage); 
                break;

            // ? NUEVO: Posiciones
            case 8:
                var equipoServicePosiciones = MauiProgram.ServiceProvider.GetService<EquipoService>();
                PosicionesPage oPosicionesPage = new PosicionesPage(equipoServicePosiciones);
                App.Navigate.PushAsync(oPosicionesPage); 
                break;

            // ? NUEVO: Goleadores
            case 9:
                var goleadorService = MauiProgram.ServiceProvider.GetService<GoleadorService>();
                var equipoServiceGoleador = MauiProgram.ServiceProvider.GetService<EquipoService>();
                GoleadorPage oGoleadorPage = new GoleadorPage(goleadorService, equipoServiceGoleador);
                App.Navigate.PushAsync(oGoleadorPage); 
                break;

            case 14:
                var equipoServiceUltimos = MauiProgram.ServiceProvider.GetService<EquipoService>();
                var juegoServiceUltimos = MauiProgram.ServiceProvider.GetService<JuegoService>();
                UltimosCincoJuegosPage oUltimosCincoJuegosPage = new UltimosCincoJuegosPage(equipoServiceUltimos, juegoServiceUltimos);
                App.Navigate.PushAsync(oUltimosCincoJuegosPage);
                break;

            // ? NUEVO: Jugadores por Año
            case 10:
                var jugadoresPorAñoService = MauiProgram.ServiceProvider.GetService<JugadoresPorAñoService>();
                JugadoresPorAñoPage oJugadoresPorAñoPage = new JugadoresPorAñoPage(jugadoresPorAñoService);
                App.Navigate.PushAsync(oJugadoresPorAñoPage); 
                break;

            // ? NUEVO: Visitas App (solo para admin)
            case 11:
                var visitasService = MauiProgram.ServiceProvider.GetService<VisitasService>();
                VisitasAppPage oVisitasAppPage = new VisitasAppPage(visitasService);
                App.Navigate.PushAsync(oVisitasAppPage); 
                break;

            // ? NUEVO: Visitas Torneos (solo para admin)
            case 12:
                var visitasTorneoService = MauiProgram.ServiceProvider.GetService<VisitasService>();
                VisitasTorneoPage oVisitasTorneoPage = new VisitasTorneoPage(visitasTorneoService);
                App.Navigate.PushAsync(oVisitasTorneoPage);
                break;

            case 13:
                CumpleañerosdelTorneoPage oCumpleañerosdelTorneoPage = new CumpleañerosdelTorneoPage(jugadorService, equipoService);
                App.Navigate.PushAsync(oCumpleañerosdelTorneoPage);
                break;

            case 15:
                var avisoFutboleandoService = MauiProgram.ServiceProvider.GetService<AvisoFutboleandoService>();
                if (avisoFutboleandoService == null)
                {
                    await DisplayAlert("Contacto", "No se pudo cargar la información de contacto.", "OK");
                    break;
                }

                ContactoPage oContactoPage = new ContactoPage(avisoFutboleandoService);
                App.Navigate.PushAsync(oContactoPage);
                break;

            case 20:
                ColaboradorPage oColaboradorPage = new ColaboradorPage(ciudadService, colaboradorService);
                App.Navigate.PushAsync(oColaboradorPage); break;

            // ? Nueva opción: Seleccionar Torneo
            case 99:
                var estadoService = MauiProgram.ServiceProvider.GetService<EstadoService>();
                var municipioService = MauiProgram.ServiceProvider.GetService<MunicipioService>();
                var ligaService = MauiProgram.ServiceProvider.GetService<LigaService>();
                var torneoService = MauiProgram.ServiceProvider.GetService<TorneoService>();

                TorneoSelectorPage oTorneoSelectorPage = new TorneoSelectorPage(
                    estadoService, municipioService, ligaService, torneoService,
                    menuService, loginService, jugadorService, ciudadService, 
                    colaboradorService, equipoService, comunicadoService);
                
                App.Navigate.PushAsync(oTorneoSelectorPage); 
                break;

            case 1000:
                Preferences.Remove("usuario");
                App.Current.MainPage = new LoginPage(menuService, loginService, jugadorService, ciudadService, colaboradorService, equipoService, comunicadoService); break;

                //case 1:
                //    CursoPage oCursoPage = new CursoPage(carreraService, cursoService);
                //    App.Navigate.PushAsync(oCursoPage); break;
                //case 2:
                //    PersonaPage oPersonaPage = new PersonaPage(personaService);
                //    App.Navigate.PushAsync(oPersonaPage); break;
                //case 3:
                //    UsuarioPage oUsuarioPage = new UsuarioPage();
                //    App.Navigate.PushAsync(oUsuarioPage); break;
                //case 4:
                //    CarreraPage oCarreraPage = new CarreraPage(carreraService);
                //    App.Navigate.PushAsync(oCarreraPage); break;
                //case 1000:
                //    Preferences.Remove("usuario");
                //    App.Current.MainPage = new LoginPage(loginService); break;

        }

        // Cerrar el flyout/menu
        App.Menu.IsPresented = false;

        // Deseleccionar el item para evitar que quede marcado
        if (sender is ListView lv) lv.SelectedItem = null;

        //App.Menu.IsPresented = false;

    }
}