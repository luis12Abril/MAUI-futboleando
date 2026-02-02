using futboleando.Service;
using futboleandoEntities.JugadoresPorAño;
using System.Collections.ObjectModel;

namespace futboleando.Pages.JugadoresPorAño;

public partial class JugadoresPorAñoPage : ContentPage
{
    private readonly JugadoresPorAñoService jugadoresPorAñoService;
    public ObservableCollection<JugadoresPorAñoCLS> listajugadoresporaño { get; set; }
    public ObservableCollection<EquipoSimpleCLS> listaequipos { get; set; }
    public EquipoSimpleCLS equipoSeleccionado { get; set; }
    private int idTorneoSeleccionado;

    public JugadoresPorAñoPage(JugadoresPorAñoService _jugadoresPorAñoService)
    {
        InitializeComponent();
        jugadoresPorAñoService = _jugadoresPorAñoService;
        listajugadoresporaño = new ObservableCollection<JugadoresPorAñoCLS>();
        listaequipos = new ObservableCollection<EquipoSimpleCLS>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        try
        {
            // Mostrar indicador de carga
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

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

            // Deshabilitar el evento del picker temporalmente
            pickerEquipo.SelectedIndexChanged -= OnEquipoSelected;

            // Cargar equipos para el picker
            var equipos = await jugadoresPorAñoService.ListarEquiposPorTorneo(idTorneoSeleccionado);
            
            listaequipos.Clear();
            listaequipos.Add(new EquipoSimpleCLS 
            { 
                idequipo = 0, 
                nombre = "-- TODOS LOS EQUIPOS --" 
            });
            
            foreach (var equipo in equipos)
            {
                listaequipos.Add(equipo);
            }

            // Seleccionar "Todos los equipos" por defecto
            equipoSeleccionado = listaequipos.FirstOrDefault();
            pickerEquipo.SelectedItem = equipoSeleccionado;

            // Cargar jugadores por año (todos los equipos)
            await CargarJugadoresPorAño(null);

            // Rehabilitar el evento del picker
            pickerEquipo.SelectedIndexChanged += OnEquipoSelected;

            // Ocultar indicador de carga
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
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
            var jugadores = await jugadoresPorAñoService.ListarJugadoresPorAño(idTorneoSeleccionado, idEquipo);

            listajugadoresporaño.Clear();
            foreach (var grupo in jugadores)
            {
                listajugadoresporaño.Add(grupo);
            }

            // Calcular y mostrar total
            int total = jugadores.Sum(j => j.cantidad);
            lblTotalJugadores.Text = total.ToString();

            // Ocultar indicador de carga
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
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
            var equipoSeleccionado = pickerEquipo.SelectedItem as EquipoSimpleCLS;
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
}
