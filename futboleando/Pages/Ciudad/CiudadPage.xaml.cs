using futboleando.Service;
using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Ciudad;

public partial class CiudadPage : ContentPage
{
	private readonly CiudadService ciudadService;
	public ObservableCollection<CiudadListCLS> listaCiudad { get; set; }
	public string nombreciudadbuscar { get; set; }
    public CiudadPage(CiudadService _ciudadService)
	{
		InitializeComponent(); 
		ciudadService = _ciudadService;
		listaCiudad = ciudadService.listarciudad();
		BindingContext = this;
	}

    private void entrynombreciudad_TextChanged(object sender, TextChangedEventArgs e)
    {
		// DisplayAlert("Texto cambiado", e.NewTextValue, "OK");
        DisplayAlert("Texto cambiado", nombreciudadbuscar, "OK");
    }
}