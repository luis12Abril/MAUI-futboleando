using System.Collections.ObjectModel;
using System.Linq;
using futboleando.Service;
using futboleandoEntities.Juego;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

namespace futboleando.Pages.Goleador;

public partial class GoleadorVerMasPage : ContentPage
{
    private readonly JuegoService? juegoService;
    private readonly int idJugador;
    private bool datosCargados;

    private ObservableCollection<JuegoGolesJugadorCLS> _listajuegos;
    private string _totalJuegosTexto = "Total de juegos con gol: 0";
    private string _totalGolesTexto = "Total de goles: 0";
    private bool _isLoading;
    private string _nombreJugador = "Goles del jugador";

    public ObservableCollection<JuegoGolesJugadorCLS> listajuegos
    {
        get => _listajuegos;
        set
        {
            _listajuegos = value;
            OnPropertyChanged(nameof(listajuegos));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public string NombreJugador
    {
        get => _nombreJugador;
        set
        {
            _nombreJugador = value;
            OnPropertyChanged(nameof(NombreJugador));
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

    public string TotalGolesTexto
    {
        get => _totalGolesTexto;
        set
        {
            _totalGolesTexto = value;
            OnPropertyChanged(nameof(TotalGolesTexto));
        }
    }

    public GoleadorVerMasPage(int idJugador, string nombreJugador)
    {
        InitializeComponent();
        this.idJugador = idJugador;
        juegoService = MauiProgram.ServiceProvider.GetService<JuegoService>();
        listajuegos = new ObservableCollection<JuegoGolesJugadorCLS>();
        NombreJugador = string.IsNullOrWhiteSpace(nombreJugador) ? "Goles del jugador" : nombreJugador.Trim();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!datosCargados)
        {
            await CargarJuegosConGol();
            datosCargados = true;
        }
    }

    private async Task CargarJuegosConGol()
    {
        IsLoading = true;

        try
        {
            if (juegoService == null)
            {
                await DisplayAlert("Error", "No se pudo cargar el servicio de juegos.", "OK");
                return;
            }

            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                return;
            }

            var juegos = await juegoService.ListarJuegosConGolesJugador(idTorneoSeleccionado, idJugador);
            var juegosOrdenados = juegos
                .Where(j => string.Equals(j.nombreestatusjuego?.Trim(), "JUGADO", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(j => j.fhorario ?? DateTime.MinValue)
                .ToList();

            listajuegos = new ObservableCollection<JuegoGolesJugadorCLS>(juegosOrdenados);
            TotalJuegosTexto = $"Total de juegos con gol: {listajuegos.Count}";
            TotalGolesTexto = $"Total de goles: {juegosOrdenados.Sum(j => j.golesjugador)}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar juegos: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
