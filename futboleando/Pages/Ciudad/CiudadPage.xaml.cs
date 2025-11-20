using futboleando.Service;
using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Ciudad;

public partial class CiudadPage : ContentPage
{
	private readonly CiudadService ciudadService;
	//public ObservableCollection<CiudadCLS> listaCiudad { get; set; }
    public CiudadPage(CiudadService _ciudadService)
	{
		InitializeComponent();
	}
}