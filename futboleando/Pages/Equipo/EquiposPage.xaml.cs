using futboleandoEntities.Jugador;
using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Equipo;

namespace futboleando.Pages;

public partial class EquiposPage : ContentPage
{
    private readonly EquipoService equipoService;
    public ObservableCollection<EquipoListCLS> listaequipo { get; set; }
    public ObservableCollection<EquipoListCLS> listafiltro { get; set; }

    public EquipoListCLS objSeleccionado { get; set; }
    public string nombreequipo { get; set; }
    public EquiposPage(EquipoService _equipoService)
	{
        InitializeComponent();
        equipoService = _equipoService;
        equipoService.Onchange += refrescarEquipo;
        listaequipo = new ObservableCollection<EquipoListCLS>();
        BindingContext = this;
        _ = listarEquipo();
        //listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);
    }

    private async Task refrescarEquipo()
    {
        await listarEquipo();
    }

    public async Task listarEquipo()
    {

        try
        {

            var listaop = await equipoService.listarEquipo();


            listaequipo.Clear();
            foreach (var jugador in listaop.Take(30))
            {
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
                listaequipo.Add(jugador);
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
            }
            listafiltro = new ObservableCollection<EquipoListCLS>(listaequipo);

        }
        catch (Exception ex)
        {
            await DisplayAlert("Debug", "Error al conectar con la API: " + ex.Message, "OK");
            return;
        }

    }

}