using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Colaborador;

namespace futboleando.Pages.Colaborador;

public partial class ColaboradorFormPage : ContentPage
{
    public ColaboradorModel oColaboradorModel { get; set; }
    private readonly ColaboradorService colaboradorService;
    public ColaboradorFormPage(ColaboradorService _colaboradorService)
    {
        InitializeComponent();
        colaboradorService = _colaboradorService;
        oColaboradorModel = new ColaboradorModel();
        BindingContext = this;
        oColaboradorModel.oColaboradorFormCLS = new ColaboradorFormCLS() 
        {
            idcolaborador = 0,
            nombre = "Luis",
            appaterno = string.Empty,
            apmaterno = string.Empty,
            edad = 35            
        };
    }

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        App.Navigate.PopAsync();
    }
}   