using futboleando.Service;
namespace futboleando.Pages;

public partial class Flyout : FlyoutPage
{
    private MenuService menuService;
    private LoginService loginService;
    private JugadorService jugadorService;
    private CiudadService ciudadService;    
    private ColaboradorService colaboradorService;

    private EquipoService equipoService;
    public Flyout(MenuService _menuService, LoginService _loginService, JugadorService _jugadorService, CiudadService _ciudadService,
        ColaboradorService _colaboradorService, EquipoService _equipoService )
    {
        InitializeComponent();
        menuService = _menuService;
        loginService = _loginService;
        jugadorService = _jugadorService;
        ciudadService = _ciudadService;
        colaboradorService = _colaboradorService;

        equipoService = _equipoService;

        var menu = new MenuPage(menuService, loginService, jugadorService, ciudadService, colaboradorService, equipoService);
        Flyout = menu;
        App.Navigate = Navigate;
        App.Menu = this;
    }
}