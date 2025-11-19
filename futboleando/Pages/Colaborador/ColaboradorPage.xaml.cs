using futboleandoEntities.Ciudad;
using System.Collections.ObjectModel;

namespace futboleando.Pages.Colaborador;

public partial class ColaboradorPage : ContentPage
{
	public ObservableCollection<CiudadListCLS> listaciudad { get; set; }
	public CiudadListCLS oCiudadListCLS { get; set; }
    public ColaboradorPage()
	{
		InitializeComponent();
		CiudadListCLS primerItem = new CiudadListCLS { idciudad = 0, nombreciudad = "-- Seleccione --", descripcion = "Descripcion de Ciudad C" };
        listaciudad = new ObservableCollection<CiudadListCLS>() 
		{
			new CiudadListCLS { idciudad = 1, nombreciudad = "Cd. Obregón", descripcion = "Descripcion de Ciudad A" },
			new CiudadListCLS { idciudad = 2, nombreciudad = "Hermosillo", descripcion = "Descripcion de Ciudad B" }
        };
		listaciudad.Insert(0, primerItem);
		oCiudadListCLS = primerItem;
        BindingContext = this;
    }
}