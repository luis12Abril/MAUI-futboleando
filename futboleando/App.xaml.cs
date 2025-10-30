using futboleando.Pages;
using futboleando.Service;

namespace futboleando
{
    public partial class App : Application
    {
        private MenuService menuService;
        private LoginService loginService;
        private JugadorService jugadorService;
        public App(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService)
        {
            InitializeComponent();
            menuService = _menuService;
            loginService = _loginService;
            jugadorService = _jugadorService;
          

            if (Preferences.Get("usuario", "") == "")
                MainPage = new LoginPage(menuService, loginService, jugadorService);
            else
                MainPage = new Flyout(menuService, loginService, jugadorService);
        }

        public static NavigationPage Navigate { get; internal set; }
        public static Flyout Menu { get; internal set; }
    }
}
