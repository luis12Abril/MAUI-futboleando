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
    private CancellationTokenSource _cts;
    private bool _isLoading = false;

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

        if (!datosYaCargados && !_isLoading)
        {
            _isLoading = true;
            _cts = new CancellationTokenSource();
            
            try
            {
                // Mostrar indicador de carga
                loadingIndicator.IsRunning = true;
                loadingIndicator.IsVisible = true;
                
                await Task.Delay(50, _cts.Token);
                await Task.WhenAll(CargarEquipos(), CargarCumpleañeros());
                datosYaCargados = true;
            }
            catch (OperationCanceledException)
            {
                System.Diagnostics.Debug.WriteLine($"⚠️ Carga cancelada por el usuario");
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
                _isLoading = false;
            }
        }
    }
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Cancelar cualquier tarea en segundo plano
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        
        System.Diagnostics.Debug.WriteLine($"🔄 Página de cumpleañeros cerrada - tareas canceladas");
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                    loadingIndicator.IsRunning = false;
                    loadingIndicator.IsVisible = false;
                });
                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] No hay cumpleañeros para mostrar");
                return;
            }

            // Carga progresiva
            listacumpleañeros.Clear();

            var primeros = todosCumpleañeros.Take(10).ToList();
            foreach (var cumpleañero in primeros)
            {
                listacumpleañeros.Add(cumpleañero);
            }

            lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {todosCumpleañeros.Count}";

            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;

            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Primeros {primeros.Count} cargados");

            // Cargar el resto en segundo plano con cancelación
            if (todosCumpleañeros.Count > 10)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var resto = todosCumpleañeros.Skip(10).ToList();
                        int batchSize = 10;

                        for (int i = 0; i < resto.Count; i += batchSize)
                        {
                            // Verificar si se canceló
                            if (_cts?.Token.IsCancellationRequested == true)
                            {
                                System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Carga en segundo plano cancelada");
                                break;
                            }
                            
                            await Task.Delay(50, _cts?.Token ?? CancellationToken.None);
                            var batch = resto.Skip(i).Take(batchSize).ToList();
                            
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                try
                                {
                                    foreach (var cumpleañero in batch)
                                    {
                                        listacumpleañeros.Add(cumpleañero);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"❌ Error agregando batch: {ex.Message}");
                                }
                            });
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Carga completa en segundo plano");
                    }
                    catch (OperationCanceledException)
                    {
                        System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Carga en segundo plano cancelada");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ Error cargando resto: {ex.Message}");
                    }
                }, _cts?.Token ?? CancellationToken.None);
            }

            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            System.Diagnostics.Debug.WriteLine($"[CUMPLEAÑEROS] Carga inicial completada en {elapsed}ms");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Error en CargarCumpleañeros: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
            
            await DisplayAlert("Error", $"Error al cargar cumpleañeros: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
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
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    
                    if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
                    {
                        foreach (var c in todosCumpleañeros)
                        {
                            listacumpleañeros.Add(c);
                        }
                        lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                    }
                    else
                    {
                        lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                    }
                    
                    OnPropertyChanged(nameof(listacumpleañeros));
                    
                    System.Diagnostics.Debug.WriteLine($"[FILTRO] Mostrando todos: {listacumpleañeros.Count} cumpleañeros");
                });
                return;
            }

            var equipoSeleccionado = picker.SelectedItem as EquipoListCLS;

            if (equipoSeleccionado == null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    
                    if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
                    {
                        foreach (var c in todosCumpleañeros)
                        {
                            listacumpleañeros.Add(c);
                        }
                        lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                    }
                    else
                    {
                        lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                    }
                    
                    OnPropertyChanged(nameof(listacumpleañeros));
                });
            }
            else
            {
                var filtrados = todosCumpleañeros
                    .Where(c => c.nombreequipo.Equals(equipoSeleccionado.nombre, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    
                    foreach (var c in filtrados)
                    {
                        listacumpleañeros.Add(c);
                    }
                    
                    OnPropertyChanged(nameof(listacumpleañeros));
                    lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                    
                    if (filtrados.Count == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[FILTRO] Equipo '{equipoSeleccionado.nombre}': Sin cumpleañeros");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[FILTRO] Equipo '{equipoSeleccionado.nombre}': {listacumpleañeros.Count} cumpleañeros");
                    }
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

            // Mostrar todos los cumpleañeros
            if (todosCumpleañeros != null && todosCumpleañeros.Count > 0)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    
                    foreach (var c in todosCumpleañeros)
                    {
                        listacumpleañeros.Add(c);
                    }
                    
                    OnPropertyChanged(nameof(listacumpleañeros));
                    lblTotalCumpleañeros.Text = $"Total de cumpleañeros: {listacumpleañeros.Count}";
                    
                    System.Diagnostics.Debug.WriteLine($"[LIMPIAR FILTRO] Mostrando todos: {listacumpleañeros.Count} cumpleañeros");
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    listacumpleañeros.Clear();
                    OnPropertyChanged(nameof(listacumpleañeros));
                    lblTotalCumpleañeros.Text = "Total de cumpleañeros: 0";
                    
                    System.Diagnostics.Debug.WriteLine($"[LIMPIAR FILTRO] No hay cumpleañeros para mostrar");
                });
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Error al limpiar filtro: {ex.Message}", "OK");
        }
    }
}
