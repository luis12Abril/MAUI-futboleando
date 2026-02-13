using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Juego;

namespace futboleando.Pages.Juego;

public partial class UltimosCincoJuegosPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    private readonly JuegoService juegoService;
    private bool datosYaCargados;
    private bool _isLoading;
    private bool _isNavigatingBack;
    private int _ultimoTorneoCargado;
    private List<UltimosCincoJuegosEquipoModel> _todosUltimosCinco = new();
    private int _currentLoadedIndex;
    private const int InitialBatchSize = 12;
    private const int IncrementalBatchSize = 20;

    private ObservableCollection<UltimosCincoJuegosEquipoModel> _listaUltimosCinco;
    public ObservableCollection<UltimosCincoJuegosEquipoModel> ListaUltimosCinco
    {
        get => _listaUltimosCinco;
        set
        {
            _listaUltimosCinco = value;
            OnPropertyChanged(nameof(ListaUltimosCinco));
        }
    }

    public UltimosCincoJuegosPage(EquipoService _equipoService, JuegoService _juegoService)
    {
        InitializeComponent();
        equipoService = _equipoService;
        juegoService = _juegoService;
        ListaUltimosCinco = new ObservableCollection<UltimosCincoJuegosEquipoModel>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _ = CargarDatos();
    }

    private async Task CargarDatos()
    {
        try
        {
            if (_isLoading)
            {
                return;
            }

            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            await Task.Yield();

            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            lblTorneoNombre.Text = $"Torneo: {nombreTorneo}";

            if (datosYaCargados && _ultimoTorneoCargado == idTorneoSeleccionado)
            {
                return;
            }

            _isLoading = true;

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                ListaUltimosCinco.Clear();
                _todosUltimosCinco.Clear();
                _currentLoadedIndex = 0;
                lblTotalEquipos.Text = "Total equipos: 0";
                return;
            }

            var stopwatchApi = Stopwatch.StartNew();

            var equiposTask = equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);
            var juegosTask = juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);

            await Task.WhenAll(equiposTask, juegosTask);

            stopwatchApi.Stop();

            var stopwatchUi = Stopwatch.StartNew();

            var equipos = equiposTask.Result;
            var juegos = juegosTask.Result;

            var ultimosCinco = await Task.Run(() =>
            {
                var juegosJugados = juegos
                    .Where(j => !string.IsNullOrWhiteSpace(j.nombreestatusjuego)
                        && j.nombreestatusjuego.Trim().Contains("JUGADO", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var juegosPorEquipo = new Dictionary<int, List<JuegoListCLS>>();

                foreach (var juego in juegosJugados)
                {
                    if (!juegosPorEquipo.TryGetValue(juego.idequipo01, out var listaEquipo1))
                    {
                        listaEquipo1 = new List<JuegoListCLS>();
                        juegosPorEquipo[juego.idequipo01] = listaEquipo1;
                    }

                    listaEquipo1.Add(juego);

                    if (!juegosPorEquipo.TryGetValue(juego.idequipo02, out var listaEquipo2))
                    {
                        listaEquipo2 = new List<JuegoListCLS>();
                        juegosPorEquipo[juego.idequipo02] = listaEquipo2;
                    }

                    listaEquipo2.Add(juego);
                }

                foreach (var listaJuegos in juegosPorEquipo.Values)
                {
                    listaJuegos.Sort((a, b) => (b.fhorario ?? DateTime.MinValue).CompareTo(a.fhorario ?? DateTime.MinValue));
                }

                var equiposOrdenados = equipos
                    .OrderByDescending(e => (e.puntos ?? 0) + (e.puntosextras ?? 0))
                    .ThenBy(e => e.nombre)
                    .ToList();

                var lista = new List<UltimosCincoJuegosEquipoModel>(equiposOrdenados.Count);

                foreach (var equipo in equiposOrdenados)
                {
                    juegosPorEquipo.TryGetValue(equipo.idequipo, out var juegosEquipo);

                    var resultados = new string[5] { "-", "-", "-", "-", "-" };

                    if (juegosEquipo is { Count: > 0 })
                    {
                        var cantidad = Math.Min(5, juegosEquipo.Count);

                        for (var i = 0; i < cantidad; i++)
                        {
                            resultados[i] = ObtenerResultadoEquipo(juegosEquipo[i], equipo.idequipo);
                        }
                    }

                    lista.Add(new UltimosCincoJuegosEquipoModel
                    {
                        EquipoNombre = equipo.nombre ?? string.Empty,
                        Puntos = (equipo.puntos ?? 0) + (equipo.puntosextras ?? 0),
                        Ultimo = resultados[0],
                        Juego2 = resultados[1],
                        Juego3 = resultados[2],
                        Juego4 = resultados[3],
                        Juego5 = resultados[4]
                    });
                }

                return lista;
            });

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ListaUltimosCinco.Clear();
                _todosUltimosCinco = ultimosCinco;
                _currentLoadedIndex = 0;
                CargarSiguienteLote();
                lblTotalEquipos.Text = $"Total equipos: {_todosUltimosCinco.Count}";
            });

            stopwatchUi.Stop();
            _ultimoTorneoCargado = idTorneoSeleccionado;
            datosYaCargados = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error al cargar últimos cinco: {ex.Message}");
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
        }
        finally
        {
            _isLoading = false;
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private static string ObtenerResultadoEquipo(JuegoListCLS juego, int idEquipo)
    {
        if (juego.idequipo01 == idEquipo)
        {
            return NormalizarResultado(juego.resequipo01, juego.golesequipo01, juego.golesequipo02);
        }

        if (juego.idequipo02 == idEquipo)
        {
            return NormalizarResultado(juego.resequipo02, juego.golesequipo02, juego.golesequipo01);
        }

        return "-";
    }

    private void CargarSiguienteLote()
    {
        if (_currentLoadedIndex >= _todosUltimosCinco.Count)
        {
            return;
        }

        var batchSize = _currentLoadedIndex == 0 ? InitialBatchSize : IncrementalBatchSize;
        var itemsToLoad = Math.Min(batchSize, _todosUltimosCinco.Count - _currentLoadedIndex);

        for (var i = 0; i < itemsToLoad; i++)
        {
            ListaUltimosCinco.Add(_todosUltimosCinco[_currentLoadedIndex]);
            _currentLoadedIndex++;
        }
    }

    private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
    {
        if (_currentLoadedIndex < _todosUltimosCinco.Count)
        {
            CargarSiguienteLote();
        }
    }

    private static string NormalizarResultado(string? resultado, int? golesFavor, int? golesContra)
    {
        if (!string.IsNullOrWhiteSpace(resultado))
        {
            return resultado.Trim();
        }

        if (golesFavor.HasValue && golesContra.HasValue)
        {
            if (golesFavor > golesContra)
            {
                return "G";
            }

            if (golesFavor < golesContra)
            {
                return "P";
            }

            return "E";
        }

        return "-";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_isNavigatingBack)
        {
            return;
        }

        try
        {
            _isNavigatingBack = true;

            if (Navigation?.NavigationStack?.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error al regresar: {ex.Message}");
        }
        finally
        {
            _isNavigatingBack = false;
        }
    }
}
