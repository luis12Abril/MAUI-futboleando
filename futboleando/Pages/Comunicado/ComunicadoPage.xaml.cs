using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Comunicado;
using futboleandoEntities.Equipo;

namespace futboleando.Pages.Comunicado;


public partial class ComunicadoPage : ContentPage
{
    private readonly ComunicadoService comunicadoService;
    public ObservableCollection<ComunicadoListCLS> listacomunicado { get; set; }
    public ObservableCollection<ComunicadoListCLS> listafiltro { get; set; }

    public ComunicadoListCLS objSeleccionado { get; set; }
    //public string nombreequipo { get; set; }
    public ComunicadoPage(ComunicadoService _comunicadoService)
	{
        InitializeComponent();
        comunicadoService = _comunicadoService;
        comunicadoService.Onchange += refrescarComunicado;
        listacomunicado = new ObservableCollection<ComunicadoListCLS>();
        BindingContext = this;
        _ = listarComunicado();
    }

    private async Task refrescarComunicado()
    {
        await listarComunicado();
    }

    public async Task listarComunicado()
    {

        try
        {

            var listaop = await comunicadoService.listarComunicado();


            listacomunicado.Clear();
            foreach (var comunicado in listaop)
            {
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
                listacomunicado.Add(comunicado);
                //await DisplayAlert("Debug ", jugador.nombre, "OK");
            }
            listafiltro = new ObservableCollection<ComunicadoListCLS>(listacomunicado);

        }
        catch (Exception ex)
        {
            await DisplayAlert("Debug", "Error al conectar con la API: " + ex.Message, "OK");
            return;
        }

    }
}