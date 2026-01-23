using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Jugador;
using futboleando.Models;
using System.ComponentModel;

namespace futboleando.Pages;

public partial class JugadorPage : ContentPage, INotifyPropertyChanged
{
    private readonly JugadorService jugadorService;
    public ObservableCollection<JugadorIndexed> listajugador { get; set; }
    private List<JugadorListCLS> listafiltro { get; set; }

    public JugadorListCLS objSeleccionado { get; set; }

    private CancellationTokenSource _debounceTokenSource;

    // ✅ Propiedad para el total de jugadores
    private int _totalJugadores;
    public int TotalJugadores
    {
        get => _totalJugadores;
        set
        {
            _totalJugadores = value;
            OnPropertyChanged(nameof(TotalJugadores));
            OnPropertyChanged(nameof(TotalJugadoresTexto));
        }
    }

    // ✅ Texto formateado para mostrar
    public string TotalJugadoresTexto => $"Total: {TotalJugadores} jugador{(TotalJugadores != 1 ? "es" : "")}";

    public JugadorPage(JugadorService _jugadorService)
    {
        InitializeComponent();
        jugadorService = _jugadorService;
        jugadorService.Onchange += refrescarJugador;
        listajugador = new ObservableCollection<JugadorIndexed>();
        listafiltro = new List<JugadorListCLS>();
        BindingContext = this;
        _ = listarJugador();
    }

    private async Task refrescarJugador()
    {
        await listarJugador();
    }

    public async Task listarJugador()
    {
        try
        {
            // ✅ Obtener el ID del torneo seleccionado desde Preferences
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            ObservableCollection<JugadorListCLS> listaop;

            if (idTorneoSeleccionado > 0)
            {
                // ✅ Obtener jugadores del torneo seleccionado
                listaop = await jugadorService.listarJugadorPorTorneo(idTorneoSeleccionado);
            }
            else
            {
                // ✅ Si no hay torneo seleccionado, obtener todos
                listaop = await jugadorService.listarJugador();
            }

            // Convertir a lista simple para mejor rendimiento
            listafiltro = listaop.Take(100).ToList();

            // Actualizar UI en el hilo principal
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listajugador.Clear();
                int index = 1;
                foreach (var jugador in listafiltro)
                {
                    listajugador.Add(new JugadorIndexed 
                    { 
                        Index = index++, 
                        Jugador = jugador 
                    });
                }
                
                // Actualizar contador
                TotalJugadores = listajugador.Count;
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar jugadores: " + ex.Message, "OK");
        }
    }

    private void btnRegresar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void searchNombre_SearchButtonPressed(object sender, EventArgs e)
    {
        DisplayAlert("Alerta", "Buscar", "OK");
    }

    private async void entryNombreJugador_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        // ✅ Cancelar búsqueda anterior (debouncing)
        _debounceTokenSource?.Cancel();
        _debounceTokenSource = new CancellationTokenSource();
        var token = _debounceTokenSource.Token;

        try
        {
            // ✅ Esperar 300ms antes de filtrar (evita filtrar en cada tecla)
            await Task.Delay(300, token);

            string textoBusqueda = e.NewTextValue?.Trim() ?? string.Empty;

            // ✅ Verificar que haya datos
            if (listafiltro == null || listafiltro.Count == 0)
                return;

            List<JugadorListCLS> listaFiltrada;

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                // Mostrar todos
                listaFiltrada = listafiltro;
            }
            else
            {
                // ✅ Filtrar en background thread (no bloquea UI)
                string textoBusquedaUpper = textoBusqueda.ToUpper();
                
                listaFiltrada = await Task.Run(() =>
                {
                    return listafiltro
                        .Where(j => 
                            (j.nombrecompleto?.ToUpper().Contains(textoBusquedaUpper) ?? false) ||
                            (j.nombre?.ToUpper().Contains(textoBusquedaUpper) ?? false) ||
                            (j.nombreequipo?.ToUpper().Contains(textoBusquedaUpper) ?? false))
                        .ToList();
                }, token);
            }

            // ✅ Actualizar UI solo una vez
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listajugador.Clear();
                int index = 1;
                foreach (var jugador in listaFiltrada)
                {
                    listajugador.Add(new JugadorIndexed 
                    { 
                        Index = index++, 
                        Jugador = jugador 
                    });
                }
                
                // ✅ Actualizar contador
                TotalJugadores = listajugador.Count;
            });
        }
        catch (TaskCanceledException)
        {
            // Usuario sigue escribiendo, ignorar esta búsqueda
        }
        catch (Exception ex)
        {
            // Ignorar otros errores para no bloquear la UI
        }
    }
}