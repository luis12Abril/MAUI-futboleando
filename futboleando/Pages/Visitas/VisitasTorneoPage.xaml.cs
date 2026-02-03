using futboleando.Service;
using futboleandoEntities.Visitas;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Visitas;

public partial class VisitasTorneoPage : ContentPage
{
    private readonly VisitasService visitasService;
    public ObservableCollection<VisitasTorneoCLS> listatorneos { get; set; }

    public VisitasTorneoPage(VisitasService _visitasService)
    {
        InitializeComponent();
        visitasService = _visitasService;
        listatorneos = new ObservableCollection<VisitasTorneoCLS>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            var totales = await visitasService.ObtenerVisitasTorneoTotales();
            lblTotalWeb.Text = totales.totalVisitasWeb.ToString("N0");
            lblTotalApp.Text = totales.totalVisitasApp.ToString("N0");

            var torneos = await visitasService.ObtenerVisitasPorTorneo();
            listatorneos = new ObservableCollection<VisitasTorneoCLS>(torneos);
            OnPropertyChanged(nameof(listatorneos));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar visitas: {ex.Message}", "OK");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
