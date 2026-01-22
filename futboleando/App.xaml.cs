using futboleando.Pages;
using futboleando.Service;

namespace futboleando
{
    public partial class App : Application
    {
        private MenuService menuService;
        private LoginService loginService;
        private JugadorService jugadorService;
        private CiudadService ciudadService;    
        private ColaboradorService colaboradorService;  
        private EquipoService equipoService;
        private ComunicadoService comunicadoService;

        public static NavigationPage Navigate { get; internal set; }
        public static Flyout Menu { get; internal set; }

        public App(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService, CiudadService _ciudadService, 
            ColaboradorService _colaboradorService, EquipoService _equipoService, ComunicadoService _comunicadoService)
        {
            InitializeComponent();
            menuService = _menuService;
            loginService = _loginService;
            jugadorService = _jugadorService;
            ciudadService = _ciudadService;
            colaboradorService = _colaboradorService;
            equipoService = _equipoService;
            comunicadoService = _comunicadoService;

            // Iniciar con SplashPage
            MainPage = new SplashPage(menuService, loginService, jugadorService, 
                ciudadService, colaboradorService, equipoService, comunicadoService);
        }
    }
}
