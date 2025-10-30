using futboleando.Service;
namespace futboleando.Pages;

public partial class Flyout : FlyoutPage
{
    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;
    public Flyout(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService)
    {
        InitializeComponent();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;
        
        var menu = new MenuPage(menuService, loginService, jugadorService);
        Flyout = menu;
        App.Navigate = Navigate;
        App.Menu = this;
    }
}