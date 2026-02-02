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
            builder.Services.AddScoped<JuegoService>();
            builder.Services.AddScoped<GoleadorService>();
            builder.Services.AddScoped<JugadoresPorAñoService>();
            builder.Services.AddScoped<VisitasService>();  // ✅ Nuevo servicio
            
            builder.Services.AddScoped<EstadoService>();
            builder.Services.AddScoped<MunicipioService>();
            builder.Services.AddScoped<LigaService>();
            builder.Services.AddScoped<TorneoService>();

            // ✅ SOLUCIÓN: Configuración mejorada del HttpClient
            builder.Services.AddScoped(sp => 
            {
                var handler = new HttpClientHandler();
                
#if ANDROID
                // Configuración específica para Android
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                handler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
#endif

                var httpClient = new HttpClient(handler)
                {
                    //apisfutbleandoMAUI.somee.com
                    // BaseAddress = new Uri("http://futboleandoapp.somee.com/"),
                    BaseAddress = new Uri("http://apisfutbleandoMAUI.somee.com/"),
                    Timeout = TimeSpan.FromSeconds(60) // ✅ Aumentar timeout para evitar cortes
                };

                // Headers recomendados
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "FutboleandoApp");

                return httpClient;
            });

            ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }
    }
}
