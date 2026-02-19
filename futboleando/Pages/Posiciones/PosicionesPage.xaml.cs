using System.Collections.ObjectModel;
using System.ComponentModel;
using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Equipo;

namespace futboleando.Pages.Posiciones;

public partial class PosicionesPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    
    private ObservableCollection<PosicionModel> _listaPosiciones;
    public ObservableCollection<PosicionModel> ListaPosiciones
    {
        get => _listaPosiciones;
        set
        {
            _listaPosiciones = value;
            OnPropertyChanged(nameof(ListaPosiciones));
        }
    }

    public PosicionesPage(EquipoService _equipoService)
    {
        InitializeComponent();
        equipoService = _equipoService;
        ListaPosiciones = new ObservableCollection<PosicionModel>();
        BindingContext = this;
        _ = CargarTablaPosiciones();
    }

    private async Task CargarTablaPosiciones()
    {
        try
        {
            // Obtener el ID del torneo seleccionado desde Preferences
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            ObservableCollection<EquipoListCLS> listaEquipos;

            if (idTorneoSeleccionado > 0)
            {
                // Obtener equipos del torneo seleccionado
                listaEquipos = await equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);
            }
            else
            {
                // Si no hay torneo seleccionado, obtener todos
                listaEquipos = await equipoService.listarEquipo();
            }

            // Ordenar por: Puntos DESC, DifGoles DESC, GolesAFavor DESC, Nombre ASC
            var equiposOrdenados = listaEquipos
                .OrderByDescending(e => e.puntos ?? 0)
                .ThenByDescending(e => e.difgoles ?? 0)
                .ThenByDescending(e => e.golesafavor ?? 0)
                .ThenBy(e => e.nombre)
                .ToList();

            // Crear la lista de posiciones con número consecutivo
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ListaPosiciones.Clear();
                int posicion = 1;
                foreach (var equipo in equiposOrdenados)
                {
                    ListaPosiciones.Add(new PosicionModel
                    {
                        Posicion = posicion++,
                        Equipo = equipo
                    });
                }
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al cargar tabla de posiciones: " + ex.Message, "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Recargar datos al aparecer la página
        _ = CargarTablaPosiciones();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // ========== EVENTOS DE TABS ==========
    
    private void OnVista1Tapped(object sender, EventArgs e)
    {
        CambiarVista(1);
    }

    private void OnVista2Tapped(object sender, EventArgs e)
    {
        CambiarVista(2);
    }

    private void OnVista3Tapped(object sender, EventArgs e)
    {
        CambiarVista(3);
    }

    private void CambiarVista(int numeroVista)
    {
        // Ocultar todos los contenidos
        contenidoVista1.IsVisible = false;
        contenidoVista2.IsVisible = false;
        contenidoVista3.IsVisible = false;

        // Restablecer estilo de todos los tabs
        vista1Border.BackgroundColor = Colors.Transparent;
        vista2Border.BackgroundColor = Colors.Transparent;
        vista3Border.BackgroundColor = Colors.Transparent;

        // Restablecer color de texto de todos los labels
        if (vista1Border.Content is Label lbl1) lbl1.TextColor = Colors.White;
        if (vista2Border.Content is Label lbl2) lbl2.TextColor = Colors.White;
        if (vista3Border.Content is Label lbl3) lbl3.TextColor = Colors.White;

        // Mostrar vista seleccionada y resaltar tab
        switch (numeroVista)
        {
            case 1:
                contenidoVista1.IsVisible = true;
                vista1Border.BackgroundColor = Colors.White;
                if (vista1Border.Content is Label lblActivo1) lblActivo1.TextColor = Color.FromArgb("#1E88E5");
                break;
            case 2:
                contenidoVista2.IsVisible = true;
                vista2Border.BackgroundColor = Colors.White;
                if (vista2Border.Content is Label lblActivo2) lblActivo2.TextColor = Color.FromArgb("#1E88E5");
                break;
            case 3:
                contenidoVista3.IsVisible = true;
                vista3Border.BackgroundColor = Colors.White;
                if (vista3Border.Content is Label lblActivo3) lblActivo3.TextColor = Color.FromArgb("#1E88E5");
                break;
        }
    }
}