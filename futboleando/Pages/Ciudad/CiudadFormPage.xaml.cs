using futboleandoEntities.Ciudad;
namespace futboleando.Pages.Ciudad;

public partial class CiudadFormPage : ContentPage
{
	public CiudadFormCLS oCiudadFormCLS { get; set; }
    public CiudadFormPage()
	{
		InitializeComponent();
		oCiudadFormCLS = new CiudadFormCLS() { nombreciudad = "A", descripcion ="B"};
		BindingContext = this;
    }
}