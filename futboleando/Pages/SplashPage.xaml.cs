using futboleando.Service;

namespace futboleando.Pages
{
    public partial class SplashPage : ContentPage
    {
        private readonly MenuService menuService;
        private readonly LoginService loginService;
        private readonly JugadorService jugadorService;
        private readonly CiudadService ciudadService;
        private readonly ColaboradorService colaboradorService;
        private readonly EquipoService equipoService;
        private readonly ComunicadoService comunicadoService;
        
        private readonly EstadoService estadoService;
        private readonly MunicipioService municipioService;
        private readonly LigaService ligaService;
        private readonly TorneoService torneoService;

        public SplashPage(MenuService _menuService, LoginService _loginService,
            JugadorService _jugadorService, CiudadService _ciudadService,
            ColaboradorService _colaboradorService, EquipoService _equipoService,
            ComunicadoService _comunicadoService,
            EstadoService _estadoService, MunicipioService _municipioService,
            LigaService _ligaService, TorneoService _torneoService)
        {
            InitializeComponent();

            menuService = _menuService;
            loginService = _loginService;
            jugadorService = _jugadorService;
            ciudadService = _ciudadService;
            colaboradorService = _colaboradorService;
            equipoService = _equipoService;
            comunicadoService = _comunicadoService;
            
            estadoService = _estadoService;
            municipioService = _municipioService;
            ligaService = _ligaService;
            torneoService = _torneoService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnimateEntrance();
        }

        private async Task AnimateEntrance()
        {
            // Animación del balón (aparecer y rotar)
            var ballAnimation = lblBall.FadeTo(1, 800, Easing.CubicOut);
            var scaleAnimation = lblBall.ScaleTo(1, 800, Easing.BounceOut);
            var rotateAnimation = lblBall.RotateTo(360, 1000, Easing.Linear);

            await Task.WhenAll(ballAnimation, scaleAnimation, rotateAnimation);

            // Animación del título
            await lblTitle.FadeTo(1, 600, Easing.CubicIn);

            // Mostrar indicador de carga
            await activityIndicator.FadeTo(1, 400);

            // Simular carga de datos (puedes agregar lógica real aquí)
            await Task.Delay(1500);

            // Navegar según el estado del login
            await NavigateToMainPage();
        }

        private async Task NavigateToMainPage()
        {
            // Verificar si hay sesión activa
            var usuario = Preferences.Get("usuario", "");

            if (string.IsNullOrEmpty(usuario))
            {
                // Ir al Login
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage(menuService, loginService, jugadorService,
                        ciudadService, colaboradorService, equipoService, comunicadoService)
                );
            }
            else
            {
                // ? CAMBIO: Siempre ir al selector de torneo (mostrará última selección si existe)
                Application.Current.MainPage = new NavigationPage(
                    new TorneoSelectorPage(estadoService, municipioService, ligaService, torneoService,
                        menuService, loginService, jugadorService, ciudadService, colaboradorService,
                        equipoService, comunicadoService)
                );
            }
        }
    }
}
