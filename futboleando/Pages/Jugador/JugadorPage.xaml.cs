//using futboleando.Pages.Jugador;
//using Android.Service.Carrier;
//using Android.Service.Carrier;
//using Android.Service.Carrier;
using futboleando.Service;
using futboleandoEntities.Jugador;
using System.Collections.ObjectModel;

namespace futboleando.Pages;

public partial class JugadorPage : ContentPage
{
    private readonly JugadorService jugadorService;
    public ObservableCollection<JugadorListCLS> listajugador { get; set; }
    public ObservableCollection<JugadorListCLS> listafiltro { get; set; }

    public JugadorListCLS objSeleccionado { get; set; }
    public string nombrejugador { get; set; }

    public JugadorPage(JugadorService _jugadorService)
	{
        InitializeComponent();
        jugadorService = _jugadorService;
        jugadorService.Onchange += refrezcarJugador;
        listajugador = new ObservableCollection<JugadorListCLS>();
        listarJugador();
        listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);
        BindingContext = this;
    }

    private async Task refrezcarJugador()
    {
        await listarJugador();
    }

    public async Task listarJugador()
    {
        var listaop = await jugadorService.listarJugador();   
        listajugador.Clear();
        foreach (var jugador in listaop)
        {
            listajugador.Add(jugador);
        }
        listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);
     
    }

    //private void entryNombreJugador_TextChanged(object sender, TextChangedEventArgs e)
    //{

    //}

    //private void lstJugadores_ItemTapped(object sender, ItemTappedEventArgs e)
    //{

    //}

    //private void swipeItemEliminar_Invoked(object sender, EventArgs e)
    //{

    //}
}