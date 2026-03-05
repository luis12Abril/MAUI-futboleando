using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using futboleando.Service;
using futboleandoEntities.Equipo;
using futboleandoEntities.Jugador;

namespace futboleando.Pages.Cumpleañero;

public partial class CumpleañerosdelTorneoPage : ContentPage, INotifyPropertyChanged
{
    private readonly JugadorService jugadorService;
    private readonly EquipoService equipoService;

    private ObservableCollection<CumpleañeroItem> _listacumpleañeros;
    public ObservableCollection<CumpleañeroItem> listacumpleañeros
    {
        get => _listacumpleañeros;
        set
        {
            _listacumpleañeros = value;
            OnPropertyChanged(nameof(listacumpleañeros));
        }
    }

    private List<CumpleañeroItem> listafiltro;

    private ObservableCollection<EquipoListCLS> _listaequipos;
    public ObservableCollection<EquipoListCLS> listaequipos
    {
        get => _listaequipos;
        set
        {
            _listaequipos = value;
            OnPropertyChanged(nameof(listaequipos));
        }
    }

    public CumpleañerosdelTorneoPage(JugadorService _jugadorService, EquipoService _equipoService)
    {
        InitializeComponent();
        jugadorService = _jugadorService;
        equipoService = _equipoService;
        listacumpleañeros = new ObservableCollection<CumpleañeroItem>();
        listafiltro = new List<CumpleañeroItem>();
        listaequipos = new ObservableCollection<EquipoListCLS>();
        BindingContext = this;
        _ = listarCumpleañeros();
    }

    private async Task listarCumpleañeros()
    {
        try
        {
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            lblTorneoNombre.Text = Preferences.Get("NombreTorneo", "Sin torneo");

            ObservableCollection<JugadorListCLS> jugadores;

            if (idTorneoSeleccionado > 0)
            {
                jugadores = await jugadorService.listarJugadorPorTorneo(idTorneoSeleccionado);
            }
            else
            {
                jugadores = await jugadorService.listarJugador();
            }

            listafiltro = CalcularCumpleañeros(jugadores.ToList());

            _ = CargarEquipos(idTorneoSeleccionado);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listacumpleañeros = new ObservableCollection<CumpleañeroItem>(listafiltro);
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar cumpleañeros: " + ex.Message, "OK");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
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

            listaequipos = new ObservableCollection<EquipoListCLS>(equipos);
        }
        catch (Exception)
        {
        }
    }

    private List<CumpleañeroItem> CalcularCumpleañeros(List<JugadorListCLS> jugadores)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        var cumpleañeros = new List<CumpleañeroItem>();

        foreach (var jugador in jugadores.Where(j => j.fnacimiento.HasValue))
        {
            var fechaNac = jugador.fnacimiento!.Value;
            var cumpleEsteAño = new DateOnly(hoy.Year, fechaNac.Month, fechaNac.Day);

            if (cumpleEsteAño < hoy)
            {
                cumpleEsteAño = new DateOnly(hoy.Year + 1, fechaNac.Month, fechaNac.Day);
            }

            var diasParaCumple = cumpleEsteAño.DayNumber - hoy.DayNumber;
            if (diasParaCumple < 0 || diasParaCumple > 14)
            {
                continue;
            }

            var edad = cumpleEsteAño.Year - fechaNac.Year;
            var nombreCompleto = string.Join(" ", new[] { jugador.nombre, jugador.appaterno, jugador.apmaterno }
                .Where(s => !string.IsNullOrWhiteSpace(s)));

            cumpleañeros.Add(new CumpleañeroItem
            {
                idjugador = jugador.idjugador ?? 0,
                idequipo = jugador.idequipo ?? 0,
                nombrecompleto = nombreCompleto,
                fechanacimiento = fechaNac,
                nombreequipo = jugador.nombreequipo ?? string.Empty,
                edad = edad,
                esCumpleanosHoy = diasParaCumple == 0,
                diasParaCumpleanos = diasParaCumple
            });
        }

        return cumpleañeros.OrderBy(c => c.diasParaCumpleanos).ToList();
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
                .Where(c =>
                    (equipoId > 0 && c.idequipo == equipoId) ||
                    (!string.IsNullOrWhiteSpace(equipoNombre) &&
                     string.Equals(c.nombreequipo?.Trim(), equipoNombre, StringComparison.OrdinalIgnoreCase)))
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

    private void ActualizarLista(List<CumpleañeroItem> cumpleañeros)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            listacumpleañeros = new ObservableCollection<CumpleañeroItem>(cumpleañeros);
            lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
        });
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public class CumpleañeroItem
    {
        public int idjugador { get; set; }
        public int idequipo { get; set; }
        public string nombrecompleto { get; set; } = string.Empty;
        public DateOnly fechanacimiento { get; set; }
        public string nombreequipo { get; set; } = string.Empty;
        public int edad { get; set; }
        public bool esCumpleanosHoy { get; set; }
        public int diasParaCumpleanos { get; set; }
        public string fechaDisplay => esCumpleanosHoy ? "HOY" : fechanacimiento.ToString("dd/MMM").ToUpper();
    }
}
