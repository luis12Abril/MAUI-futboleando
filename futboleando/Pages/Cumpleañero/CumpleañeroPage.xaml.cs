using futboleando.Service;
using futboleandoEntities.Cumpleañero;
using futboleandoEntities.Equipo;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace futboleando.Pages.Cumpleañero;

public partial class CumpleañeroPage : ContentPage, INotifyPropertyChanged
{
    private readonly CumpleañeroService cumpleañeroService;
    private readonly EquipoService equipoService;
    private bool _isLoading;
    private bool _datosCargados;
    private int _ultimoTorneoCargado;
    private CancellationTokenSource? _loadCts;
    private bool _isNavigatingBack;

    private ObservableCollection<CumpleañeroCLS> _listacumpleañeros;
    public ObservableCollection<CumpleañeroCLS> listacumpleañeros
    {
        get => _listacumpleañeros;
        set
        {
            _listacumpleañeros = value;
            OnPropertyChanged(nameof(listacumpleañeros));
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_isNavigatingBack)
        {
            return;
        }

        _isNavigatingBack = true;
        CancelarCarga();

        try
        {
            await Navigation.PopAsync();
        }
        finally
        {
            _isNavigatingBack = false;
        }
    }

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

    private int idTorneoSeleccionado;
    private List<CumpleañeroCLS> todosCumpleañeros;

    public CumpleañeroPage(CumpleañeroService _cumpleañeroService, EquipoService _equipoService)
    {
        InitializeComponent();

        cumpleañeroService = _cumpleañeroService;
        equipoService = _equipoService;
        listacumpleañeros = new ObservableCollection<CumpleañeroCLS>();
        listaequipos = new ObservableCollection<EquipoListCLS>();
        todosCumpleañeros = new List<CumpleañeroCLS>();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isNavigatingBack)
        {
            return;
        }

        await Task.Delay(50);
        await CargarDatos();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CancelarCarga();
    }

    private async Task CargarDatos()
    {
        try
        {
            if (_isLoading || _isNavigatingBack)
            {
                return;
            }

            CancelarCarga();
            _loadCts = new CancellationTokenSource();
            var token = _loadCts.Token;

            _isLoading = true;
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            lblTorneoNombre.Text = nombreTorneo;

            if (_datosCargados && _ultimoTorneoCargado == idTorneoSeleccionado)
            {
                return;
            }

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                listacumpleañeros.Clear();
                lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                return;
            }

            pickerEquipo.SelectedIndexChanged -= OnEquipoSelected;

            var equiposTask = equipoService.listarEquipoPorTorneoResumen(idTorneoSeleccionado);
            var cumpleañerosTask = cumpleañeroService.ListarCumpleañerosPorTorneo(idTorneoSeleccionado);

            await Task.WhenAll(equiposTask, cumpleañerosTask);

            if (token.IsCancellationRequested || _isNavigatingBack)
            {
                return;
            }

            var equipos = equiposTask.Result ?? new ObservableCollection<EquipoListCLS>();
            var cumpleañeros = cumpleañerosTask.Result ?? new List<CumpleañeroCLS>();

            todosCumpleañeros = cumpleañeros;

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (token.IsCancellationRequested || _isNavigatingBack)
                {
                    return;
                }

                listaequipos = new ObservableCollection<EquipoListCLS>(equipos);
                listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(todosCumpleañeros);
                pickerEquipo.SelectedIndex = -1;
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {todosCumpleañeros.Count}";
            });

            pickerEquipo.SelectedIndexChanged += OnEquipoSelected;

            _ultimoTorneoCargado = idTorneoSeleccionado;
            _datosCargados = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error en CargarDatos: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
            await DisplayAlert("Error", $"Error al cargar la página: {ex.Message}", "OK");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            _isLoading = false;
        }
    }

    private void CancelarCarga()
    {
        if (_loadCts == null)
        {
            return;
        }

        if (!_loadCts.IsCancellationRequested)
        {
            _loadCts.Cancel();
        }

        _loadCts.Dispose();
        _loadCts = null;
    }

    private void OnEquipoSelected(object sender, EventArgs e)
    {
        try
        {
            var picker = sender as Picker;

            if (picker == null || picker.SelectedIndex == -1)
            {
                listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(todosCumpleañeros);
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoListCLS;

            if (equipoSeleccionado == null)
            {
                listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(todosCumpleañeros);
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
            }
            else
            {
                var filtrados = todosCumpleañeros
                    .Where(c => c.nombreequipo.Equals(equipoSeleccionado.nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(filtrados);
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
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
            pickerEquipo.SelectedIndex = -1;
            listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(todosCumpleañeros);
            lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al limpiar filtro: {ex.Message}", "OK");
        }
    }
}
