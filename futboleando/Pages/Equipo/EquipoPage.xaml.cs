using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using futboleando.Converters;
using futboleandoEntities.Jugador;
using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Equipo;
using futboleando.Models;
using System.ComponentModel;

namespace futboleando.Pages;

public partial class EquipoPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    private ObservableCollection<EquipoIndexed> _listaequipo;
    private string _nombreTorneoSeleccionado = "";
    public ObservableCollection<EquipoIndexed> listaequipo
    {
        get => _listaequipo;
        set
        {
            _listaequipo = value;
            OnPropertyChanged(nameof(listaequipo));
        }
    }
    public ObservableCollection<EquipoListCLS> listafiltro { get; set; }

    public EquipoListCLS objSeleccionado { get; set; }
    public string nombreequipo { get; set; }

    public string NombreTorneoSeleccionado
    {
        get => _nombreTorneoSeleccionado;
        set
        {
            _nombreTorneoSeleccionado = value;
            OnPropertyChanged(nameof(NombreTorneoSeleccionado));
        }
    }

    // Propiedad para el total de equipos
    private int _totalEquipos;
    public int TotalEquipos
    {
        get => _totalEquipos;
        set
        {
            _totalEquipos = value;
            OnPropertyChanged(nameof(TotalEquipos));
        }
    }
    
    public EquipoPage(EquipoService _equipoService)
	{
        InitializeComponent();
        equipoService = _equipoService;
        equipoService.Onchange += refrescarEquipo;
        listaequipo = new ObservableCollection<EquipoIndexed>();
        BindingContext = this;
        _ = listarEquipo();
    }

    private async Task refrescarEquipo()
    {
        await listarEquipo();
    }

    public async Task listarEquipo()
    {
        try
        {
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            NombreTorneoSeleccionado = Preferences.Get("NombreTorneo", "Sin torneo");

            ObservableCollection<EquipoListCLS> listaop;

            if (idTorneoSeleccionado > 0)
            {
                listaop = await equipoService.listarEquipoPorTorneoResumen(idTorneoSeleccionado);
            }
            else
            {
                listaop = await equipoService.listarEquipoResumen();
            }

            var indexedEquipos = listaop.Select((equipo, i) => new EquipoIndexed
            {
                Index = i + 1,
                Equipo = equipo,
                TieneFoto = false
            }).ToList();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listaequipo = new ObservableCollection<EquipoIndexed>(indexedEquipos);
                TotalEquipos = listaequipo.Count;
            });

            _ = PreloadImagesAsync(indexedEquipos);

            listafiltro = new ObservableCollection<EquipoListCLS>(listaop.ToList());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al conectar con la API: " + ex.Message, "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async Task PreloadImagesAsync(IReadOnlyList<EquipoIndexed> equipos)
    {
        if (equipos == null || equipos.Count == 0)
        {
            return;
        }

        var converter = new ByteArrayToImageConverter();
        var tasks = equipos.Select(equipo => CargarFotoAsync(equipo, converter)).ToList();
        await Task.WhenAll(tasks);
    }

    private async Task CargarFotoAsync(EquipoIndexed equipo, ByteArrayToImageConverter converter)
    {
        if (equipo?.Equipo == null)
        {
            return;
        }

        var fotoBase64 = await equipoService.ObtenerFotoEquipo(equipo.Equipo.idequipo);
        if (string.IsNullOrWhiteSpace(fotoBase64))
        {
            return;
        }

        var source = converter.Convert(fotoBase64, typeof(ImageSource), null, CultureInfo.InvariantCulture) as ImageSource;
        if (source == null)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            equipo.FotoSource = source;
            equipo.TieneFoto = true;
        });
    }
}