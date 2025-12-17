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
    public CiudadListCLS opcionSeleccionadaCLS { get; set; }

    public ColaboradorFormPage(ColaboradorService _colaboradorService, CiudadService _ciudadService)
    {
        InitializeComponent();
        colaboradorService = _colaboradorService;
        ciudadService = _ciudadService;
        oColaboradorModel = new ColaboradorModel();
       
        oColaboradorModel.oColaboradorFormCLS = new ColaboradorFormCLS();
        BindingContext = this;

        // Inicializar de forma asíncrona
        InicializarAsync();              
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
        objForm.idciudad = opcionSeleccionadaCLS.idciudad;
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
        App.Navigate.PopAsync();
    }
}   