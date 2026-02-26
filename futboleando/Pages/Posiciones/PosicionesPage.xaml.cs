using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Equipo;

namespace futboleando.Pages.Posiciones;

public partial class PosicionesPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    private bool _isLoading;
    private bool _datosCargados;
    private int _ultimoTorneoCargado;
    
    private ObservableCollection<PosicionModel> _listaPosiciones;
    public ObservableCollection<PosicionModel> ListaPosiciones
    {
        get => _listaPosiciones;
        set
        {
            _listaPosiciones = value;
            OnPropertyChanged(nameof(ListaPosiciones));
        }
    }

    public PosicionesPage(EquipoService _equipoService)
    {
        InitializeComponent();
        equipoService = _equipoService;
        ListaPosiciones = new ObservableCollection<PosicionModel>();
        BindingContext = this;
    }

    private async Task CargarTablaPosiciones()
    {
        try
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;

            // Obtener el ID del torneo seleccionado desde Preferences
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            lblTorneoNombre.Text = nombreTorneo;

            if (_datosCargados && _ultimoTorneoCargado == idTorneoSeleccionado)
            {
                return;
            }

            ObservableCollection<EquipoListCLS> listaEquipos;
            var stopwatchApi = Stopwatch.StartNew();

            if (idTorneoSeleccionado > 0)
            {
                // Obtener equipos del torneo seleccionado (resumen para evitar fotos)
                listaEquipos = await equipoService.listarEquipoPorTorneoResumen(idTorneoSeleccionado);
            }
            else
            {
                // Si no hay torneo seleccionado, obtener todos
                listaEquipos = await equipoService.listarEquipoResumen();
            }

            stopwatchApi.Stop();

            var stopwatchUi = Stopwatch.StartNew();

            var posiciones = await Task.Run(() =>
            {
                var equiposOrdenados = listaEquipos
                    .OrderByDescending(e => e.puntos ?? 0)
                    .ThenByDescending(e => e.difgoles ?? 0)
                    .ThenByDescending(e => e.golesafavor ?? 0)
                    .ThenBy(e => e.nombre)
                    .ToList();

                var lista = new List<PosicionModel>(equiposOrdenados.Count);
                var posicion = 1;

                foreach (var equipo in equiposOrdenados)
                {
                    lista.Add(new PosicionModel
                    {
                        Posicion = posicion++,
                        Equipo = equipo,
                        EquipoNombre = equipo.nombre ?? string.Empty,
                        Jugados = equipo.jugados ?? 0,
                        Ganados = equipo.ganados ?? 0,
                        Perdidos = equipo.perdidos ?? 0,
                        Empatados = equipo.empatados ?? 0,
                        EmpatadosGanados = equipo.empatadosganados ?? 0,
                        GolesAFavor = equipo.golesafavor ?? 0,
                        GolesEnContra = equipo.golesencontra ?? 0,
                        DifGoles = equipo.difgoles ?? 0,
                        Puntos = equipo.puntos ?? 0
                    });
                }

                return lista;
            });

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ListaPosiciones.Clear();
                foreach (var posicion in posiciones)
                {
                    ListaPosiciones.Add(posicion);
                }
            });

            stopwatchUi.Stop();
            Debug.WriteLine($"[POSICIONES] API: {stopwatchApi.Elapsed.TotalSeconds:F2}s | UI: {stopwatchUi.Elapsed.TotalSeconds:F2}s");

            _ultimoTorneoCargado = idTorneoSeleccionado;
            _datosCargados = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar tabla de posiciones: " + ex.Message, "OK");
        }
        finally
        {
            _isLoading = false;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = CargarTablaPosiciones();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // ========== EVENTOS DE TABS ==========
    
    private void OnVista1Tapped(object sender, EventArgs e)
    {
        CambiarVista(1);
    }

    private void OnVista2Tapped(object sender, EventArgs e)
    {
        CambiarVista(2);
    }

    private void OnVista3Tapped(object sender, EventArgs e)
    {
        CambiarVista(3);
    }

    private void OnVista4Tapped(object sender, EventArgs e)
    {
        CambiarVista(4);
    }

    private void CambiarVista(int numeroVista)
    {
        // Ocultar todos los contenidos
        contenidoVista1.IsVisible = false;
        contenidoVista2.IsVisible = false;
        contenidoVista3.IsVisible = false;
        contenidoVista4.IsVisible = false;

        // Restablecer estilo de todos los tabs
        vista1Border.BackgroundColor = Colors.Transparent;
        vista2Border.BackgroundColor = Colors.Transparent;
        vista3Border.BackgroundColor = Colors.Transparent;
        vista4Border.BackgroundColor = Colors.Transparent;

        // Restablecer color de texto de todos los labels
        if (vista1Border.Content is Label lbl1) lbl1.TextColor = Colors.White;
        if (vista2Border.Content is Label lbl2) lbl2.TextColor = Colors.White;
        if (vista3Border.Content is Label lbl3) lbl3.TextColor = Colors.White;
        if (vista4Border.Content is Label lbl4) lbl4.TextColor = Colors.White;

        // Mostrar vista seleccionada y resaltar tab
        switch (numeroVista)
        {
            case 1:
                contenidoVista1.IsVisible = true;
                vista1Border.BackgroundColor = Colors.White;
                if (vista1Border.Content is Label lblActivo1) lblActivo1.TextColor = Color.FromArgb("#1E88E5");
                break;
            case 2:
                contenidoVista2.IsVisible = true;
                vista2Border.BackgroundColor = Colors.White;
                if (vista2Border.Content is Label lblActivo2) lblActivo2.TextColor = Color.FromArgb("#1E88E5");
                break;
            case 3:
                contenidoVista3.IsVisible = true;
                vista3Border.BackgroundColor = Colors.White;
                if (vista3Border.Content is Label lblActivo3) lblActivo3.TextColor = Color.FromArgb("#1E88E5");
                break;
            case 4:
                contenidoVista4.IsVisible = true;
                vista4Border.BackgroundColor = Colors.White;
                if (vista4Border.Content is Label lblActivo4) lblActivo4.TextColor = Color.FromArgb("#1E88E5");
                break;
        }
    }
}