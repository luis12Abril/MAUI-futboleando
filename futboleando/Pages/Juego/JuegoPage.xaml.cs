using futboleando.Service;
using futboleandoEntities.Juego;
using futboleandoEntities.Jornada;
using futboleandoEntities.Equipo;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace futboleando.Pages.Juego;

public partial class JuegoPage : ContentPage, INotifyPropertyChanged
{
    private readonly JuegoService juegoService;
    private readonly EquipoService equipoService;
    public ObservableCollection<JuegoListCLS> listajuegos { get; set; }
    public ObservableCollection<JornadaListCLS> listajornada { get; set; }
    public JornadaListCLS jornadaSeleccionada { get; set; }

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
    
    private List<JuegoListCLS> todosLosJuegos;
    private List<JuegoListCLS> juegosFiltrados;
    private int idTorneoSeleccionado;
    private bool _isLoading = false;
    private bool datosYaCargados;
    private int _ultimoTorneoCargado;
    private int? _ultimoJuegoSeleccionadoId;
    private int _currentLoadedIndex = 0;
    private const int INITIAL_BATCH_SIZE = 10; // Primer lote: 10 items
    private const int INCREMENTAL_BATCH_SIZE = 15; // Lotes incrementales: 15 items

    public JuegoPage(JuegoService _juegoService)
    {
        InitializeComponent();
        juegoService = _juegoService;
        equipoService = MauiProgram.ServiceProvider.GetService<EquipoService>();
        listajuegos = new ObservableCollection<JuegoListCLS>();
        listajornada = new ObservableCollection<JornadaListCLS>();
        todosLosJuegos = new List<JuegoListCLS>();
        juegosFiltrados = new List<JuegoListCLS>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (pickerJornada != null)
        {
            pickerJornada.IsEnabled = false;
            pickerJornada.Unfocus();

            Device.StartTimer(TimeSpan.FromMilliseconds(150), () =>
            {
                pickerJornada.IsEnabled = true;
                pickerJornada.Unfocus();
                return false;
            });
        }
        var torneoActual = Preferences.Get("UltimoTorneo", 0);

        if (datosYaCargados && _ultimoTorneoCargado == torneoActual)
        {
            ScrollToJuegoSeleccionado();
            return;
        }

        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        if (_isLoading) return;
        
        _isLoading = true;
        
        try
        {
            // Mostrar indicador de carga principal
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            NombreTorneoSeleccionado = nombreTorneo;

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                datosYaCargados = false;
                return;
            }

            pickerJornada.SelectedIndexChanged -= OnJornadaSelected;

            var jornadas = await juegoService.ListarJornadasPorTorneo(idTorneoSeleccionado);
            var jornadasFiltradas = jornadas
                .Where(j => !string.Equals(j.nombre?.Trim(), "-- Todas las Jornadas --", StringComparison.OrdinalIgnoreCase))
                .OrderBy(j => j.finiciojornada)
                .ToList();
            listajornada = new ObservableCollection<JornadaListCLS>(jornadasFiltradas);
            OnPropertyChanged(nameof(listajornada));
            
            // Cargar TODOS los juegos en memoria de una sola vez
            var juegos = await juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);
            todosLosJuegos = juegos.ToList();

            ObservableCollection<EquipoListCLS> equipos = new();
            if (equipoService != null)
            {
                equipos = await equipoService.listarEquipoPorTorneoResumen(idTorneoSeleccionado);
            }

            NormalizarNombresEquipos(todosLosJuegos, equipos);
            juegosFiltrados = todosLosJuegos;

            // Limpiar y cargar SOLO el primer lote
            listajuegos.Clear();
            _currentLoadedIndex = 0;
            CargarSiguienteLote();

            jornadaSeleccionada = null;
            pickerJornada.SelectedIndex = -1;

            ActualizarContador();

            pickerJornada.SelectedIndexChanged += OnJornadaSelected;

            datosYaCargados = true;
            _ultimoTorneoCargado = idTorneoSeleccionado;

            ScrollToJuegoSeleccionado();

            // Ocultar indicador de carga principal
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }

        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar juegos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
        finally
        {
            _isLoading = false;
        }

    }

    private static void NormalizarNombresEquipos(List<JuegoListCLS> juegos, IEnumerable<EquipoListCLS> equipos)
    {
        if (juegos == null || juegos.Count == 0)
        {
            return;
        }

        var nombresPorEquipo = new Dictionary<int, string>();

        if (equipos != null)
        {
            foreach (var equipo in equipos)
            {
                if (equipo.idequipo > 0 && !string.IsNullOrWhiteSpace(equipo.nombre))
                {
                    nombresPorEquipo[equipo.idequipo] = equipo.nombre.Trim();
                }
            }
        }

        foreach (var juego in juegos)
        {
            if (juego.idequipo01 > 0 && !string.IsNullOrWhiteSpace(juego.nombreequipo01))
            {
                nombresPorEquipo[juego.idequipo01] = juego.nombreequipo01.Trim();
            }

            if (juego.idequipo02 > 0 && !string.IsNullOrWhiteSpace(juego.nombreequipo02))
            {
                nombresPorEquipo[juego.idequipo02] = juego.nombreequipo02.Trim();
            }
        }

        foreach (var juego in juegos)
        {
            juego.nombreequipo01 = ResolverNombreEquipo(juego.idequipo01, juego.nombreequipo01, nombresPorEquipo);
            juego.nombreequipo02 = ResolverNombreEquipo(juego.idequipo02, juego.nombreequipo02, nombresPorEquipo);
        }
    }

    private static string ResolverNombreEquipo(int idEquipo, string nombreActual, IReadOnlyDictionary<int, string> nombresPorEquipo)
    {
        if (idEquipo > 0 && nombresPorEquipo.TryGetValue(idEquipo, out var nombreCatalogo))
        {
            return nombreCatalogo;
        }

        var nombre = nombreActual?.Trim();
        return string.IsNullOrWhiteSpace(nombre) ? "SIN EQUIPO" : nombre;
    }

    private void OnLimpiarFiltroClicked(object sender, EventArgs e)
    {
        if (listajornada == null || listajornada.Count == 0)
        {
            return;
        }

        jornadaSeleccionada = null;
        pickerJornada.SelectedIndex = -1;
        _ = FiltrarJuegosAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Método para cargar el siguiente lote de items
    private void CargarSiguienteLote()
    {
        if (_currentLoadedIndex >= juegosFiltrados.Count)
            return;

        int batchSize = (_currentLoadedIndex == 0) ? INITIAL_BATCH_SIZE : INCREMENTAL_BATCH_SIZE;
        int itemsToLoad = Math.Min(batchSize, juegosFiltrados.Count - _currentLoadedIndex);

        for (int i = 0; i < itemsToLoad; i++)
        {
            listajuegos.Add(juegosFiltrados[_currentLoadedIndex]);
            _currentLoadedIndex++;
        }

        ActualizarContador();
    }

    // Evento cuando se acerca al final de la lista
    private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (footerLoadingIndicator.IsRunning)
            return;

        if (_currentLoadedIndex < juegosFiltrados.Count)
        {
            footerLoadingIndicator.IsRunning = true;
            footerLoadingIndicator.IsVisible = true;

            // Pequeño delay para mostrar el indicador
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                CargarSiguienteLote();
                
                footerLoadingIndicator.IsRunning = false;
                footerLoadingIndicator.IsVisible = false;
                
                return false;
            });
        }
    }

    private async void OnJornadaSelected(object sender, EventArgs e)
    {
        if (_isLoading) return;

        await FiltrarJuegosAsync();
    }

    private async Task FiltrarJuegosAsync()
    {
        try
        {
            // Mostrar indicador mientras filtra
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            await Task.Run(() =>
            {
                // Filtrar en background thread
                if (jornadaSeleccionada == null || jornadaSeleccionada.idjornada == 0)
                {
                    juegosFiltrados = todosLosJuegos;
                }
                else
                {
                    juegosFiltrados = todosLosJuegos
                        .Where(j => j.idjornada == jornadaSeleccionada.idjornada)
                        .ToList();
                }
            });

            // Actualizar UI en el main thread
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listajuegos.Clear();
                _currentLoadedIndex = 0;
                CargarSiguienteLote();
                
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            });

            ScrollToJuegoSeleccionado();
            datosYaCargados = true;
            _ultimoTorneoCargado = idTorneoSeleccionado;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al filtrar juegos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private void ActualizarContador()
    {
        lblTotalJuegos.Text = $"Total de juegos: {juegosFiltrados.Count}";
    }

    private async void OnVerMasClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int idJuego)
            {
                _ultimoJuegoSeleccionadoId = idJuego;
                await Navigation.PushAsync(new JuegoVerMasPage(juegoService, idJuego));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }
    }

    private void ScrollToJuegoSeleccionado()
    {
        if (_ultimoJuegoSeleccionadoId is null || juegosFiltrados.Count == 0)
        {
            return;
        }

        var index = juegosFiltrados.FindIndex(j => j.idjuego == _ultimoJuegoSeleccionadoId.Value);
        if (index < 0)
        {
            return;
        }

        while (_currentLoadedIndex <= index)
        {
            CargarSiguienteLote();
        }

        var item = listajuegos.FirstOrDefault(j => j.idjuego == _ultimoJuegoSeleccionadoId.Value);
        if (item != null)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                collectionJuegos.ScrollTo(item, position: ScrollToPosition.Center, animate: false);
            });
        }
    }
}
