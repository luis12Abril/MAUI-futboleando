using futboleando.Pages.GoleadorVerMas;
using futboleando.Service;
using futboleandoEntities.Goleador;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Goleador;

public partial class GoleadorPage : ContentPage
{
    private readonly GoleadorService goleadorService;
    public ObservableCollection<GoleadorCLS> listagoleadores { get; set; }
    private int idTorneoSeleccionado;

    public GoleadorPage(GoleadorService _goleadorService)
    {
        InitializeComponent();
        goleadorService = _goleadorService;
        listagoleadores = new ObservableCollection<GoleadorCLS>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarGoleadores();
    }

    private async Task CargarGoleadores()
    {
        try
        {
            // Mostrar indicador de carga
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            // Obtener torneo seleccionado
            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");

            lblTorneoNombre.Text = $"Torneo: {nombreTorneo}";

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                return;
            }

            // Cargar goleadores del torneo
            var goleadores = await goleadorService.ListarGoleadoresPorTorneo(idTorneoSeleccionado);

            listagoleadores.Clear();
            foreach (var goleador in goleadores)
            {
                listagoleadores.Add(goleador);
            }

            // Actualizar contador
            lblTotalGoleadores.Text = $"Total de goleadores: {listagoleadores.Count}";

            // Ocultar indicador de carga
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar goleadores: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void OnVerMasClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            if (button?.CommandParameter is int idJugador)
            {
                // Navegar a la página de detalles del goleador
                var goleadorVerMasPage = new GoleadorVerMasPage(idJugador);
                await Navigation.PushAsync(goleadorVerMasPage);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
        }
    }
}
