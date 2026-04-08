using futboleando.Service;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

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
            builder.Services.AddScoped<ComentarioService>();
            builder.Services.AddScoped<GoleadorService>();
            builder.Services.AddScoped<JugadoresPorAñoService>();
            builder.Services.AddScoped<VisitasService>();  // ✅ Nuevo servicio
            builder.Services.AddScoped<CumpleañeroService>();
            builder.Services.AddScoped<AvisoFutboleandoService>();
            
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

                //  BaseAddress = new Uri("http://futboleandoapp.somee.com/"),
                //  BaseAddress = new Uri("http://futboleando2026.somee.com/"),
                //  BaseAddress = new Uri("http://luisbarreras-001-site1.site4future.com/"),
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("http://bato1970-001-site5.jtempurl.com/"),
                    Timeout = TimeSpan.FromSeconds(90)
                };

                // Headers recomendados
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "FutboleandoApp");


                // Credenciales temporales para hosting gratuito con Password Protection
                // var usuarioHosting = "11301142";
                // var passwordHosting = "60-dayfreetrial";
                // var basicAuthToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{usuarioHosting}:{passwordHosting}"));
                // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);

                return httpClient;
            });

            ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }
    }
}
