using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System.Threading.Tasks;

namespace futboleando.Pages.Colaborador;

public partial class ColaboradorFormPage : ContentPage
{
    public ColaboradorModel oColaboradorModel { get; set; }
    private readonly ColaboradorService colaboradorService;
    private readonly CiudadService ciudadService;
    //public CiudadListCLS opcionSeleccionadaCLS { get; set; }

    public ColaboradorFormPage(ColaboradorService _colaboradorService, CiudadService _ciudadService)
    {
        InitializeComponent();
        colaboradorService = _colaboradorService;
        ciudadService = _ciudadService;

        colaboradorService.OnGet += recuperarColaboradorPorId;

        oColaboradorModel = new ColaboradorModel();
       
        oColaboradorModel.oColaboradorFormCLS = new ColaboradorFormCLS();
        oColaboradorModel.opcionSeleccionadaCLS = new CiudadListCLS();
        BindingContext = this;

        // Inicializar de forma asíncrona
        InicializarAsync();              
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        colaboradorService.OnGet -= recuperarColaboradorPorId;
    }

    private async Task recuperarColaboradorPorId(int idcolaborador)
    {
        var listaciudad = await ciudadService.listarCiudad();
        ColaboradorFormCLS objcolaborador = await colaboradorService.recuperarColaboradorPorId(idcolaborador);
        oColaboradorModel.oColaboradorFormCLS = objcolaborador;
        oColaboradorModel.opcionSeleccionadaCLS = listaciudad.FirstOrDefault(x => x.idciudad == objcolaborador.idciudad);
        //opcionSeleccionadaCLS = listaciudad.FirstOrDefault(x => x.idciudad == oColaboradorModel.oColaboradorFormCLS.idciudad);
    }

    private async void InicializarAsync()
    {
        await listarCombos();
    }

    private async Task listarCombos()
    {
        var listaciudad = await ciudadService.listarCiudad();
        oColaboradorModel.listaciudad = listaciudad;
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        ColaboradorFormCLS objForm = oColaboradorModel.oColaboradorFormCLS;
        objForm.idciudad = oColaboradorModel.opcionSeleccionadaCLS.idciudad;
        int resp = await colaboradorService.guardarColaborador(objForm);

        if (resp == 0)
        {
            await DisplayAlert("Error", "Ocurrió un error al guardar el colaborador", "OK");

        }
        else
        {
            ciudadService.NotificarChange();
            await DisplayAlert("Éxito", "Colaborador guardado correctamente", "OK");
            //App.Current.MainPage = new Flyout();  
            // Regresar a la página anterior
            await App.Navigate.PopAsync();
        }

        //DisplayAlert("Valor", opcionSeleccionadaCLS.idciudad.ToString(), "OK");
    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        App.Navigate.PopAsync();    // Esta opcion me regresa a la pagina anterior
        // App.Current.MainPage = new Flyout();        Esta opcion me regresa al menu principal
    }
}   