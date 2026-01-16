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
        public static NavigationPage Navigate { get; internal set; }
        public static Flyout Menu { get; internal set; }

        public App(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService, CiudadService _ciudadService, 
            ColaboradorService _colaboradorService, EquipoService _equipoService)
        {
            InitializeComponent();
            menuService = _menuService;
            loginService = _loginService;
            jugadorService = _jugadorService;
            ciudadService = _ciudadService;
            colaboradorService = _colaboradorService;

            equipoService = _equipoService;

            // este Preferences se lleno el login
            // MainPage es la pagina principal que se carga al iniciar la app
            if (Preferences.Get("usuario", "") == "")
                MainPage = new LoginPage(menuService, loginService, jugadorService, ciudadService, colaboradorService, equipoService);
            else
                MainPage = new Flyout(menuService, loginService, jugadorService, ciudadService, colaboradorService, equipoService);
        }
    }
}
