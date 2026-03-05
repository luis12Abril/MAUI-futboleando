using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace futboleando
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            WindowCompat.SetDecorFitsSystemWindows(Window, false);
            ViewCompat.SetOnApplyWindowInsetsListener(Window.DecorView, new SystemInsetsListener());
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#FFFFFF"));

            var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
            controller.AppearanceLightStatusBars = true;
        }

        private sealed class SystemInsetsListener : Java.Lang.Object, AndroidX.Core.View.IOnApplyWindowInsetsListener
        {
            public WindowInsetsCompat OnApplyWindowInsets(Android.Views.View v, WindowInsetsCompat insets)
            {
                var systemBars = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());
                v.SetPadding(systemBars.Left, systemBars.Top, systemBars.Right, systemBars.Bottom);
                return insets;
            }
        }
    }
}

