using futboleando.Service;
using futboleandoEntities.Juego;
using futboleandoEntities.Jornada;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Juego;

public partial class JuegoPage : ContentPage
{
    private readonly JuegoService juegoService;
    public ObservableCollection<JuegoListCLS> listajuegos { get; set; }
    public ObservableCollection<JornadaListCLS> listajornada { get; set; }
    public JornadaListCLS jornadaSeleccionada { get; set; }
    
    private List<JuegoListCLS> todosLosJuegos;
    private List<JuegoListCLS> juegosFiltrados;
    private int idTorneoSeleccionado;
    private bool _isLoading = false;
    private int _currentLoadedIndex = 0;
    private const int INITIAL_BATCH_SIZE = 10; // Primer lote: 10 items
    private const int INCREMENTAL_BATCH_SIZE = 15; // Lotes incrementales: 15 items

    public JuegoPage(JuegoService _juegoService)
    {
        InitializeComponent();
        juegoService = _juegoService;
        listajuegos = new ObservableCollection<JuegoListCLS>();
        listajornada = new ObservableCollection<JornadaListCLS>();
        todosLosJuegos = new List<JuegoListCLS>();
        juegosFiltrados = new List<JuegoListCLS>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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

            lblTorneoNombre.Text = $"Torneo: {nombreTorneo}";

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                return;
            }

            pickerJornada.SelectedIndexChanged -= OnJornadaSelected;

            var jornadas = await juegoService.ListarJornadasPorTorneo(idTorneoSeleccionado);
            listajornada.Clear();
            
            listajornada.Add(new JornadaListCLS 
            { 
                idjornada = 0, 
                nombre = "-- Todas las Jornadas --", 
                idtorneo = idTorneoSeleccionado 
            });
            
            foreach (var jornada in jornadas)
            {
                listajornada.Add(jornada);
            }
            
            // Cargar TODOS los juegos en memoria de una sola vez
            var juegos = await juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);
            todosLosJuegos = juegos.ToList();
            juegosFiltrados = todosLosJuegos;

            // Limpiar y cargar SOLO el primer lote
            listajuegos.Clear();
            _currentLoadedIndex = 0;
            CargarSiguienteLote();

            jornadaSeleccionada = listajornada.FirstOrDefault();
            pickerJornada.SelectedItem = jornadaSeleccionada;

            ActualizarContador();

            pickerJornada.SelectedIndexChanged += OnJornadaSelected;

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
        lblTotalJuegos.Text = $"Total de juegos: {juegosFiltrados.Count} (mostrando {listajuegos.Count})";
    }

    private async void OnVerMasClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int idJuego)
            {
                await DisplayAlert("Ver Más", 
                    $"Proximamente: Detalles del juego #{idJuego}\n\n" +
                    "Aquí se mostrará:\n" +
                    "• Alineaciones\n" +
                    "• Goleadores\n" +
                    "• Tarjetas\n" +
                    "• Estadísticas", 
                    "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }
    }
}
