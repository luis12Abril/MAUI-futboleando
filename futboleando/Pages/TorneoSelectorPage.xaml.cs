using futboleando.Service;
using futboleandoEntities.Estado;
using futboleandoEntities.Municipio;
using futboleandoEntities.Liga;
using futboleandoEntities.Torneo;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;

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

        // ? Solo UNA bandera necesaria
        private bool _isInitializing = false;
        private bool _suppressSelectionEvents = false;

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

            // ? Evitar múltiples inicializaciones simultáneas
            if (_isInitializing) return;
            
            _isInitializing = true;
            try
            {
                DisableAllPickers();
                await CargarDatosIniciales();
            }
            finally
            {
                pickerEstado.InputTransparent = true;
                await Task.Delay(150);
                EnablePickersBasedOnData();
                await PostRestoreUnfocusAsync();
                pickerEstado.InputTransparent = false;
                _isInitializing = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pickerEstado?.Unfocus();
            pickerMunicipio?.Unfocus();
            pickerLiga?.Unfocus();
            pickerTorneo?.Unfocus();
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                // ? Cargar estados
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

                // ? Intentar restaurar selección anterior
                var ultimoEstado = Preferences.Get("UltimoEstado", 0);
                var ultimoMunicipio = Preferences.Get("UltimoMunicipio", 0);
                var ultimaLiga = Preferences.Get("UltimaLiga", 0);
                var ultimoTorneo = Preferences.Get("UltimoTorneo", 0);

                if (ultimoEstado > 0)
                {
                    await RestaurarSeleccion(ultimoEstado, ultimoMunicipio, ultimaLiga, ultimoTorneo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", 
                    $"No se pudieron cargar los estados.\n\n" +
                    $"Error: {ex.Message}\n\n" +
                    $"Verifica que la API esté ejecutándose en:\n" +
                    $"http://futboleandoapp.somee.com/", "OK");
            }
        }

        private async Task PostRestoreUnfocusAsync()
        {
            await Task.Delay(150);
            pickerEstado?.Unfocus();
            pickerMunicipio?.Unfocus();
            pickerLiga?.Unfocus();
            pickerTorneo?.Unfocus();
            await Task.Delay(150);
            pickerEstado?.Unfocus();
            pickerMunicipio?.Unfocus();
            pickerLiga?.Unfocus();
            pickerTorneo?.Unfocus();
        }

        private async Task RestaurarSeleccion(int idEstado, int idMunicipio, int idLiga, int idTorneo)
        {
            try
            {
                var estados = pickerEstado.ItemsSource as List<EstadoListCLS>;
                var estado = estados?.FirstOrDefault(e => e.idestado == idEstado);

                if (estado == null)
                {
                    ValidarSeleccionCompleta();
                    return;
                }

                _suppressSelectionEvents = true;
                pickerEstado.SelectedItem = estado;

                var municipiosTask = municipioService.ListarPorEstado(idEstado);
                var ligasTask = idMunicipio > 0
                    ? ligaService.ListarPorMunicipio(idMunicipio)
                    : Task.FromResult(new ObservableCollection<LigaListCLS>());

                var torneosTask = idLiga > 0
                    ? torneoService.ListarPorLiga(idLiga)
                    : Task.FromResult(new ObservableCollection<TorneoListCLS>());

                await Task.WhenAll(municipiosTask, ligasTask, torneosTask);

                var municipios = municipiosTask.Result;
                if (municipios != null && municipios.Count > 0)
                {
                    pickerMunicipio.ItemsSource = municipios.ToList();
                    pickerMunicipio.ItemDisplayBinding = new Binding("nombre");
                    var municipio = municipios.FirstOrDefault(m => m.idmunicipio == idMunicipio);
                    if (municipio != null)
                    {
                        pickerMunicipio.SelectedItem = municipio;
                    }
                }

                var ligas = ligasTask.Result;
                if (ligas != null && ligas.Count > 0)
                {
                    pickerLiga.ItemsSource = ligas.ToList();
                    pickerLiga.ItemDisplayBinding = new Binding("nombre");
                    var liga = ligas.FirstOrDefault(l => l.idliga == idLiga);
                    if (liga != null)
                    {
                        pickerLiga.SelectedItem = liga;
                    }
                }

                var torneos = torneosTask.Result;
                if (torneos != null && torneos.Count > 0)
                {
                    pickerTorneo.ItemsSource = torneos.ToList();
                    pickerTorneo.ItemDisplayBinding = new Binding("nombre");
                    var torneo = torneos.FirstOrDefault(t => t.idtorneo == idTorneo);
                    if (torneo != null)
                    {
                        pickerTorneo.SelectedItem = torneo;
                    }
                }

                EnablePickersBasedOnData();
                ValidarSeleccionCompleta();
                _suppressSelectionEvents = false;
            }
            catch (Exception ex)
            {
                _suppressSelectionEvents = false;
                await DisplayAlert("Error", $"Error al restaurar selección: {ex.Message}", "OK");
            }
        }

        private void DisableAllPickers()
        {
            if (pickerEstado != null) pickerEstado.IsEnabled = false;
            if (pickerMunicipio != null) pickerMunicipio.IsEnabled = false;
            if (pickerLiga != null) pickerLiga.IsEnabled = false;
            if (pickerTorneo != null) pickerTorneo.IsEnabled = false;
        }

        private void EnablePickersBasedOnData()
        {
            if (pickerEstado != null)
            {
                pickerEstado.IsEnabled = pickerEstado.ItemsSource != null;
                pickerEstado.Opacity = pickerEstado.IsEnabled ? 1.0 : 0.6;
            }

            if (pickerMunicipio != null)
            {
                pickerMunicipio.IsEnabled = pickerMunicipio.ItemsSource != null;
                pickerMunicipio.Opacity = pickerMunicipio.IsEnabled ? 1.0 : 0.6;
            }

            if (pickerLiga != null)
            {
                pickerLiga.IsEnabled = pickerLiga.ItemsSource != null;
                pickerLiga.Opacity = pickerLiga.IsEnabled ? 1.0 : 0.6;
            }

            if (pickerTorneo != null)
            {
                pickerTorneo.IsEnabled = pickerTorneo.ItemsSource != null;
                pickerTorneo.Opacity = pickerTorneo.IsEnabled ? 1.0 : 0.6;
            }
        }

        private async void OnEstadoSelected(object sender, EventArgs e)
        {
            if (_isInitializing || _suppressSelectionEvents) return;

            var estadoSeleccionado = pickerEstado.SelectedItem as EstadoListCLS;
            if (estadoSeleccionado == null) return;

            try
            {
                // ? Limpiar selecciones dependientes
                pickerMunicipio.ItemsSource = null;
                pickerMunicipio.SelectedItem = null;
                pickerMunicipio.IsEnabled = false;
                
                pickerLiga.ItemsSource = null;
                pickerLiga.SelectedItem = null;
                pickerLiga.IsEnabled = false;
                
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;
                pickerTorneo.IsEnabled = false;

                // ? Cargar municipios
                var municipios = await municipioService.ListarPorEstado(estadoSeleccionado.idestado);
                if (municipios != null && municipios.Count > 0)
                {
                    pickerMunicipio.ItemsSource = municipios.ToList();
                    pickerMunicipio.ItemDisplayBinding = new Binding("nombre");
                    pickerMunicipio.IsEnabled = true;
                    pickerMunicipio.Opacity = 1.0;
                }

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar municipios: {ex.Message}", "OK");
            }
        }

        private async void OnMunicipioSelected(object sender, EventArgs e)
        {
            if (_isInitializing || _suppressSelectionEvents) return;

            var municipioSeleccionado = pickerMunicipio.SelectedItem as MunicipioListCLS;
            if (municipioSeleccionado == null) return;

            try
            {
                // ? Limpiar selecciones dependientes
                pickerLiga.ItemsSource = null;
                pickerLiga.SelectedItem = null;
                pickerLiga.IsEnabled = false;
                
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;
                pickerTorneo.IsEnabled = false;

                // ? Cargar ligas
                var ligas = await ligaService.ListarPorMunicipio(municipioSeleccionado.idmunicipio);
                if (ligas != null && ligas.Count > 0)
                {
                    pickerLiga.ItemsSource = ligas.ToList();
                    pickerLiga.ItemDisplayBinding = new Binding("nombre");
                    pickerLiga.IsEnabled = true;
                    pickerLiga.Opacity = 1.0;
                }

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar ligas: {ex.Message}", "OK");
            }
        }

        private async void OnLigaSelected(object sender, EventArgs e)
        {
            if (_isInitializing || _suppressSelectionEvents) return;

            var ligaSeleccionada = pickerLiga.SelectedItem as LigaListCLS;
            if (ligaSeleccionada == null) return;

            try
            {
                // ? Limpiar selección dependiente
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;
                pickerTorneo.IsEnabled = false;

                // ? Cargar torneos
                var torneos = await torneoService.ListarPorLiga(ligaSeleccionada.idliga);
                if (torneos != null && torneos.Count > 0)
                {
                    pickerTorneo.ItemsSource = torneos.ToList();
                    pickerTorneo.ItemDisplayBinding = new Binding("nombre");
                    pickerTorneo.IsEnabled = true;
                    pickerTorneo.Opacity = 1.0;
                }

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar torneos: {ex.Message}", "OK");
            }
        }

        private void OnTorneoSelected(object sender, EventArgs e)
        {
            if (_isInitializing || _suppressSelectionEvents) return;
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
                // ? Guardar selección
                Preferences.Set("UltimoEstado", estadoSeleccionado.idestado);
                Preferences.Set("UltimoMunicipio", municipioSeleccionado.idmunicipio);
                Preferences.Set("UltimaLiga", ligaSeleccionada.idliga);
                Preferences.Set("UltimoTorneo", torneoSeleccionado.idtorneo);

                Preferences.Set("NombreEstado", estadoSeleccionado.nombre);
                Preferences.Set("NombreMunicipio", municipioSeleccionado.nombre);
                Preferences.Set("NombreLiga", ligaSeleccionada.nombre);
                Preferences.Set("NombreTorneo", torneoSeleccionado.nombre);

                // ? Navegar
                if (Navigation.NavigationStack.Count > 1)
                {
                    await Navigation.PopAsync();
                }
                else
                {
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
    }
}
