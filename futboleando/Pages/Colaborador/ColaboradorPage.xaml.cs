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
    public CiudadListCLS oCiudadListCLS { get; set; }

    private ColaboradorService colaboradorService;
    private CiudadService ciudadService;

    public ColaboradorPage(CiudadService _ciudadService, ColaboradorService _colaboradorService )
	{
		InitializeComponent();
        colaboradorService = _colaboradorService;
        ciudadService = _ciudadService;

        CiudadListCLS primerItem = new CiudadListCLS { idciudad = 0, nombreciudad = "-- Todos --", descripcion = "Descripcion de Ciudad C" };
        listaciudad = ciudadService.listarciudad();
        listacolaborador = colaboradorService.listarcolaborador();

        listafiltro = new ObservableCollection<ColaboradorListCLS>(listacolaborador);
        listaciudad.Insert(0, primerItem);
		oCiudadListCLS = primerItem;
        BindingContext = this;
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
}