using System.Collections.ObjectModel;
using System.ComponentModel;
using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Juego;

namespace futboleando.Pages.Juego;

public partial class UltimosCincoJuegosPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    private readonly JuegoService juegoService;
    private bool datosYaCargados;
    private bool _isNavigatingBack;
    private int _ultimoTorneoCargado;

    private ObservableCollection<UltimosCincoJuegosEquipoModel> _listaUltimosCinco;
    public ObservableCollection<UltimosCincoJuegosEquipoModel> ListaUltimosCinco
    {
        get => _listaUltimosCinco;
        set
        {
            _listaUltimosCinco = value;
            OnPropertyChanged(nameof(ListaUltimosCinco));
        }
    }

    public UltimosCincoJuegosPage(EquipoService _equipoService, JuegoService _juegoService)
    {
        InitializeComponent();
        equipoService = _equipoService;
        juegoService = _juegoService;
        ListaUltimosCinco = new ObservableCollection<UltimosCincoJuegosEquipoModel>();
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
            loadingIndicator.IsRunning = true;
            loadingIndicator.IsVisible = true;

            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);
            var nombreTorneo = Preferences.Get("NombreTorneo", "Sin torneo");
            lblTorneoNombre.Text = $"Torneo: {nombreTorneo}";

            if (datosYaCargados && _ultimoTorneoCargado == idTorneoSeleccionado)
            {
                return;
            }

            if (idTorneoSeleccionado == 0)
            {
                await DisplayAlert("Aviso", "No hay un torneo seleccionado", "OK");
                ListaUltimosCinco.Clear();
                lblTotalEquipos.Text = "Total equipos: 0";
                return;
            }

            var equipos = await equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);
            var juegos = await juegoService.ListarJuegosPorTorneo(idTorneoSeleccionado);

            var juegosJugados = juegos
                .Where(j => !string.IsNullOrWhiteSpace(j.nombreestatusjuego)
                    && j.nombreestatusjuego.Trim().Contains("JUGADO", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var equiposOrdenados = equipos
                .OrderByDescending(e => (e.puntos ?? 0) + (e.puntosextras ?? 0))
                .ThenBy(e => e.nombre)
                .ToList();

            ListaUltimosCinco.Clear();

            foreach (var equipo in equiposOrdenados)
            {
                var juegosEquipo = juegosJugados
                    .Where(j => j.idequipo01 == equipo.idequipo || j.idequipo02 == equipo.idequipo)
                    .OrderByDescending(j => j.fhorario ?? DateTime.MinValue)
                    .Take(5)
                    .ToList();

                var resultados = juegosEquipo
                    .Select(j => ObtenerResultadoEquipo(j, equipo.idequipo))
                    .ToList();

                while (resultados.Count < 5)
                {
                    resultados.Add("-");
                }

                ListaUltimosCinco.Add(new UltimosCincoJuegosEquipoModel
                {
                    EquipoNombre = equipo.nombre,
                    Puntos = (equipo.puntos ?? 0) + (equipo.puntosextras ?? 0),
                    Ultimo = resultados.ElementAtOrDefault(0) ?? "-",
                    Juego2 = resultados.ElementAtOrDefault(1) ?? "-",
                    Juego3 = resultados.ElementAtOrDefault(2) ?? "-",
                    Juego4 = resultados.ElementAtOrDefault(3) ?? "-",
                    Juego5 = resultados.ElementAtOrDefault(4) ?? "-"
                });
            }

            lblTotalEquipos.Text = $"Total equipos: {ListaUltimosCinco.Count}";
            _ultimoTorneoCargado = idTorneoSeleccionado;
            datosYaCargados = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"? Error al cargar últimos cinco: {ex.Message}");
            await DisplayAlert("Error", $"Error al cargar datos: {ex.Message}", "OK");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
        }
    }

    private static string ObtenerResultadoEquipo(JuegoListCLS juego, int idEquipo)
    {
        if (juego.idequipo01 == idEquipo)
        {
            return NormalizarResultado(juego.resequipo01, juego.golesequipo01, juego.golesequipo02);
        }

        if (juego.idequipo02 == idEquipo)
        {
            return NormalizarResultado(juego.resequipo02, juego.golesequipo02, juego.golesequipo01);
        }

        return "-";
    }

    private static string NormalizarResultado(string? resultado, int? golesFavor, int? golesContra)
    {
        if (!string.IsNullOrWhiteSpace(resultado))
        {
            return resultado.Trim();
        }

        if (golesFavor.HasValue && golesContra.HasValue)
        {
            if (golesFavor > golesContra)
            {
                return "G";
            }

            if (golesFavor < golesContra)
            {
                return "P";
            }

            return "E";
        }

        return "-";
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
            System.Diagnostics.Debug.WriteLine($"? Error al regresar: {ex.Message}");
        }
        finally
        {
            _isNavigatingBack = false;
        }
    }
}
