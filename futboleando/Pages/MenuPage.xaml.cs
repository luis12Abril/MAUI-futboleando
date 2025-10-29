using futboleando.Service;
using futboleandoEntities.Menu;
using System.Collections.ObjectModel;

namespace futboleando.Pages;

public partial class MenuPage : ContentPage
{
    public ObservableCollection<MenuCLS> listamenu { get; set; }
    public MenuCLS oMenuCLS { get; set; }
    private MenuService menuService;


    public MenuPage(MenuService _menuService)
	{
        InitializeComponent();
        menuService = _menuService;
        listarMenus();

        BindingContext = this;
    }

    public async Task listarMenus()
    {
        listamenu = await menuService.listarMenu();
    }

    private void lstMenu_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if(e.Item is MenuCLS selectedMenu)
        {

            //CarreraService carreraService = MauiProgram.ServiceProvider.GetService<CarreraService>();
            //CursoService cursoService = MauiProgram.ServiceProvider.GetService<CursoService>();
            //LoginService loginService = MauiProgram.ServiceProvider.GetService<LoginService>();
            //PersonaService personaService = MauiProgram.ServiceProvider.GetService<PersonaService>();

            int idmenu = selectedMenu.idmenu;
            LoginService loginService = MauiProgram.ServiceProvider.GetService<LoginService>();

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
                    App.Current.MainPage = new LoginPage(loginService); break;

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
            App.Menu.IsPresented = false;
        }
       
    }
}