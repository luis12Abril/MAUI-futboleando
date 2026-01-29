using futboleando.Pages.Juego;
using futboleando.Pages.Goleador;
using futboleando.Pages.Posiciones;
using futboleando.Service;

namespace futboleando.Pages;

public partial class PrincipalPage : ContentPage
{
    public PrincipalPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // ? Cargar el nombre del torneo seleccionado
        CargarNombreTorneo();
    }

    private void CargarNombreTorneo()
    {
        // ? Obtener el nombre del torneo guardado en Preferences
        var nombreTorneo = Preferences.Get("NombreTorneo", string.Empty);
        
        if (!string.IsNullOrWhiteSpace(nombreTorneo))
        {
            // ? Solo el nombre del torneo, sin ícono
            lblTorneoSeleccionado.Text = nombreTorneo;
        }
        else
        {
            lblTorneoSeleccionado.Text = "Sin torneo seleccionado";
        }
    }

    private async void OnJuegosTapped(object sender, EventArgs e)
    {
        var juegoService = MauiProgram.ServiceProvider.GetService<JuegoService>();
        if (juegoService != null)
        {
            var juegoPage = new JuegoPage(juegoService);
            await App.Navigate.PushAsync(juegoPage);
        }
    }

    private async void OnPosicionesTapped(object sender, EventArgs e)
    {
        var equipoService = MauiProgram.ServiceProvider.GetService<EquipoService>();
        if (equipoService != null)
        {
            var posicionesPage = new PosicionesPage(equipoService);
            await App.Navigate.PushAsync(posicionesPage);
        }
    }

    private async void OnEquiposTapped(object sender, EventArgs e)
    {
        var equipoService = MauiProgram.ServiceProvider.GetService<EquipoService>();
        if (equipoService != null)
        {
            var equipoPage = new EquipoPage(equipoService);
            await App.Navigate.PushAsync(equipoPage);
        }
    }

    private async void OnGoleadoresTapped(object sender, EventArgs e)
    {
        var goleadorService = MauiProgram.ServiceProvider.GetService<GoleadorService>();
        var equipoService = MauiProgram.ServiceProvider.GetService<EquipoService>();
        
        if (goleadorService != null && equipoService != null)
        {
            var goleadorPage = new GoleadorPage(goleadorService, equipoService);
            await App.Navigate.PushAsync(goleadorPage);
        }
    }
}