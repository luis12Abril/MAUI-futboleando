//using futboleando.Pages.Jugador;
//using Android.Service.Carrier;
//using Android.Service.Carrier;
//using Android.Service.Carrier;
using System.Collections.ObjectModel;
//using Android.Service.Carrier;
using futboleando.Service;
using futboleandoEntities.Jugador;

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
        BindingContext = this;
        _ = listarJugador();
        //listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);
       
    }

    private async Task refrezcarJugador()
    {
        await listarJugador();
    }

    public async Task listarJugador()
    {

        try
        {

            var listaop = await jugadorService.listarJugador();


            listajugador.Clear();
            foreach (var jugador in listaop.Take(30))
            {
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
                listajugador.Add(jugador);
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
            }
            listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);


            //if (listaop.Count == 0)
            //{
            //    await DisplayAlert("Debug", "No se recibieron datos de la API", "OK");
            //    return;
            //}
            //else
            //{
            //    //Console.WriteLine("Debug: Datos recibidos de la API");
            //    await DisplayAlert("debug ", "Se recibieron " + listaop.Count.ToString() + " datos de la API", "OK");
            //    //+listaop.Count.ToString() + "
            //}


            //Actualizar en el hilo principal de forma eficiente
            //await MainThread.InvokeOnMainThreadAsync(() =>
            //    {
            //        listajugador.Clear();

            //        // Agregar todos los elementos de una vez
            //        // TEMPORAL: Solo cargar los primeros 100 registros para probar
            //        foreach (var jugador in listaop.Take(30))
            //        {
            //            listajugador.Add(jugador);
            //        }
            //    });
            //listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);



            // Mostrar el alert DESPUÉS de cargar los datos
            //await DisplayAlert("debug ", "Se recibieron " + listaop.Count.ToString() + " datos de la API", "OK");

            //listajugador.Clear();
            //foreach (var jugador in listaop.Take(30))
            //{
            //    //await DisplayAlert("Debug ", jugador.nombre, "OK");
            //    listajugador.Add(jugador);
            //    //await DisplayAlert("Debug ", jugador.nombre, "OK");
            //}
            //listafiltro = new ObservableCollection<JugadorListCLS>(listajugador);

        }
        catch(Exception ex)
        {
            await DisplayAlert("Debug", "Error al conectar con la API: " + ex.Message, "OK");
            return;
        }



    }

    private void btnRegresar_Clicked(object sender, EventArgs e)
    {
        //App.Navigate.PopAsync();
        Navigation.PopAsync();
    }

    private void searchNombre_SearchButtonPressed(object sender, EventArgs e)
    {
        DisplayAlert("Alerta", "Buscar: " + nombrejugador, "OK");
    }



    private void entryNombreJugador_TextChanged(object sender, TextChangedEventArgs e)
    {
        ObservableCollection<JugadorListCLS> listaop;
        listajugador.Clear();

        if (nombrejugador == null || nombrejugador == "")
        {
            listaop = listafiltro;
        }
        else
        {
            var listaJugadorFiltrada = listafiltro.Where(j => j.nombrecompleto!.ToUpper().Contains(nombrejugador.ToUpper())).ToList();
            listaop = new ObservableCollection<JugadorListCLS>(listaJugadorFiltrada);
        }

        foreach (var item in listaop)
        {
            listajugador.Add(item);
        }


    }

    private void entryNombreJugador_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        ObservableCollection<JugadorListCLS> listaop;
        listajugador.Clear();

        if (nombrejugador == null || nombrejugador == "")
        {
            listaop = listafiltro;
        }
        else
        {
            var listaJugadorFiltrada = listafiltro.Where(j => j.nombrecompleto!.ToUpper().Contains(nombrejugador.ToUpper())).ToList();
            listaop = new ObservableCollection<JugadorListCLS>(listaJugadorFiltrada);
        }

        foreach (var item in listaop)
        {
            listajugador.Add(item);
        }
    }

    //private void lstJugadores_ItemTapped(object sender, ItemTappedEventArgs e)
    //{

    //}

    //private void swipeItemEliminar_Invoked(object sender, EventArgs e)
    //{

    //}
}