using futboleando.Pages.GoleadorVerMas;
using futboleando.Service;
using futboleandoEntities.Goleador;
using futboleandoEntities.Equipo;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace futboleando.Pages.Goleador;

public partial class GoleadorPage : ContentPage
{
    private readonly GoleadorService goleadorService;
    private readonly EquipoService equipoService;
    public ObservableCollection<GoleadorCLS> listagoleadores { get; set; }
    public ObservableCollection<EquipoListCLS> listaequipos { get; set; }
    public EquipoListCLS equipoSeleccionado { get; set; }
    private int idTorneoSeleccionado;
    private List<GoleadorCLS> todosLosGoleadores; // Lista completa para filtrar
    private bool datosYaCargados = false; // Flag para evitar recargas innecesarias

    public GoleadorPage(GoleadorService _goleadorService, EquipoService _equipoService)
    {
        InitializeComponent();
        goleadorService = _goleadorService;
        equipoService = _equipoService;
        listagoleadores = new ObservableCollection<GoleadorCLS>();
        listaequipos = new ObservableCollection<EquipoListCLS>();
        todosLosGoleadores = new List<GoleadorCLS>();
        BindingContext = this;
        
        // Mostrar indicador de carga inmediatamente
        loadingIndicator.IsRunning = true;
        loadingIndicator.IsVisible = true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Solo cargar si no se han cargado los datos previamente
        if (!datosYaCargados)
        {
            // OPTIMIZACIÓN CRÍTICA: Permitir que la UI se renderice primero
            await Task.Delay(50); // Dar tiempo a que se muestre la página
            
            // Cargar equipos y goleadores en paralelo
            var tareaEquipos = CargarEquipos();
            var tareaGoleadores = CargarGoleadores();
            
            // Esperar ambas tareas
            await Task.WhenAll(tareaEquipos, tareaGoleadores);
            
            datosYaCargados = true;
        }
    }

    private async Task CargarEquipos()
    {
        try
        {
            // Obtener torneo seleccionado
            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            if (idTorneoSeleccionado == 0)
            {
                return;
            }

            // Cargar equipos del torneo
            var equipos = await equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);

            // Recrear la ObservableCollection (más rápido que Clear + foreach)
            listaequipos = new ObservableCollection<EquipoListCLS>(equipos);
            OnPropertyChanged(nameof(listaequipos));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar equipos: {ex.Message}", "OK");
        }
    }

    private async Task CargarGoleadores()
    {
        try
        {
            var startTime = DateTime.Now;
            
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

            System.Diagnostics.Debug.WriteLine($"[GOLEADORES] Iniciando carga... {(DateTime.Now - startTime).TotalMilliseconds}ms");

            // Cargar goleadores del torneo
            var goleadores = await goleadorService.ListarGoleadoresPorTorneo(idTorneoSeleccionado);
            
            System.Diagnostics.Debug.WriteLine($"[GOLEADORES] API respondió en {(DateTime.Now - startTime).TotalMilliseconds}ms");

            // Guardar la lista completa
            todosLosGoleadores = goleadores.ToList();

            System.Diagnostics.Debug.WriteLine($"[GOLEADORES] Lista guardada en {(DateTime.Now - startTime).TotalMilliseconds}ms");

            // OPTIMIZACIÓN RADICAL: Cargar en lotes para renderizado progresivo
            listagoleadores = new ObservableCollection<GoleadorCLS>();
            
            // Cargar primeros 10 inmediatamente
            var primerosLotes = todosLosGoleadores.Take(10).ToList();
            foreach (var goleador in primerosLotes)
            {
                listagoleadores.Add(goleador);
            }
            
            OnPropertyChanged(nameof(listagoleadores));
            lblTotalGoleadores.Text = $"Total de goleadores: {todosLosGoleadores.Count}";
            
            // Ocultar indicador ANTES de cargar el resto
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            
            System.Diagnostics.Debug.WriteLine($"[GOLEADORES] Primeros 10 cargados en {(DateTime.Now - startTime).TotalMilliseconds}ms");
            
            // Cargar el resto en lotes de 10 con delay
            var resto = todosLosGoleadores.Skip(10).ToList();
            int batchSize = 10;
            
            for (int i = 0; i < resto.Count; i += batchSize)
            {
                await Task.Delay(50); // Dar tiempo al renderizado
                
                var batch = resto.Skip(i).Take(batchSize).ToList();
                foreach (var goleador in batch)
                {
                    listagoleadores.Add(goleador);
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"[GOLEADORES] Carga completada en {(DateTime.Now - startTime).TotalMilliseconds}ms");
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

    private void OnEquipoSelected(object sender, EventArgs e)
    {
        try
        {
            var picker = sender as Picker;
            
            // Si el SelectedIndex es -1, significa que se seleccionó el título o se canceló
            if (picker == null || picker.SelectedIndex == -1)
            {
                // Usar MainThread para actualizar la UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listagoleadores = new ObservableCollection<GoleadorCLS>(todosLosGoleadores);
                    OnPropertyChanged(nameof(listagoleadores));
                    lblTotalGoleadores.Text = $"Total de goleadores: {listagoleadores.Count}";
                });
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoListCLS;

            if (equipoSeleccionado == null)
            {
                // Usar MainThread para actualizar la UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listagoleadores = new ObservableCollection<GoleadorCLS>(todosLosGoleadores);
                    OnPropertyChanged(nameof(listagoleadores));
                    lblTotalGoleadores.Text = $"Total de goleadores: {listagoleadores.Count}";
                });
            }
            else
            {
                // Filtrar goleadores por equipo seleccionado
                var goleadoresFiltrados = todosLosGoleadores
                    .Where(g => g.idequipo == equipoSeleccionado.idequipo)
                    .ToList();

                // Usar MainThread para actualizar la UI
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listagoleadores = new ObservableCollection<GoleadorCLS>(goleadoresFiltrados);
                    OnPropertyChanged(nameof(listagoleadores));
                    lblTotalGoleadores.Text = $"Total de goleadores: {listagoleadores.Count}";
                });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al filtrar: {ex.Message}", "OK");
        }
    }

    private void OnLimpiarFiltroClicked(object sender, EventArgs e)
    {
        try
        {
            // Limpiar la selección del picker
            pickerEquipo.SelectedIndex = -1;
            
            // Recargar directamente desde la lista completa (instantáneo)
            if (todosLosGoleadores != null && todosLosGoleadores.Count > 0)
            {
                // Usar MainThread para actualizar la UI de forma eficiente
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listagoleadores = new ObservableCollection<GoleadorCLS>(todosLosGoleadores);
                    OnPropertyChanged(nameof(listagoleadores));
                    lblTotalGoleadores.Text = $"Total de goleadores: {listagoleadores.Count}";
                });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al limpiar filtro: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
