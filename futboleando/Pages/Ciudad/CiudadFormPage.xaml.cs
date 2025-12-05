using futboleando.Models;
using futboleando.Service;
using futboleandoEntities.Ciudad;
namespace futboleando.Pages.Ciudad;

public partial class CiudadFormPage : ContentPage
{
	public CiudadModel oCiudadModel { get; set; }
    public CiudadFormCLS oCiudadFormCLS { get; set; }
    private readonly CiudadService ciudadService;

    public CiudadFormPage(CiudadService _ciudadService)
	{
		InitializeComponent();
        oCiudadModel = new CiudadModel();
        ciudadService = _ciudadService;
        oCiudadModel.oCiudadFormCLS = new CiudadFormCLS() { nombreciudad = "A", descripcion ="B"};
		BindingContext = this;
    }

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        // es lo mismo que dar click en la flecha de arriba a la izquierda
        App.Navigate.PopAsync();
    }
}