using System.Collections.ObjectModel;
using System.ComponentModel;
using futboleando.Service;
using futboleandoEntities.Juego;
using Microsoft.Maui.Storage;

namespace futboleando.Pages.Equipo;

public partial class EquipoVerMasPage : ContentPage, INotifyPropertyChanged
{
    private readonly JuegoService juegoService;
    private readonly int idEquipo;
    private bool datosCargados;

    private ObservableCollection<JuegoListCLS> _listajuegos;
    private string _nombreEquipoSeleccionado = "";
    private string _totalJuegosTexto = "Total de juegos: 0";

    public ObservableCollection<JuegoListCLS> listajuegos
    {
        get => _listajuegos;
        set
        {
            _listajuegos = value;
            OnPropertyChanged(nameof(listajuegos));
        }
    }

    public string NombreEquipoSeleccionado
    {
        get => _nombreEquipoSeleccionado;
        set
        {
            _nombreEquipoSeleccionado = value;
            OnPropertyChanged(nameof(NombreEquipoSeleccionado));
        }
    }

    public string TotalJuegosTexto
    {
        get => _totalJuegosTexto;
        set
        {
            _totalJuegosTexto = value;
            OnPropertyChanged(nameof(TotalJuegosTexto));
        }
    }

    public EquipoVerMasPage(JuegoService _juegoService, int _idEquipo, string nombreEquipo)
    {
        InitializeComponent();
        juegoService = _juegoService;
        idEquipo = _idEquipo;
        listajuegos = new ObservableCollection<JuegoListCLS>();
        NombreEquipoSeleccionado = nombreEquipo?.Trim() ?? "Equipo";
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!datosCargados)
        {
            await CargarJuegosEquipo();
            datosCargados = true;
        }
    }

    private async Task CargarJuegosEquipo()
    {
        loadingIndicator.IsRunning = true;
        loadingIndicator.IsVisible = true;

        try
        {
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                return;
            }

            var juegos = await juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);
            var juegosEquipo = juegos
                .Where(j => j.idequipo01 == idEquipo || j.idequipo02 == idEquipo)
                .OrderByDescending(j => j.fhorario ?? DateTime.MinValue)
                .ToList();

            listajuegos = new ObservableCollection<JuegoListCLS>(juegosEquipo);
            TotalJuegosTexto = $"Total de juegos: {listajuegos.Count}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar juegos: {ex.Message}", "OK");
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
