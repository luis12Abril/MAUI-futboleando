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

        try
        {
            _isNavigatingBack = true;

            if (Navigation?.NavigationStack?.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error al regresar: {ex.Message}");
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
    private bool datosYaCargados = false;

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

        if (!datosYaCargados)
        {
            try
            {
                // Mostrar indicador de carga
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
                
                await Task.Delay(100);
                await CargarEquipos();
                await CargarCumpleañeros();
                
                datosYaCargados = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Error en OnAppearing: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
                
                await DisplayAlert("Error", $"Error al cargar la página: {ex.Message}", "OK");
            }
            finally
            {
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
            }
        }
    }

    private async Task CargarEquipos()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] CargarEquipos iniciado");
            
            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] idTorneo: {idTorneoSeleccionado}");

            if (idTorneoSeleccionado == 0)
            {
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] No hay torneo seleccionado");
                return;
            }

            var equipos = await equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);
            
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Equipos recibidos: {equipos?.Count ?? 0}");
            
            if (equipos != null)
            {
                listaequipos = new ObservableCollection<EquipoListCLS>(equipos);
            }
            
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] CargarEquipos completado");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error en CargarEquipos: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
            await DisplayAlert("Error", $"Error al cargar equipos: {ex.Message}", "OK");
        }
    }

    private async Task CargarCumpleañeros()
    {
        try
        {
            var startTime = DateTime.Now;
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] CargarCumpleañeros iniciado");

            idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                loadingIndicator.IsRunning = false;
                loadingIndicator.IsVisible = false;
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Llamando al servicio con torneo {idTorneoSeleccionado}...");

            var cumpleañeros = await cumpleañeroService.ListarCumpleañerosPorTorneo(idTorneoSeleccionado);

            if (cumpleañeros == null)
            {
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] El servicio devolvió null");
                cumpleañeros = new List<CumpleañeroCLS>();
            }

            todosCumpleañeros = cumpleañeros;

            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Total recibidos: {todosCumpleañeros.Count}");

            // Verificar si hay cumpleañeros
            if (todosCumpleañeros.Count == 0)
            {
                listacumpleañeros.Clear();
                lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] No hay cumpleañeros para mostrar");
                return;
            }

            // Cargar todos los cumpleañeros de manera simple
            listacumpleañeros = new ObservableCollection<CumpleañeroCLS>(todosCumpleañeros);

            lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {todosCumpleañeros.Count}";

            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Carga completada en {elapsed}ms - {todosCumpleañeros.Count} cumpleañeros");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error en CargarCumpleañeros: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
            
            listacumpleañeros.Clear();
            lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
            
            await DisplayAlert("Error", $"Error al cargar cumpleañeros: {ex.Message}", "OK");
        }
    }

    private void OnEquipoSelected(object sender, EventArgs e)
    {
        try
        {
            var picker = sender as Picker;

            if (picker == null || picker.SelectedIndex == -1)
            {
                // Mostrar todos
                listacumpleañeros.Clear();
                
                if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
                {
                    foreach (var c in todosCumpleañeros)
                    {
                        listacumpleañeros.Add(c);
                    }
                }
                
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                System.Diagnostics.Debug.WriteLine($"[FILTRO] Mostrando todos: {listacumpleañeros.Count} cumpleañeros");
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoListCLS;

            if (equipoSeleccionado == null)
            {
                listacumpleañeros.Clear();
                
                if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
                {
                    foreach (var c in todosCumpleañeros)
                    {
                        listacumpleañeros.Add(c);
                    }
                }
                
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
            }
            else
            {
                var filtrados = todosCumpleañeros
                    .Where(c => c.nombreequipo.Equals(equipoSeleccionado.nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                listacumpleañeros.Clear();
                
                foreach (var c in filtrados)
                {
                    listacumpleañeros.Add(c);
                }
                
                lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                
                System.Diagnostics.Debug.WriteLine($"[FILTRO] Equipo '{equipoSeleccionado.nombre}': {listacumpleañeros.Count} cumpleañeros");
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

            // Mostrar todos los cumpleañeros
            listacumpleañeros.Clear();
            
            if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
            {
                foreach (var c in todosCumpleañeros)
                {
                    listacumpleañeros.Add(c);
                }
            }
            
            lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
            System.Diagnostics.Debug.WriteLine($"[LIMPIAR FILTRO] Mostrando todos: {listacumpleañeros.Count} cumpleañeros");
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al limpiar filtro: {ex.Message}", "OK");
        }
    }
}
