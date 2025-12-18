//using Android.Service.Carrier;
using futboleando.Service;
using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Ciudad;

public partial class CiudadPage : ContentPage
{
	private readonly CiudadService ciudadService;
	public ObservableCollection<CiudadListCLS> listaCiudad { get; set; }
	public ObservableCollection<CiudadListCLS> listaFiltro;

	public CiudadListCLS objSeleccionado { get; set; }
    public string nombreciudadbuscar { get; set; }

    public CiudadPage(CiudadService _ciudadService)
	{
		InitializeComponent(); 
		ciudadService = _ciudadService;
		ciudadService.OnChange += refrescarCiudad;
        listarCiudad();		
		listaFiltro = new ObservableCollection<CiudadListCLS>(listaCiudad);
        BindingContext = this;
	}

    private async Task refrescarCiudad()
    {
        listaCiudad = await ciudadService.listarCiudad();
    }

    public async Task listarCiudad()
	{ 
		listaCiudad = await ciudadService.listarCiudad();
    }

	private void entrynombreciudad_TextChanged(object sender, TextChangedEventArgs e)
	{
		// DisplayAlert("Texto cambiado", e.NewTextValue, "OK");
		//DisplayAlert("Texto cambiado", nombreciudadbuscar, "OK");

		listaCiudad.Clear();
		ObservableCollection<CiudadListCLS> listaop;
        if (string.IsNullOrEmpty(nombreciudadbuscar))
		{
			listaop = listaFiltro;
			
		}
		else
		{
			var listaCiudadFiltrada = listaFiltro.Where(c => c.nombreciudad!.ToUpper().Contains(nombreciudadbuscar.ToUpper())).ToList();
			listaop = new ObservableCollection<CiudadListCLS>(listaCiudadFiltrada);
		}

        foreach (var item in listaop)
        {
            listaCiudad.Add(item);
        }
    }

    private void toolbarAdd_Clicked(object sender, EventArgs e)
    {
        //CiudadService ciudadService = MauiProgram.ServiceProvider.GetService<CiudadService>();
        CiudadFormPage oCiudadFormPage = new CiudadFormPage(ciudadService);
		Navigation.PushAsync(oCiudadFormPage);
    }

    private async void lstCiudad_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        CiudadFormPage oCiudadFormPage = new CiudadFormPage(ciudadService);

		int idciudad = objSeleccionado.idciudad;
		//CiudadFormCLS objCiudad = await ciudadService.recuperarCiudadPorId(idciudad);
		ciudadService.NotificarGet(idciudad);
		Navigation.PushAsync(oCiudadFormPage);

		//int idciudad = ((CiudadListCLS)e.Item).idciudad;
		//CiudadFormCLS oCiudadFormCLS = ciudadService.recuperarCiudadPorId(idciudad);
		//oCiudadFormPage.oCiudadModel.oCiudadFormCLS = oCiudadFormCLS;
		//ciudadService.NotificarGet((CiudadFormCLS)e.Item);
		//Navigation.PushAsync(oCiudadFormPage);
	}
}