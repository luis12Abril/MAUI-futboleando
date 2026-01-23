using futboleandoEntities.Login;
using futboleando.Service;

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

        bool exito = await loginService.login(oLoginCLS);
        
        if (exito == true)
        {
            // Guardar sesión
            Preferences.Set("usuario", "ok");
            
            // ? CAMBIO: Siempre ir al selector de torneo
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
            await DisplayAlert("Error de Autenticación", "Usuario o contraseña incorrecta. Por favor intente nuevamente.", "OK");
            btnIngresar.IsEnabled = true;
            btnIngresar.Text = "INGRESAR";
        }
    }

    private async void OnRegistrarTapped(object sender, EventArgs e)
    {
        // Aquí puedes navegar a una página de registro o mostrar un mensaje
        await DisplayAlert("Registro", "La funcionalidad de registro estará disponible próximamente.", "OK");
    }
}