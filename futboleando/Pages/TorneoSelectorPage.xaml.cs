using futboleando.Service;
using futboleandoEntities.Estado;
using futboleandoEntities.Municipio;
using futboleandoEntities.Liga;
using futboleandoEntities.Torneo;

namespace futboleando.Pages
{
    public partial class TorneoSelectorPage : ContentPage
    {
        private readonly EstadoService estadoService;
        private readonly MunicipioService municipioService;
        private readonly LigaService ligaService;
        private readonly TorneoService torneoService;

        private readonly MenuService menuService;
        private readonly LoginService loginService;
        private readonly JugadorService jugadorService;
        private readonly CiudadService ciudadService;
        private readonly ColaboradorService colaboradorService;
        private readonly EquipoService equipoService;
        private readonly ComunicadoService comunicadoService;

        // ? Bandera para evitar que eventos se disparen durante la carga/restauración
        private bool _isRestoring = false;
        private bool _isFirstLoad = true;
        private bool _isProcessingEvent = false;  // ? Nueva bandera para evitar eventos concurrentes
        private bool _allowPickerFocus = false;  // ? NUEVA: Controlar si los pickers pueden tener foco

        public TorneoSelectorPage(
            EstadoService _estadoService,
            MunicipioService _municipioService,
            LigaService _ligaService,
            TorneoService _torneoService,
            MenuService _menuService,
            LoginService _loginService,
            JugadorService _jugadorService,
            CiudadService _ciudadService,
            ColaboradorService _colaboradorService,
            EquipoService _equipoService,
            ComunicadoService _comunicadoService)
        {
            InitializeComponent();

            estadoService = _estadoService;
            municipioService = _municipioService;
            ligaService = _ligaService;
            torneoService = _torneoService;

            menuService = _menuService;
            loginService = _loginService;
            jugadorService = _jugadorService;
            ciudadService = _ciudadService;
            colaboradorService = _colaboradorService;
            equipoService = _equipoService;
            comunicadoService = _comunicadoService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // ? BLOQUEAR focus en los pickers
            _allowPickerFocus = false;
            
            // ? Forzar opacidad normal ANTES de deshabilitar
            pickerMunicipio.Opacity = 1.0;
            pickerLiga.Opacity = 1.0;
            pickerTorneo.Opacity = 1.0;
            
            pickerMunicipio.IsEnabled = false;
            pickerLiga.IsEnabled = false;
            pickerTorneo.IsEnabled = false;

            if (_isFirstLoad)
            {
                _isFirstLoad = false;
                await CargarDatosIniciales();
            }
            else
            {
                // ? Desregistrar eventos ANTES de recargar
                pickerEstado.SelectedIndexChanged -= OnEstadoSelected;
                pickerMunicipio.SelectedIndexChanged -= OnMunicipioSelected;
                pickerLiga.SelectedIndexChanged -= OnLigaSelected;
                pickerTorneo.SelectedIndexChanged -= OnTorneoSelected;

                // ? No es primera vez: solo recargar sin eventos
                await RecargarSinEventos();

                // ? Esperar menos tiempo
                await Task.Delay(300);  // Reducido de 1000ms a 300ms

                // ? Registrar eventos nuevamente
                pickerEstado.SelectedIndexChanged += OnEstadoSelected;
                pickerMunicipio.SelectedIndexChanged += OnMunicipioSelected;
                pickerLiga.SelectedIndexChanged += OnLigaSelected;
                pickerTorneo.SelectedIndexChanged += OnTorneoSelected;

                // ? AHORA SÍ permitir que los pickers se puedan abrir
                _allowPickerFocus = true;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            // ? CRUCIAL: Resetear banderas y bloquear pickers al salir
            _allowPickerFocus = false;
            _isRestoring = false;
            _isProcessingEvent = false;
            
            // ? SOLUCIÓN RADICAL: Forzar unfocus en TODOS los pickers
            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    pickerEstado.Unfocus();
                    pickerMunicipio.Unfocus();
                    pickerLiga.Unfocus();
                    pickerTorneo.Unfocus();
                    
                    // ? Deshabilitar TODOS los pickers
                    pickerEstado.IsEnabled = true;  // Estado siempre habilitado
                    pickerMunicipio.IsEnabled = false;
                    pickerLiga.IsEnabled = false;
                    pickerTorneo.IsEnabled = false;
                }
                catch { }
            });
        }

        private async Task RecargarSinEventos()
        {
            try
            {
                _isRestoring = true;

                var ultimoEstado = Preferences.Get("UltimoEstado", 0);
                var ultimoMunicipio = Preferences.Get("UltimoMunicipio", 0);
                var ultimaLiga = Preferences.Get("UltimaLiga", 0);
                var ultimoTorneo = Preferences.Get("UltimoTorneo", 0);

                if (ultimoEstado > 0)
                {
                    await RestaurarSeleccion(ultimoEstado, ultimoMunicipio, ultimaLiga, ultimoTorneo);
                }

                // ? Esperar menos tiempo para que se vea más rápido
                await Task.Delay(300);  // Reducido de 1000ms a 300ms
                
                // ? Habilitar los pickers manteniendo opacidad
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (pickerMunicipio.ItemsSource != null && (pickerMunicipio.ItemsSource as List<MunicipioListCLS>)?.Count > 0)
                    {
                        pickerMunicipio.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerMunicipio.IsEnabled = true;
                    }
                        
                    if (pickerLiga.ItemsSource != null && (pickerLiga.ItemsSource as List<LigaListCLS>)?.Count > 0)
                    {
                        pickerLiga.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerLiga.IsEnabled = true;
                    }
                        
                    if (pickerTorneo.ItemsSource != null && (pickerTorneo.ItemsSource as List<TorneoListCLS>)?.Count > 0)
                    {
                        pickerTorneo.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerTorneo.IsEnabled = true;
                    }
                });
            }
            finally
            {
                _isRestoring = false;
            }
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                // ? Activar bandera: estamos cargando/restaurando
                _isRestoring = true;

                var estados = await estadoService.ListarEstados();

                if (estados == null || estados.Count == 0)
                {
                    await DisplayAlert("Aviso", 
                        "No se encontraron estados en la base de datos.\n\n" +
                        "Verifica:\n" +
                        "1. Que la API esté corriendo\n" +
                        "2. Que haya datos en la tabla ESTADO\n" +
                        "3. La conexión a internet", "OK");
                    return;
                }

                pickerEstado.ItemsSource = estados.ToList();
                pickerEstado.ItemDisplayBinding = new Binding("nombre");

                var ultimoEstado = Preferences.Get("UltimoEstado", 0);
                var ultimoMunicipio = Preferences.Get("UltimoMunicipio", 0);
                var ultimaLiga = Preferences.Get("UltimaLiga", 0);
                var ultimoTorneo = Preferences.Get("UltimoTorneo", 0);

                if (ultimoEstado > 0)
                {
                    await RestaurarSeleccion(ultimoEstado, ultimoMunicipio, ultimaLiga, ultimoTorneo);
                }

                // ? Esperar menos tiempo para que se vea más rápido
                await Task.Delay(300);  // Reducido de 1000ms a 300ms

                // ? Habilitar los pickers manteniendo opacidad
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (pickerMunicipio.ItemsSource != null && (pickerMunicipio.ItemsSource as List<MunicipioListCLS>)?.Count > 0)
                    {
                        pickerMunicipio.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerMunicipio.IsEnabled = true;
                    }
                        
                    if (pickerLiga.ItemsSource != null && (pickerLiga.ItemsSource as List<LigaListCLS>)?.Count > 0)
                    {
                        pickerLiga.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerLiga.IsEnabled = true;
                    }
                        
                    if (pickerTorneo.ItemsSource != null && (pickerTorneo.ItemsSource as List<TorneoListCLS>)?.Count > 0)
                    {
                        pickerTorneo.Opacity = 1.0;  // ? Forzar antes de habilitar
                        pickerTorneo.IsEnabled = true;
                    }
                });

                // ? Registrar eventos UNA SOLA VEZ al final
                pickerEstado.SelectedIndexChanged += OnEstadoSelected;
                pickerMunicipio.SelectedIndexChanged += OnMunicipioSelected;
                pickerLiga.SelectedIndexChanged += OnLigaSelected;
                pickerTorneo.SelectedIndexChanged += OnTorneoSelected;

                // ? Permitir que los pickers se puedan abrir
                _allowPickerFocus = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", 
                    $"No se pudieron cargar los estados.\n\n" +
                    $"Error: {ex.Message}\n\n" +
                    $"Verifica que la API esté ejecutándose en:\n" +
                    $"http://futboleandoapp.somee.com/", "OK");
            }
            finally
            {
                // ? Desactivar bandera: ya terminamos de cargar/restaurar
                _isRestoring = false;
            }
        }

        private async Task RestaurarSeleccion(int idEstado, int idMunicipio, int idLiga, int idTorneo)
        {
            try
            {
                // ? Cargar TODO secuencialmente en segundo plano (sin mostrar)
                var estados = pickerEstado.ItemsSource as List<EstadoListCLS>;
                
                var municipiosObs = await municipioService.ListarPorEstado(idEstado);
                var municipios = municipiosObs?.ToList() ?? new List<MunicipioListCLS>();
                
                var ligasObs = municipios.Count > 0 ? await ligaService.ListarPorMunicipio(idMunicipio) : null;
                var ligas = ligasObs?.ToList() ?? new List<LigaListCLS>();
                
                var torneosObs = ligas.Count > 0 ? await torneoService.ListarPorLiga(idLiga) : null;
                var torneos = torneosObs?.ToList() ?? new List<TorneoListCLS>();

                // ? TODO está cargado, ahora asignar de una sola vez (sin eventos activos)
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    // Asignar Estado
                    var estado = estados?.FirstOrDefault(e => e.idestado == idEstado);
                    if (estado != null)
                        pickerEstado.SelectedItem = estado;

                    // Asignar Municipios - PERO NO HABILITAR AÚN
                    if (municipios.Count > 0)
                    {
                        pickerMunicipio.ItemsSource = municipios;
                        pickerMunicipio.ItemDisplayBinding = new Binding("nombre");
                        pickerMunicipio.Opacity = 1.0;  // ? Opacidad normal aunque esté deshabilitado
                        // ? NO habilitar aquí - se habilita después del delay

                        var municipio = municipios.FirstOrDefault(m => m.idmunicipio == idMunicipio);
                        if (municipio != null)
                            pickerMunicipio.SelectedItem = municipio;
                    }

                    // Asignar Ligas - PERO NO HABILITAR AÚN
                    if (ligas.Count > 0)
                    {
                        pickerLiga.ItemsSource = ligas;
                        pickerLiga.ItemDisplayBinding = new Binding("nombre");
                        pickerLiga.Opacity = 1.0;  // ? Opacidad normal aunque esté deshabilitado
                        // ? NO habilitar aquí - se habilita después del delay

                        var liga = ligas.FirstOrDefault(l => l.idliga == idLiga);
                        if (liga != null)
                            pickerLiga.SelectedItem = liga;
                    }

                    // Asignar Torneos - PERO NO HABILITAR AÚN
                    if (torneos.Count > 0)
                    {
                        pickerTorneo.ItemsSource = torneos;
                        pickerTorneo.ItemDisplayBinding = new Binding("nombre");
                        pickerTorneo.Opacity = 1.0;  // ? Opacidad normal aunque esté deshabilitado
                        // ? NO habilitar aquí - se habilita después del delay

                        var torneo = torneos.FirstOrDefault(t => t.idtorneo == idTorneo);
                        if (torneo != null)
                            pickerTorneo.SelectedItem = torneo;
                    }

                    // Validar botón
                    ValidarSeleccionCompleta();
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private async void OnEstadoSelected(object sender, EventArgs e)
        {
            // ? Si estamos restaurando O procesando otro evento, ignorar
            if (_isRestoring || _isProcessingEvent) return;

            try
            {
                _isProcessingEvent = true;

                var estadoSeleccionado = pickerEstado.SelectedItem as EstadoListCLS;
                if (estadoSeleccionado == null) return;

                pickerMunicipio.ItemsSource = null;
                pickerMunicipio.SelectedItem = null;
                pickerLiga.ItemsSource = null;
                pickerLiga.SelectedItem = null;
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;

                var municipios = await municipioService.ListarPorEstado(estadoSeleccionado.idestado);
                pickerMunicipio.ItemsSource = municipios.ToList();
                pickerMunicipio.ItemDisplayBinding = new Binding("nombre");
                pickerMunicipio.IsEnabled = municipios.Count > 0;
                pickerMunicipio.Opacity = 1.0;  // ? Color normal

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                _isProcessingEvent = false;
            }
        }

        private async void OnMunicipioSelected(object sender, EventArgs e)
        {
            // ? Si estamos restaurando O procesando otro evento, ignorar
            if (_isRestoring || _isProcessingEvent) return;

            try
            {
                _isProcessingEvent = true;

                var municipioSeleccionado = pickerMunicipio.SelectedItem as MunicipioListCLS;
                if (municipioSeleccionado == null) return;

                pickerLiga.ItemsSource = null;
                pickerLiga.SelectedItem = null;
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;

                var ligas = await ligaService.ListarPorMunicipio(municipioSeleccionado.idmunicipio);
                pickerLiga.ItemsSource = ligas.ToList();
                pickerLiga.ItemDisplayBinding = new Binding("nombre");
                pickerLiga.IsEnabled = ligas.Count > 0;
                pickerLiga.Opacity = 1.0;  // ? Color normal

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                _isProcessingEvent = false;
            }
        }

        private async void OnLigaSelected(object sender, EventArgs e)
        {
            // ? Si estamos restaurando O procesando otro evento, ignorar
            if (_isRestoring || _isProcessingEvent) return;

            try
            {
                _isProcessingEvent = true;

                var ligaSeleccionada = pickerLiga.SelectedItem as LigaListCLS;
                if (ligaSeleccionada == null) return;

                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;

                var torneos = await torneoService.ListarPorLiga(ligaSeleccionada.idliga);
                pickerTorneo.ItemsSource = torneos.ToList();
                pickerTorneo.ItemDisplayBinding = new Binding("nombre");
                pickerTorneo.IsEnabled = torneos.Count > 0;
                pickerTorneo.Opacity = 1.0;  // ? Color normal

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                _isProcessingEvent = false;
            }
        }

        private void OnTorneoSelected(object sender, EventArgs e)
        {
            // ? Si estamos restaurando O procesando otro evento, ignorar
            if (_isRestoring || _isProcessingEvent) return;

            ValidarSeleccionCompleta();
        }

        private void ValidarSeleccionCompleta()
        {
            bool seleccionCompleta =
                pickerEstado.SelectedItem != null &&
                pickerMunicipio.SelectedItem != null &&
                pickerLiga.SelectedItem != null &&
                pickerTorneo.SelectedItem != null;

            btnVerTorneo.IsEnabled = seleccionCompleta;
            
            if (seleccionCompleta)
            {
                // Gradiente elegante cuando está habilitado
                btnVerTorneo.Background = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop { Color = Color.FromArgb("#1e3c72"), Offset = 0.0f },
                        new GradientStop { Color = Color.FromArgb("#2a5298"), Offset = 0.5f },
                        new GradientStop { Color = Color.FromArgb("#7e22ce"), Offset = 1.0f }
                    }
                };
            }
            else
            {
                btnVerTorneo.BackgroundColor = Color.FromArgb("#CCCCCC");
            }
        }

        private async void OnVerTorneoClicked(object sender, EventArgs e)
        {
            var estadoSeleccionado = pickerEstado.SelectedItem as EstadoListCLS;
            var municipioSeleccionado = pickerMunicipio.SelectedItem as MunicipioListCLS;
            var ligaSeleccionada = pickerLiga.SelectedItem as LigaListCLS;
            var torneoSeleccionado = pickerTorneo.SelectedItem as TorneoListCLS;

            if (estadoSeleccionado == null || municipioSeleccionado == null ||
                ligaSeleccionada == null || torneoSeleccionado == null)
            {
                await DisplayAlert("Atención", "Por favor completa la selección", "OK");
                return;
            }

            try
            {
                Preferences.Set("UltimoEstado", estadoSeleccionado.idestado);
                Preferences.Set("UltimoMunicipio", municipioSeleccionado.idmunicipio);
                Preferences.Set("UltimaLiga", ligaSeleccionada.idliga);
                Preferences.Set("UltimoTorneo", torneoSeleccionado.idtorneo);

                Preferences.Set("NombreEstado", estadoSeleccionado.nombre);
                Preferences.Set("NombreMunicipio", municipioSeleccionado.nombre);
                Preferences.Set("NombreLiga", ligaSeleccionada.nombre);
                Preferences.Set("NombreTorneo", torneoSeleccionado.nombre);

                // Verificar si estamos en navegación (desde el menú) o es primera vez
                if (Navigation.NavigationStack.Count > 1)
                {
                    // Estamos en navegación del menú, regresar directamente
                    await Navigation.PopAsync();
                }
                else
                {
                    // Es primera vez (login), crear el Flyout
                    Application.Current.MainPage = new Flyout(
                        menuService, loginService, jugadorService,
                        ciudadService, colaboradorService, equipoService, comunicadoService);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }

        private void OnPickerFocused(object sender, FocusEventArgs e)
        {
            // ? Si NO permitimos focus, desenfocamos inmediatamente el picker
            if (!_allowPickerFocus && sender is Picker picker)
            {
                picker.Unfocus();
            }
        }
    }
}
