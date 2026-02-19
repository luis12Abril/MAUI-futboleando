using futboleando.Service;
using futboleandoEntities.Visitas;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Visitas;

public partial class VisitasAppPage : ContentPage
{
    private readonly VisitasService visitasService;
    public ObservableCollection<VisitaUsuarioCLS> listavisitas { get; set; }
    public ObservableCollection<TipoUsuarioSimpleCLS> listatiposusuarios { get; set; }
    public TipoUsuarioSimpleCLS tipoUsuarioSeleccionado { get; set; }

    public VisitasAppPage(VisitasService _visitasService)
    {
        InitializeComponent();
        visitasService = _visitasService;
        listavisitas = new ObservableCollection<VisitaUsuarioCLS>();
        listatiposusuarios = new ObservableCollection<TipoUsuarioSimpleCLS>();
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

            // Cargar totales de visitas
            var totales = await visitasService.ObtenerVisitasTotales();
            // ? Formato de miles estilo México (1,660 y 5,689)
            lblTotalWeb.Text = totales.totalVisitasWeb.ToString("N0");
            lblTotalApp.Text = totales.totalVisitasApp.ToString("N0");

            // Deshabilitar el evento del picker temporalmente
            pickerTipoUsuario.SelectedIndexChanged -= OnTipoUsuarioSelected;

            // Cargar tipos de usuario para el picker
            var tipos = await visitasService.ObtenerTiposUsuario();
            
            System.Diagnostics.Debug.WriteLine($"?? Tipos de usuario obtenidos: {tipos.Count}");

            listatiposusuarios.Clear();
            
            // ? Agregar solo los tipos reales (sin opción "TODOS")
            foreach (var tipo in tipos)
            {
                listatiposusuarios.Add(tipo);
                System.Diagnostics.Debug.WriteLine($"  - {tipo.nombre} (ID: {tipo.idtipousuario})");
            }

            // Mostrar todos los datos al entrar (sin selección en el picker)
            tipoUsuarioSeleccionado = null;
            pickerTipoUsuario.SelectedItem = null;

            System.Diagnostics.Debug.WriteLine("?? Tipo seleccionado: (ninguno)");

            // Cargar visitas de todos los usuarios
            await CargarVisitasPorUsuario(null);

            // Rehabilitar el evento del picker
            pickerTipoUsuario.SelectedIndexChanged += OnTipoUsuarioSelected;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async Task CargarVisitasPorUsuario(int? idTipoUsuario)
    {
        try
        {
            // Mostrar indicador de carga
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            System.Diagnostics.Debug.WriteLine($"Cargando visitas. IdTipoUsuario: {idTipoUsuario?.ToString() ?? "TODOS"}");

            // INICIAR CRONOMETRO
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Obtener visitas de usuarios (solo usuarios con visitas > 0)
            var visitas = await visitasService.ObtenerVisitasPorUsuario(idTipoUsuario);

            // DETENER CRONOMETRO
            stopwatch.Stop();
            var tiempoAPI = stopwatch.Elapsed.TotalSeconds;

            System.Diagnostics.Debug.WriteLine($"API respondio en: {tiempoAPI:F2} segundos");
            System.Diagnostics.Debug.WriteLine($"Usuarios con visitas: {visitas.Count}");

            // MOSTRAR DATOS
            listavisitas = new ObservableCollection<VisitaUsuarioCLS>(visitas);
            OnPropertyChanged(nameof(listavisitas));

            // Ocultar indicador
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;

            System.Diagnostics.Debug.WriteLine($"API respondio en: {tiempoAPI:F2} segundos");
            System.Diagnostics.Debug.WriteLine($"Usuarios mostrados: {visitas.Count}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            
            await DisplayAlert("Error", $"Error al cargar visitas: {ex.Message}", "OK");
            
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private async void OnTipoUsuarioSelected(object sender, EventArgs e)
    {
        try
        {
            var tipoSeleccionado = pickerTipoUsuario.SelectedItem as TipoUsuarioSimpleCLS;
            if (tipoSeleccionado == null) return;

            System.Diagnostics.Debug.WriteLine($"?? Cambio de tipo de usuario: {tipoSeleccionado.nombre} (ID: {tipoSeleccionado.idtipousuario})");

            // ? Pasar directamente el ID del tipo de usuario
            await CargarVisitasPorUsuario(tipoSeleccionado.idtipousuario);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error en OnTipoUsuarioSelected: {ex.Message}");
            await DisplayAlert("Error", $"Error al filtrar: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnLimpiarFiltroClicked(object sender, EventArgs e)
    {
        try
        {
            pickerTipoUsuario.SelectedIndexChanged -= OnTipoUsuarioSelected;
            tipoUsuarioSeleccionado = null;
            pickerTipoUsuario.SelectedItem = null;
            pickerTipoUsuario.SelectedIndexChanged += OnTipoUsuarioSelected;

            await CargarVisitasPorUsuario(null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error en OnLimpiarFiltroClicked: {ex.Message}");
            await DisplayAlert("Error", $"Error al limpiar filtro: {ex.Message}", "OK");
        }
    }
}
