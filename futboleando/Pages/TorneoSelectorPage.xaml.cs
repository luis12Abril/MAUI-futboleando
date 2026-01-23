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

            _ = CargarDatosIniciales();
        }

        private async Task CargarDatosIniciales()
        {
            try
            {
                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

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
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private async Task RestaurarSeleccion(int idEstado, int idMunicipio, int idLiga, int idTorneo)
        {
            try
            {
                var estados = pickerEstado.ItemsSource as List<EstadoListCLS>;
                var estado = estados?.FirstOrDefault(e => e.idestado == idEstado);
                if (estado != null)
                {
                    pickerEstado.SelectedItem = estado;
                    await Task.Delay(500);

                    var municipios = pickerMunicipio.ItemsSource as List<MunicipioListCLS>;
                    var municipio = municipios?.FirstOrDefault(m => m.idmunicipio == idMunicipio);
                    if (municipio != null)
                    {
                        pickerMunicipio.SelectedItem = municipio;
                        await Task.Delay(500);

                        var ligas = pickerLiga.ItemsSource as List<LigaListCLS>;
                        var liga = ligas?.FirstOrDefault(l => l.idliga == idLiga);
                        if (liga != null)
                        {
                            pickerLiga.SelectedItem = liga;
                            await Task.Delay(500);

                            var torneos = pickerTorneo.ItemsSource as List<TorneoListCLS>;
                            var torneo = torneos?.FirstOrDefault(t => t.idtorneo == idTorneo);
                            if (torneo != null)
                            {
                                pickerTorneo.SelectedItem = torneo;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private async void OnEstadoSelected(object sender, EventArgs e)
        {
            try
            {
                var estadoSeleccionado = pickerEstado.SelectedItem as EstadoListCLS;
                if (estadoSeleccionado == null) return;

                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

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

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private async void OnMunicipioSelected(object sender, EventArgs e)
        {
            try
            {
                var municipioSeleccionado = pickerMunicipio.SelectedItem as MunicipioListCLS;
                if (municipioSeleccionado == null) return;

                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                pickerLiga.ItemsSource = null;
                pickerLiga.SelectedItem = null;
                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;

                var ligas = await ligaService.ListarPorMunicipio(municipioSeleccionado.idmunicipio);
                pickerLiga.ItemsSource = ligas.ToList();
                pickerLiga.ItemDisplayBinding = new Binding("nombre");
                pickerLiga.IsEnabled = ligas.Count > 0;

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private async void OnLigaSelected(object sender, EventArgs e)
        {
            try
            {
                var ligaSeleccionada = pickerLiga.SelectedItem as LigaListCLS;
                if (ligaSeleccionada == null) return;

                activityIndicator.IsRunning = true;
                activityIndicator.IsVisible = true;

                pickerTorneo.ItemsSource = null;
                pickerTorneo.SelectedItem = null;

                var torneos = await torneoService.ListarPorLiga(ligaSeleccionada.idliga);
                pickerTorneo.ItemsSource = torneos.ToList();
                pickerTorneo.ItemDisplayBinding = new Binding("nombre");
                pickerTorneo.IsEnabled = torneos.Count > 0;

                ValidarSeleccionCompleta();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private void OnTorneoSelected(object sender, EventArgs e)
        {
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

                Application.Current.MainPage = new Flyout(
                    menuService, loginService, jugadorService,
                    ciudadService, colaboradorService, equipoService, comunicadoService);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}
