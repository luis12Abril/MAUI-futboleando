using futboleando.Service;
using futboleandoEntities.Menu;
using System.Collections.ObjectModel;

namespace futboleando.Pages;

public partial class MenuPage : ContentPage
{
    public ObservableCollection<MenuCLS> listamenu { get; set; }
    public MenuCLS oMenuCLS { get; set; }
    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;


    public MenuPage(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService)
    {
        InitializeComponent();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;

        listarMenus();

        BindingContext = this;
    }

    public async Task listarMenus()
    {
        listamenu = await menuService.listarMenu();
    }

    private void lstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
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
                JugadorPage oJugadorPage = new JugadorPage(jugadorService);
                App.Navigate.PushAsync(oJugadorPage); break;
               
            case 1000:
                Preferences.Remove("usuario");
                App.Current.MainPage = new LoginPage(menuService, loginService, jugadorService); break;

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