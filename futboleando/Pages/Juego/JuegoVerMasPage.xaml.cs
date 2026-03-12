using futboleando.Service;
using futboleandoEntities.Comentario;
using futboleandoEntities.Juego;
using System.Globalization;
using Microsoft.Maui.Storage;

namespace futboleando.Pages.Juego;

public partial class JuegoVerMasPage : ContentPage
{
    private readonly JuegoService juegoService;
    private readonly ComentarioService comentarioService;
    private readonly int idJuego;

    public JuegoVerMasPage(JuegoService _juegoService, int _idJuego)
    {
        InitializeComponent();
        juegoService = _juegoService;
        comentarioService = MauiProgram.ServiceProvider.GetService<ComentarioService>();
        idJuego = _idJuego;
        _ = CargarDetallesJuego();
        ActualizarContadorComentario(0);
    }

    private async Task CargarDetallesJuego()
    {
        try
        {
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

            lblJornada.Text = detalles.nombrejornada?.ToUpper() ?? "JORNADA";

            if (detalles.fhorario.HasValue)
            {
                var cultura = new CultureInfo("es-ES");
                var fecha = detalles.fhorario.Value;

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

            lblNombreEquipo01.Text = detalles.nombreequipo01?.ToUpper() ?? "EQUIPO 1";
            lblGolesEquipo01.Text = detalles.golesequipo01?.ToString() ?? "0";

            if (!string.IsNullOrWhiteSpace(detalles.fotoequipo01))
            {
                System.Diagnostics.Debug.WriteLine("Foto equipo 1 disponible");
            }

            lblNombreEquipo02.Text = detalles.nombreequipo02?.ToUpper() ?? "EQUIPO 2";
            lblGolesEquipo02.Text = detalles.golesequipo02?.ToString() ?? "0";

            if (!string.IsNullOrWhiteSpace(detalles.fotoequipo02))
            {
                System.Diagnostics.Debug.WriteLine("Foto equipo 2 disponible");
            }

            lblCampo.Text = detalles.nombrecampo?.ToUpper() ?? "SIN ASIGNAR";
            lblArbitro.Text = detalles.nombrearbitro?.ToUpper() ?? "SIN ASIGNAR";

            lblTituloGolesEquipo01.Text = $"GOLES DE: {detalles.nombreequipo01?.ToUpper() ?? "EQUIPO 1"}";

            System.Diagnostics.Debug.WriteLine("===== EQUIPO 1 =====");
            System.Diagnostics.Debug.WriteLine($"ID Equipo: {detalles.idequipo01}");
            System.Diagnostics.Debug.WriteLine($"Goles totales: {detalles.golesequipo01}");
            System.Diagnostics.Debug.WriteLine($"Lista goles null? {detalles.golesEquipo01 == null}");
            System.Diagnostics.Debug.WriteLine($"Cantidad en lista: {detalles.golesEquipo01?.Count ?? 0}");

            var golesEquipo01 = detalles.golesEquipo01?
                .Where(g => g.habilitado.GetValueOrDefault(1) == 1)
                .ToList();

            if (golesEquipo01 != null && golesEquipo01.Count > 0)
            {
                foreach (var gol in golesEquipo01)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Jugador ID: {gol.idjugador}, Nombre: '{gol.nombrejugador}', Goles: {gol.goles}");
                }

                collectionGolesEquipo01.ItemsSource = golesEquipo01;
                collectionGolesEquipo01.IsVisible = true;
                lblNoGolesEquipo01.IsVisible = false;
            }
            else
            {
                collectionGolesEquipo01.IsVisible = false;
                lblNoGolesEquipo01.IsVisible = true;
                System.Diagnostics.Debug.WriteLine("❌ No hay goles en la lista del equipo 1");
            }

            lblTituloGolesEquipo02.Text = $"GOLES DE: {detalles.nombreequipo02?.ToUpper() ?? "EQUIPO 2"}";

            System.Diagnostics.Debug.WriteLine("===== EQUIPO 2 =====");
            System.Diagnostics.Debug.WriteLine($"ID Equipo: {detalles.idequipo02}");
            System.Diagnostics.Debug.WriteLine($"Goles totales: {detalles.golesequipo02}");
            System.Diagnostics.Debug.WriteLine($"Lista goles null? {detalles.golesEquipo02 == null}");
            System.Diagnostics.Debug.WriteLine($"Cantidad en lista: {detalles.golesEquipo02?.Count ?? 0}");

            var golesEquipo02 = detalles.golesEquipo02?
                .Where(g => g.habilitado.GetValueOrDefault(1) == 1)
                .ToList();

            if (golesEquipo02 != null && golesEquipo02.Count > 0)
            {
                foreach (var gol in golesEquipo02)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Jugador ID: {gol.idjugador}, Nombre: '{gol.nombrejugador}', Goles: {gol.goles}");
                }

                collectionGolesEquipo02.ItemsSource = golesEquipo02;
                collectionGolesEquipo02.IsVisible = true;
                lblNoGolesEquipo02.IsVisible = false;
            }
            else
            {
                collectionGolesEquipo02.IsVisible = false;
                lblNoGolesEquipo02.IsVisible = true;
                System.Diagnostics.Debug.WriteLine("❌ No hay goles en la lista del equipo 2");
            }

            System.Diagnostics.Debug.WriteLine("Detalles del juego cargados exitosamente");

            await CargarComentarios();
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

    private async Task CargarComentarios()
    {
        if (comentarioService == null)
        {
            return;
        }

        var comentarios = await comentarioService.ListarComentariosPorJuego(idJuego);
        collectionComentarios.ItemsSource = comentarios;
        lblNoComentarios.IsVisible = comentarios.Count == 0;

        var totalComentarios = comentarios.Count;
        var limiteAlcanzado = totalComentarios >= 30;

        lblComentariosInfo.Text = $"Máx. 100 caracteres · {totalComentarios}/30 comentarios";
        txtComentario.IsEnabled = !limiteAlcanzado;
        btnEnviarComentario.IsEnabled = !limiteAlcanzado;
    }

    private void OnComentarioTextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue == null)
        {
            ActualizarContadorComentario(0);
            return;
        }

        var texto = e.NewTextValue;
        if (texto.Length > 100)
        {
            texto = texto.Substring(0, 100);
            txtComentario.Text = texto;
        }

        ActualizarContadorComentario(texto.Length);
        btnEnviarComentario.IsEnabled = !string.IsNullOrWhiteSpace(texto);
    }

    private async void OnEnviarComentarioClicked(object sender, EventArgs e)
    {
        if (comentarioService == null)
        {
            return;
        }

        var texto = txtComentario.Text?.Trim();
        if (string.IsNullOrWhiteSpace(texto))
        {
            await DisplayAlert("Aviso", "Escribe un comentario antes de enviarlo.", "OK");
            return;
        }

        if (texto.Length > 100)
        {
            await DisplayAlert("Aviso", "El comentario no puede superar 100 caracteres.", "OK");
            return;
        }

        var idUsuario = Preferences.Get("IdUsuario", 0);
        if (idUsuario <= 0)
        {
            await DisplayAlert("Aviso", "Debes iniciar sesión para comentar.", "OK");
            return;
        }

        var comentario = new ComentarioCreateCLS
        {
            idjuego = idJuego,
            idusuario = idUsuario,
            comentario = texto
        };

        var resultado = await comentarioService.AgregarComentario(comentario);
        if (resultado == null)
        {
            await DisplayAlert("Error", "No se pudo enviar el comentario.", "OK");
            return;
        }

        txtComentario.Text = string.Empty;
        ActualizarContadorComentario(0);
        await CargarComentarios();
    }

    private void ActualizarContadorComentario(int longitud)
    {
        lblComentarioCount.Text = $"{longitud}/100";
    }
}
