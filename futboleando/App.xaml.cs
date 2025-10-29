using futboleando.Pages;
using futboleando.Service;

namespace futboleando
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            LoginService loginService = MauiProgram.ServiceProvider.GetService<LoginService>();

            if (Preferences.Get("usuario", "") == "")
                MainPage = new LoginPage(loginService);
            else
                MainPage = new Flyout();
        }

        public static NavigationPage Navigate { get; internal set; }
        public static Flyout Menu { get; internal set; }
    }
}
