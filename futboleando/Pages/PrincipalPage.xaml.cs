using futboleando.Pages.Ciudad;
using futboleando.Pages.Comunicado;
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

    private async void OnComunicadosTapped(object sender, EventArgs e)
    {
        var comunicadoService = MauiProgram.ServiceProvider.GetService<ComunicadoService>();
        if (comunicadoService != null)
        {
            var comunicadoPage = new ComunicadoPage(comunicadoService);
            await App.Navigate.PushAsync(comunicadoPage);
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

    private async void OnJugadoresTapped(object sender, EventArgs e)
    {
        var jugadorService = MauiProgram.ServiceProvider.GetService<JugadorService>();
        if (jugadorService != null)
        {
            var jugadorPage = new JugadorPage(jugadorService);
            await App.Navigate.PushAsync(jugadorPage);
        }
    }

    private async void OnCiudadesTapped(object sender, EventArgs e)
    {
        var ciudadService = MauiProgram.ServiceProvider.GetService<CiudadService>();
        if (ciudadService != null)
        {
            var ciudadPage = new CiudadPage(ciudadService);
            await App.Navigate.PushAsync(ciudadPage);
        }
    }
}