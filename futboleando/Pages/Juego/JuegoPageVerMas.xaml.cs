using futboleando.Service;
using futboleandoEntities.Juego;
using System.Globalization;

namespace futboleando.Pages.Juego;

public partial class JuegoPageVerMas : ContentPage
{
    private readonly JuegoService juegoService;
    private readonly int idJuego;

    public JuegoPageVerMas(JuegoService _juegoService, int _idJuego)
    {
        InitializeComponent();
        juegoService = _juegoService;
        idJuego = _idJuego;
        
        _ = CargarDetallesJuego();
    }

    private async Task CargarDetallesJuego()
    {
        try
        {
            // ? Mostrar mensaje de depuración
            System.Diagnostics.Debug.WriteLine($"Cargando detalles del juego ID: {idJuego}");
            
            var detalles = await juegoService.ObtenerDetallesJuego(idJuego);

            if (detalles == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: detalles es null");
                await DisplayAlert("Error", "No se pudieron cargar los detalles del juego. Intente nuevamente.", "OK");
                await Navigation.PopAsync();
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Detalles cargados: {detalles.nombrejornada}");

            // ========== ENCABEZADO ==========
            lblJornada.Text = detalles.nombrejornada?.ToUpper() ?? "JORNADA";
            
            // Formatear fecha
            if (detalles.fhorario.HasValue)
            {
                var cultura = new CultureInfo("es-ES");
                var fecha = detalles.fhorario.Value;
                
                string diaSemana = cultura.TextInfo.ToTitleCase(fecha.ToString("dddd", cultura));
                string dia = fecha.ToString("dd");
                string mes = cultura.DateTimeFormat.GetAbbreviatedMonthName(fecha.Month).ToUpper();
                string anio = fecha.ToString("yyyy");
                string hora = fecha.ToString("h:mm tt", cultura).ToUpper();
                
                lblFechaHora.Text = $"{dia}/{mes}/{anio} {hora}";
            }
            else
            {
                lblFechaHora.Text = "Por confirmar";
            }

            lblEstatus.Text = detalles.nombreestatusjuego?.ToUpper() ?? "SIN ESTATUS";

            // ========== MARCADOR ==========
            // Equipo 1
            lblNombreEquipo01.Text = detalles.nombreequipo01?.ToUpper() ?? "EQUIPO 1";
            lblGolesEquipo01.Text = detalles.golesequipo01?.ToString() ?? "0";
            
            if (!string.IsNullOrWhiteSpace(detalles.fotoequipo01))
            {
                try
                {
                    var bytes = Convert.FromBase64String(detalles.fotoequipo01);
                    imgEquipo01.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    System.Diagnostics.Debug.WriteLine("Imagen equipo 1 cargada");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error cargando imagen equipo 1: {ex.Message}");
                }
            }

            // Equipo 2
            lblNombreEquipo02.Text = detalles.nombreequipo02?.ToUpper() ?? "EQUIPO 2";
            lblGolesEquipo02.Text = detalles.golesequipo02?.ToString() ?? "0";
            
            if (!string.IsNullOrWhiteSpace(detalles.fotoequipo02))
            {
                try
                {
                    var bytes = Convert.FromBase64String(detalles.fotoequipo02);
                    imgEquipo02.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                    System.Diagnostics.Debug.WriteLine("Imagen equipo 2 cargada");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error cargando imagen equipo 2: {ex.Message}");
                }
            }

            // ========== INFORMACIÓN DEL PARTIDO ==========
            lblCampo.Text = detalles.nombrecampo?.ToUpper() ?? "SIN ASIGNAR";
            lblArbitro.Text = detalles.nombrearbitro?.ToUpper() ?? "SIN ASIGNAR";

            // ========== GOLEADORES EQUIPO 1 ==========
            lblTituloGolesEquipo01.Text = $"GOLES DE: {detalles.nombreequipo01?.ToUpper() ?? "EQUIPO 1"}";
            
            System.Diagnostics.Debug.WriteLine($"===== EQUIPO 1 =====");
            System.Diagnostics.Debug.WriteLine($"ID Equipo: {detalles.idequipo01}");
            System.Diagnostics.Debug.WriteLine($"Goles totales: {detalles.golesequipo01}");
            System.Diagnostics.Debug.WriteLine($"Lista goles null? {detalles.golesEquipo01 == null}");
            System.Diagnostics.Debug.WriteLine($"Cantidad en lista: {detalles.golesEquipo01?.Count ?? 0}");
            
            if (detalles.golesEquipo01 != null && detalles.golesEquipo01.Count > 0)
            {
                foreach (var gol in detalles.golesEquipo01)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Jugador ID: {gol.idjugador}, Nombre: '{gol.nombrejugador}', Goles: {gol.goles}");
                }
                
                collectionGolesEquipo01.ItemsSource = detalles.golesEquipo01;
                collectionGolesEquipo01.IsVisible = true;
                lblNoGolesEquipo01.IsVisible = false;
            }
            else
            {
                collectionGolesEquipo01.IsVisible = false;
                lblNoGolesEquipo01.IsVisible = true;
                System.Diagnostics.Debug.WriteLine("?? No hay goles en la lista del equipo 1");
            }

            // ========== GOLEADORES EQUIPO 2 ==========
            lblTituloGolesEquipo02.Text = $"GOLES DE: {detalles.nombreequipo02?.ToUpper() ?? "EQUIPO 2"}";
            
            System.Diagnostics.Debug.WriteLine($"===== EQUIPO 2 =====");
            System.Diagnostics.Debug.WriteLine($"ID Equipo: {detalles.idequipo02}");
            System.Diagnostics.Debug.WriteLine($"Goles totales: {detalles.golesequipo02}");
            System.Diagnostics.Debug.WriteLine($"Lista goles null? {detalles.golesEquipo02 == null}");
            System.Diagnostics.Debug.WriteLine($"Cantidad en lista: {detalles.golesEquipo02?.Count ?? 0}");
            
            if (detalles.golesEquipo02 != null && detalles.golesEquipo02.Count > 0)
            {
                foreach (var gol in detalles.golesEquipo02)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Jugador ID: {gol.idjugador}, Nombre: '{gol.nombrejugador}', Goles: {gol.goles}");
                }
                
                collectionGolesEquipo02.ItemsSource = detalles.golesEquipo02;
                collectionGolesEquipo02.IsVisible = true;
                lblNoGolesEquipo02.IsVisible = false;
            }
            else
            {
                collectionGolesEquipo02.IsVisible = false;
                lblNoGolesEquipo02.IsVisible = true;
                System.Diagnostics.Debug.WriteLine("?? No hay goles en la lista del equipo 2");
            }
            
            System.Diagnostics.Debug.WriteLine("Detalles del juego cargados exitosamente");
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR HTTP: {httpEx.Message}");
            await DisplayAlert("Error de Conexión", 
                $"No se pudo conectar con el servidor.\n\nDetalles: {httpEx.Message}", 
                "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR GENERAL: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            await DisplayAlert("Error", 
                $"Error al cargar los detalles:\n\n{ex.Message}", 
                "OK");
            await Navigation.PopAsync();
        }
    }
}