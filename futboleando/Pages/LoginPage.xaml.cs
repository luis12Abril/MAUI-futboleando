using futboleandoEntities.Login;
using futboleando.Service;

namespace futboleando.Pages;

public partial class LoginPage : ContentPage
{
    public LoginCLS oLoginCLS { get; set; }

    private readonly LoginService loginService;

    //public string nombreusuario { get; set; }
    //public string contra { get; set; }
    public LoginPage(LoginService _loginService)
    {
        InitializeComponent();
        oLoginCLS = new LoginCLS();
        loginService = _loginService;
        BindingContext = this;
    }

    private async void btnIngresar_Clicked(object sender, EventArgs e)
    {
        bool exito = await loginService.login(oLoginCLS);
        if (exito == true)
        {
            Preferences.Set("usuario", "ok");
            Flyout p = new Flyout();
            App.Current.MainPage = p;
        }
        else
        {
            DisplayAlert("Error", "Usuario o contraseña incorrecta", "Salir");
        }

    }
}