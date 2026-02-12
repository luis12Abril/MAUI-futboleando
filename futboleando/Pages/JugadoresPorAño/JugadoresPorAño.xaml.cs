using futboleando.Service;
using futboleandoEntities.JugadoresPorAño;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace futboleando.Pages.JugadoresPorAño;

public partial class JugadoresPorAñoPage : ContentPage, INotifyPropertyChanged
{
    private readonly JugadoresPorAñoService jugadoresPorAñoService;
    private ObservableCollection<JugadoresPorAñoCLS> _listajugadoresporaño;
    public ObservableCollection<JugadoresPorAñoCLS> listajugadoresporaño
    {
        get => _listajugadoresporaño;
        set
        {
            _listajugadoresporaño = value;
            OnPropertyChanged(nameof(listajugadoresporaño));
        }
    }

    private ObservableCollection<EquipoSimpleCLS> _listaequipos;
    public ObservableCollection<EquipoSimpleCLS> listaequipos
    {
        get => _listaequipos;
        set
        {
            _listaequipos = value;
            OnPropertyChanged(nameof(listaequipos));
        }
    }

    public EquipoSimpleCLS equipoSeleccionado { get; set; }
    private int idTorneoSeleccionado;
    private bool _isLoading;
    private bool _datosCargados;

    private string _nombreTorneoSeleccionado = "";
    public string NombreTorneoSeleccionado
    {
        get => _nombreTorneoSeleccionado;
        set
        {
            _nombreTorneoSeleccionado = value;
            OnPropertyChanged(nameof(NombreTorneoSeleccionado));
        }
    }

    public JugadoresPorAñoPage(JugadoresPorAñoService _jugadoresPorAñoService)
    {
        InitializeComponent();
        jugadoresPorAñoService = _jugadoresPorAñoService;
        listajugadoresporaño = new ObservableCollection<JugadoresPorAñoCLS>();
        listaequipos = new ObservableCollection<EquipoSimpleCLS>();
        BindingContext = this;
        _ = CargarDatos();
    }

    private async Task CargarDatos()
    {
        if (_isLoading)
        {
            return;
        }

        if (_datosCargados)
        {
            return;
        }

        _isLoading = true;

        try
        {
            // Mostrar indicador de carga
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            // Obtener torneo seleccionado
            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            NombreTorneoSeleccionado = nombreTorneo;

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                return;
            }

            pickerEquipo.SelectedIndexChanged -= OnEquipoSelected;

            var equiposTask = jugadoresPorAñoService.ListarEquiposPorTorneo(idTorneoSeleccionado);
            var stopwatch = Stopwatch.StartNew();
            var jugadoresTask = jugadoresPorAñoService.ListarJugadoresPorAño(idTorneoSeleccionado, null);

            await Task.WhenAll(equiposTask, jugadoresTask);
            stopwatch.Stop();

            var equipos = equiposTask.Result;
            var jugadores = jugadoresTask.Result;
            var total = jugadores.Sum(j => j.cantidad);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listaequipos = new ObservableCollection<EquipoSimpleCLS>(equipos);
                equipoSeleccionado = null;
                pickerEquipo.SelectedIndex = -1;
                listajugadoresporaño = new ObservableCollection<JugadoresPorAñoCLS>(jugadores);
                lblTotalJugadores.Text = total.ToString();
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            });

            pickerEquipo.SelectedIndexChanged += OnEquipoSelected;

            _datosCargados = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task CargarJugadoresPorAño(int? idEquipo)
    {
        try
        {
            // Mostrar indicador de carga
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            // Obtener jugadores agrupados por año
            var stopwatch = Stopwatch.StartNew();
            var jugadores = await jugadoresPorAñoService.ListarJugadoresPorAño(idTorneoSeleccionado, idEquipo);
            stopwatch.Stop();
            var total = jugadores.Sum(j => j.cantidad);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listajugadoresporaño = new ObservableCollection<JugadoresPorAñoCLS>(jugadores);
                lblTotalJugadores.Text = total.ToString();
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            });

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar jugadores: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void OnEquipoSelected(object sender, EventArgs e)
    {
        try
        {
            var picker = sender as Picker;
            if (picker == null || picker.SelectedIndex == -1)
            {
                await CargarJugadoresPorAño(null);
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoSimpleCLS;
            if (equipoSeleccionado == null) return;

            // Si es "Todos los equipos" (id = 0), pasar null
            int? idEquipo = equipoSeleccionado.idequipo == 0 ? null : equipoSeleccionado.idequipo;
            
            await CargarJugadoresPorAño(idEquipo);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al filtrar: {ex.Message}", "OK");
        }
    }

    private void OnLimpiarFiltroClicked(object sender, EventArgs e)
    {
        pickerEquipo.SelectedIndex = -1;
        _ = CargarJugadoresPorAño(null);
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
