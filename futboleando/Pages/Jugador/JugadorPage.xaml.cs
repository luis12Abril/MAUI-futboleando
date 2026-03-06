using System.Collections.ObjectModel;
using System.Linq;
using futboleando.Service;
using futboleandoEntities.Equipo;
using futboleandoEntities.Jugador;
using futboleando.Models;
using System.ComponentModel;

namespace futboleando.Pages;

public partial class JugadorPage : ContentPage, INotifyPropertyChanged
{
    private readonly JugadorService jugadorService;
    private readonly EquipoService equipoService;
    private ObservableCollection<JugadorIndexed> _listajugador;
    public ObservableCollection<JugadorIndexed> listajugador
    {
        get => _listajugador;
        set
        {
            _listajugador = value;
            OnPropertyChanged(nameof(listajugador));
        }
    }
    private List<JugadorListCLS> listafiltro { get; set; }

    public ObservableCollection<EquipoListCLS> listaequipos { get; set; }

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

    public JugadorListCLS objSeleccionado { get; set; }

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

    public JugadorPage(JugadorService _jugadorService, EquipoService _equipoService)
    {
        InitializeComponent();
        jugadorService = _jugadorService;
        equipoService = _equipoService;
        jugadorService.Onchange += refrescarJugador;
        listajugador = new ObservableCollection<JugadorIndexed>();
        listafiltro = new List<JugadorListCLS>();
        listaequipos = new ObservableCollection<EquipoListCLS>();
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
            NombreTorneoSeleccionado = Preferences.Get("NombreTorneo", "Sin torneo");

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

            listafiltro = listaop.ToList();

            _ = CargarEquipos(idTorneoSeleccionado);

            // Actualizar UI en el hilo principal
            var indexed = listafiltro.Select((jugador, i) => new JugadorIndexed
            {
                Index = i + 1,
                Jugador = jugador
            }).ToList();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listajugador = new ObservableCollection<JugadorIndexed>(indexed);
                TotalJugadores = listajugador.Count;
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar jugadores: " + ex.Message, "OK");
        }
    }

    private async Task CargarEquipos(int idTorneoSeleccionado)
    {
        try
        {
            ObservableCollection<EquipoListCLS> equipos;

            if (idTorneoSeleccionado > 0)
            {
                equipos = await equipoService.listarEquipoPorTorneoResumen(idTorneoSeleccionado);
            }
            else
            {
                equipos = await equipoService.listarEquipoResumen();
            }

            var equiposOrdenados = equipos
                .OrderBy(e => e.nombre?.Trim())
                .ToList();

            listaequipos = new ObservableCollection<EquipoListCLS>(equiposOrdenados);
            OnPropertyChanged(nameof(listaequipos));
        }
        catch (Exception)
        {
        }
    }

    private void OnEquipoSelected(object sender, EventArgs e)
    {
        try
        {
            if (listafiltro == null || listafiltro.Count == 0)
            {
                return;
            }

            var picker = sender as Picker;
            if (picker == null || picker.SelectedIndex == -1)
            {
                ActualizarLista(listafiltro);
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoListCLS;
            if (equipoSeleccionado == null)
            {
                ActualizarLista(listafiltro);
                return;
            }

            var equipoId = equipoSeleccionado.idequipo;
            var equipoNombre = equipoSeleccionado.nombre?.Trim();

            var filtrados = listafiltro
                .Where(j =>
                    (equipoId > 0 && j.idequipo == equipoId) ||
                    (!string.IsNullOrWhiteSpace(equipoNombre) &&
                     string.Equals(j.nombreequipo?.Trim(), equipoNombre, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            ActualizarLista(filtrados);
        }
        catch (Exception)
        {
        }
    }

    private void OnLimpiarFiltroClicked(object sender, EventArgs e)
    {
        pickerEquipo.SelectedIndex = -1;
        ActualizarLista(listafiltro);
    }

    private void ActualizarLista(List<JugadorListCLS> jugadores)
    {
        var indexed = jugadores.Select((jugador, i) => new JugadorIndexed
        {
            Index = i + 1,
            Jugador = jugador
        }).ToList();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            listajugador = new ObservableCollection<JugadorIndexed>(indexed);
            TotalJugadores = listajugador.Count;
        });
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}