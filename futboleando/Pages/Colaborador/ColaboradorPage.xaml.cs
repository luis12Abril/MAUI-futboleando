using futboleando.Service;
using futboleandoEntities.Ciudad;
using futboleandoEntities.Colaborador;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Colaborador;

public partial class ColaboradorPage : ContentPage
{
	public ObservableCollection<CiudadListCLS> listaciudad { get; set; }
    public ObservableCollection<ColaboradorListCLS> listacolaborador { get; set; }
    public ObservableCollection<ColaboradorListCLS> listafiltro { get; set; }

    public ColaboradorListCLS objSeleccionado { get; set; }

    private ObservableCollection<ColaboradorListCLS> listafiltro2;
    public CiudadListCLS oCiudadListCLS { get; set; }

    private ColaboradorService colaboradorService;
    private CiudadService ciudadService;
    public string nombrecolaborador { get; set; }

    public ColaboradorPage(CiudadService _ciudadService, ColaboradorService _colaboradorService )
	{
		InitializeComponent();
        colaboradorService = _colaboradorService;
        ciudadService = _ciudadService;

        listarCursos();

        listafiltro = new ObservableCollection<ColaboradorListCLS>(listacolaborador);
        BindingContext = this;
    }

    public async Task listarCursos()
    {
        CiudadListCLS primerItem = new CiudadListCLS { idciudad = 0, nombreciudad = "-- Todos --", descripcion = "Descripcion de Ciudad C" };

        listaciudad = new ObservableCollection<CiudadListCLS>(await ciudadService.listarCiudad());
        listaciudad.Insert(0, primerItem);
        oCiudadListCLS = primerItem;

        listacolaborador = await colaboradorService.listarColaborador(); 
    }

    private void pickerCiudad_SelectedIndexChanged(object sender, EventArgs e)
    {
        string nombreciudad = oCiudadListCLS.nombreciudad;

        if(nombreciudad == "-- Todos --")
        {
            listacolaborador.Clear();
            foreach (var colaborador in listafiltro)
            {
                listacolaborador.Add(colaborador);
            }
        }
        else
        {
            var colaboradoresFiltrados = listafiltro.Where(p => p.nombreciudad == nombreciudad).ToList();
            listacolaborador.Clear();
            foreach (var colaborador in colaboradoresFiltrados)
            {
                listacolaborador.Add(colaborador);
            }
        }

        // DisplayAlert("Ciudad Seleccionada", $"Ciudad: {oCiudadListCLS.nombreciudad}", "OK");
    }

    private void searchNombre_SearchButtonPressed(object sender, EventArgs e)
    {
        ObservableCollection<ColaboradorListCLS> listaop;   
        listacolaborador.Clear();
        if (nombrecolaborador == null || nombrecolaborador == "")
        {
            listaop = listafiltro;
        }
        else
        {
            var listaColaboradorFiltrada = listafiltro.Where(c => c.nombre!.ToUpper().Contains(nombrecolaborador.ToUpper()) || c.appaterno!.ToUpper().Contains(nombrecolaborador.ToUpper()) || c.apmaterno!.ToUpper().Contains(nombrecolaborador.ToUpper())).ToList();
            listaop = new ObservableCollection<ColaboradorListCLS>(listaColaboradorFiltrada);
        }
        foreach (var item in listaop)
        {
            if(!listacolaborador.Contains(item))
            {
                listacolaborador.Add(item);
            }
        }

    }

    private void toolbarAdd_Clicked(object sender, EventArgs e)
    {        
        ColaboradorFormPage oColaboradorFormPage = new ColaboradorFormPage(colaboradorService, ciudadService);
        Navigation.PushAsync(oColaboradorFormPage);
    }

    private void lstColaborador_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var instancia = new ColaboradorFormPage(colaboradorService, ciudadService);
        int idcolaborador = objSeleccionado.idcolaborador;
        colaboradorService.NotificarGet(idcolaborador);
        App.Navigate.PushAsync(instancia);
    }
}