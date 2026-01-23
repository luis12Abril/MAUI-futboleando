using futboleandoEntities.Jugador;
using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Equipo;
using futboleando.Models;
using System.ComponentModel;

namespace futboleando.Pages;

public partial class EquipoPage : ContentPage, INotifyPropertyChanged
{
    private readonly EquipoService equipoService;
    public ObservableCollection<EquipoIndexed> listaequipo { get; set; }
    public ObservableCollection<EquipoListCLS> listafiltro { get; set; }

    public EquipoListCLS objSeleccionado { get; set; }
    public string nombreequipo { get; set; }

    // Propiedad para el total de equipos
    private int _totalEquipos;
    public int TotalEquipos
    {
        get => _totalEquipos;
        set
        {
            _totalEquipos = value;
            OnPropertyChanged(nameof(TotalEquipos));
        }
    }
    
    public EquipoPage(EquipoService _equipoService)
	{
        InitializeComponent();
        equipoService = _equipoService;
        equipoService.Onchange += refrescarEquipo;
        listaequipo = new ObservableCollection<EquipoIndexed>();
        BindingContext = this;
        _ = listarEquipo();
    }

    private async Task refrescarEquipo()
    {
        await listarEquipo();
    }

    public async Task listarEquipo()
    {
        try
        {
            // Obtener el ID del torneo seleccionado desde Preferences
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            ObservableCollection<EquipoListCLS> listaop;

            if (idTorneoSeleccionado > 0)
            {
                // Obtener equipos del torneo seleccionado
                listaop = await equipoService.listarEquipoPorTorneo(idTorneoSeleccionado);
            }
            else
            {
                // Si no hay torneo seleccionado, obtener todos
                listaop = await equipoService.listarEquipo();
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                listaequipo.Clear();
                int index = 1;
                foreach (var equipo in listaop)
                {
                    listaequipo.Add(new EquipoIndexed 
                    { 
                        Index = index++, 
                        Equipo = equipo 
                    });
                }
                
                // Actualizar contador
                TotalEquipos = listaequipo.Count;
            });

            listafiltro = new ObservableCollection<EquipoListCLS>(listaop.ToList());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al conectar con la API: " + ex.Message, "OK");
            return;
        }
    }
}