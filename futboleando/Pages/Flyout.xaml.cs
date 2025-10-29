using futboleando.Service;
namespace futboleando.Pages;

public partial class Flyout : FlyoutPage
{
    public Flyout()
    {
        InitializeComponent();
        MenuService menuService = MauiProgram.ServiceProvider.GetService<MenuService>();
        var menu = new MenuPage(menuService);
        Flyout = menu;
        App.Navigate = Navigate;
        App.Menu = this;
    }
}