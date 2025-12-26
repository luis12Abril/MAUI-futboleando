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
        ciudadService.OnGet += recuperarCarrera;
        oCiudadModel.oCiudadFormCLS = new CiudadFormCLS();
		BindingContext = this;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ciudadService.OnGet -= recuperarCarrera;
    }

    private async Task recuperarCarrera(int id)
    {
        CiudadFormCLS objCiudad = await ciudadService.recuperarCiudadPorId(id);
        oCiudadModel.oCiudadFormCLS = objCiudad;
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Confirmar", "¿Desea guardar la ciudad?", "Sí", "No");
        if (confirmar)
        {
            int respuesta = await ciudadService.guardarCiudad(oCiudadModel.oCiudadFormCLS);
            if (respuesta == 0)
            {
                await DisplayAlert("Error", "Ocurrió un error al guardar la ciudad", "OK");

            }
            else
            {
                ciudadService.NotificarChange();
                await DisplayAlert("Éxito", "Ciudad guardada correctamente", "OK");
                // Regresar a la página anterior
                await App.Navigate.PopAsync();
            }
        }

        
    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        // es lo mismo que dar click en la flecha de arriba a la izquierda
        App.Navigate.PopAsync();
    }
}