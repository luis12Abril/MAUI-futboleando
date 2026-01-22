using Microsoft.Extensions.Logging;
using futboleando.Service;

namespace futboleando
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddScoped<LoginService>();
            builder.Services.AddScoped<MenuService>();
            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<JugadorService>();
            builder.Services.AddSingleton<CiudadService>();
            builder.Services.AddSingleton<ColaboradorService>();
            builder.Services.AddScoped<EquipoService>();
            builder.Services.AddScoped<ComunicadoService>();


            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://futboleandoapp.somee.com/")
            });
            ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }
    }
}
