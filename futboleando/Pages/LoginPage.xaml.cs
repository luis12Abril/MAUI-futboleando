using futboleandoEntities.Login;
using futboleando.Service;

namespace futboleando.Pages;

public partial class LoginPage : ContentPage
{
    public LoginCLS oLoginCLS { get; set; }

    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;

    //public string nombreusuario { get; set; }
    //public string contra { get; set; }
    public LoginPage(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService)
    {
        InitializeComponent();
        oLoginCLS = new LoginCLS();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;
        BindingContext = this;
    }

    private async void btnIngresar_Clicked(object sender, EventArgs e)
    {
        bool exito = await loginService.login(oLoginCLS);
        if (exito == true)
        {
            // se utiliza para para guardar datos en el dispositivo
            Preferences.Set("usuario", "ok");
            Flyout p = new Flyout(menuService, loginService, jugadorService);
            App.Current.MainPage = p;
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrecta", "Salir");
        }

    }
}