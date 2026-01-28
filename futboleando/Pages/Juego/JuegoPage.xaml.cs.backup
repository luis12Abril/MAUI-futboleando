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
    
    private List<JuegoListCLS> todosLosJuegos;  // ? Cambiar a List<> para mejor rendimiento
    private int idTorneoSeleccionado;
    private bool _isLoading = false;

    public JuegoPage(JuegoService _juegoService)
    {
        InitializeComponent();
        juegoService = _juegoService;
        listajuegos = new ObservableCollection<JuegoListCLS>();
        listajornada = new ObservableCollection<JornadaListCLS>();
        todosLosJuegos = new List<JuegoListCLS>();  // ? List en vez de ObservableCollection
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
            // ? Obtener el ID del torneo seleccionado
            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");

            // ? Actualizar título con nombre del torneo
            lblTorneoNombre.Text = $"Torneo: {nombreTorneo}";

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                return;
            }

            // ? Desregistrar evento temporalmente
            pickerJornada.SelectedIndexChanged -= OnJornadaSelected;

            // ? Cargar jornadas
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
			
			// ? Cargar todos los juegos en memoria (List, no ObservableCollection)
			var juegos = await juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);
            todosLosJuegos = juegos.ToList();  // ? Guardar como List directamente

            // ? Cargar SOLO los primeros juegos para mostrar rápido
            await CargarJuegosEnLotes(todosLosJuegos);

            // ? Seleccionar "Todas las Jornadas" por defecto
            jornadaSeleccionada = listajornada.FirstOrDefault();
            pickerJornada.SelectedItem = jornadaSeleccionada;

            // ? Actualizar contador
            ActualizarContador();

            // ? Re-registrar evento
            pickerJornada.SelectedIndexChanged += OnJornadaSelected;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar juegos: {ex.Message}", "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    // ? NUEVO: Cargar juegos en lotes para no bloquear UI
    private async Task CargarJuegosEnLotes(List<JuegoListCLS> juegos, int batchSize = 20)
    {
        listajuegos.Clear();

        // ? Si hay pocos juegos, cargar todos de una vez
        if (juegos.Count <= batchSize)
        {
            foreach (var juego in juegos)
            {
                listajuegos.Add(juego);
            }
            return;
        }

        // ? Cargar en lotes para no bloquear el UI
        int totalJuegos = juegos.Count;
        int procesados = 0;

        while (procesados < totalJuegos)
        {
            // ? Tomar el siguiente lote
            var lote = juegos.Skip(procesados).Take(batchSize).ToList();
            
            // ? Agregar el lote a la colección observable
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                foreach (var juego in lote)
                {
                    listajuegos.Add(juego);
                }
            });

            procesados += batchSize;

            // ? Pequeña pausa para que el UI se actualice (solo si hay más lotes)
            if (procesados < totalJuegos)
            {
                await Task.Delay(10);  // 10ms de pausa entre lotes
            }
        }
    }

    private void OnJornadaSelected(object sender, EventArgs e)
    {
        if (_isLoading) return;

        // ? Usar Task.Run para no bloquear el UI thread
        Task.Run(async () =>
        {
            await FiltrarJuegosAsync();
        });
    }

    // ? NUEVO: Método async para filtrar
    private async Task FiltrarJuegosAsync()
    {
        try
        {
            List<JuegoListCLS> juegosFiltrados;

            // ? Filtrar en background thread
            if (jornadaSeleccionada == null || jornadaSeleccionada.idjornada == 0)
            {
                // ? Todas las jornadas
                juegosFiltrados = todosLosJuegos.ToList();
            }
            else
            {
                // ? Jornada específica (filtrado rápido)
                juegosFiltrados = todosLosJuegos
                    .Where(j => j.idjornada == jornadaSeleccionada.idjornada)
                    .ToList();
            }

            // ? Actualizar UI en el main thread con carga en lotes
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await CargarJuegosEnLotes(juegosFiltrados);
                ActualizarContador();
            });
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", $"Error al filtrar juegos: {ex.Message}", "OK");
            });
        }
    }

    private void ActualizarContador()
    {
        lblTotalJuegos.Text = $"Total de juegos: {listajuegos.Count}";
    }

    private async void OnVerMasClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is Button button && button.CommandParameter is int idJuego)
            {
                // ? TODO: Navegar a página de detalles del juego
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