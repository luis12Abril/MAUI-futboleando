using futboleandoEntities.Login;
using futboleando.Service;
using futboleando.Pages.Registro;

namespace futboleando.Pages;

public partial class LoginPage : ContentPage
{
    public LoginCLS oLoginCLS { get; set; }

    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;
    private CiudadService ciudadService;
    private ColaboradorService colaboradorService;  

    private EquipoService equipoService;
    private ComunicadoService comunicadoService;

    public LoginPage(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService, CiudadService _ciudadService,
        ColaboradorService _colaboradorService, EquipoService _equipoService, ComunicadoService _comunicadoService)
    {
        InitializeComponent();
        oLoginCLS = new LoginCLS();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;
        ciudadService = _ciudadService;
        colaboradorService = _colaboradorService;

        equipoService = _equipoService;
        comunicadoService = _comunicadoService;
        BindingContext = this;
    }

    private async void btnIngresar_Clicked(object sender, EventArgs e)
    {
        // Validar campos vacíos
        if (string.IsNullOrWhiteSpace(oLoginCLS.nombreusuario) || string.IsNullOrWhiteSpace(oLoginCLS.contra))
        {
            await DisplayAlert("Error", "Por favor complete todos los campos", "OK");
            return;
        }

        // Mostrar indicador de carga
        btnIngresar.IsEnabled = false;
        btnIngresar.Text = "Ingresando...";

        // ? Llamar al servicio de login mejorado
        var loginResponse = await loginService.login(oLoginCLS);
        
        if (loginResponse != null && loginResponse.exito == true)
        {
            // ? Guardar sesión con datos del usuario
            Preferences.Set("usuario", "ok");
            Preferences.Set("IdUsuario", loginResponse.idusuario);
            Preferences.Set("NombreUsuario", loginResponse.nombre);
            Preferences.Set("IdTipoUsuario", loginResponse.idtipousuario);
            Preferences.Set("NombreTipoUsuario", loginResponse.nombretipousuario);
            
            // ? Ir directamente al selector de torneo (sin mensaje de bienvenida)
            var estadoService = MauiProgram.ServiceProvider.GetService<EstadoService>();
            var municipioService = MauiProgram.ServiceProvider.GetService<MunicipioService>();
            var ligaService = MauiProgram.ServiceProvider.GetService<LigaService>();
            var torneoService = MauiProgram.ServiceProvider.GetService<TorneoService>();

            App.Current.MainPage = new NavigationPage(
                new TorneoSelectorPage(estadoService, municipioService, ligaService, torneoService,
                    menuService, loginService, jugadorService, ciudadService, colaboradorService,
                    equipoService, comunicadoService)
            );
        }
        else
        {
            string mensajeError = loginResponse?.mensaje ?? "Error de conexión con el servidor";
            await DisplayAlert("Error de Autenticación", mensajeError, "OK");
            btnIngresar.IsEnabled = true;
            btnIngresar.Text = "INGRESAR";
        }
    }

    private async void OnRegistrarTapped(object sender, EventArgs e)
    {
        // ? Navegar a la página de registro usando PushModalAsync (siempre funciona)
        var registroPage = new RegistroPage(loginService);
        await Navigation.PushModalAsync(new NavigationPage(registroPage)
        {
            BarBackgroundColor = Colors.Transparent,
            BarTextColor = Colors.White
        });
    }
}