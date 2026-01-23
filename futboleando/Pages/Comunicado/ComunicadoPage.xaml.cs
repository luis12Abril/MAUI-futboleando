using System.Collections.ObjectModel;
using futboleando.Service;
using futboleandoEntities.Comunicado;
using futboleandoEntities.Equipo;

namespace futboleando.Pages.Comunicado;

public partial class ComunicadoPage : ContentPage
{
    private readonly ComunicadoService comunicadoService;
    public ObservableCollection<ComunicadoListCLS> listacomunicado { get; set; }
    public ObservableCollection<ComunicadoListCLS> listafiltro { get; set; }

    public ComunicadoListCLS objSeleccionado { get; set; }
    
    public ComunicadoPage(ComunicadoService _comunicadoService)
    {
        InitializeComponent();
        comunicadoService = _comunicadoService;
        comunicadoService.Onchange += refrescarComunicado;
        listacomunicado = new ObservableCollection<ComunicadoListCLS>();
        BindingContext = this;
        _ = listarComunicado();
    }

    private async Task refrescarComunicado()
    {
        await listarComunicado();
    }

    public async Task listarComunicado()
    {
        try
        {
            // Obtener el ID del torneo seleccionado desde Preferences
            var idTorneoSeleccionado = Preferences.Get("UltimoTorneo", 0);

            ObservableCollection<ComunicadoListCLS> listaop;

            if (idTorneoSeleccionado > 0)
            {
                // Obtener comunicados del torneo seleccionado
                listaop = await comunicadoService.listarComunicadoPorTorneo(idTorneoSeleccionado);
            }
            else
            {
                // Si no hay torneo seleccionado, obtener todos
                listaop = await comunicadoService.listarComunicado();
            }

            listacomunicado.Clear();
            foreach (var comunicado in listaop)
            {
                listacomunicado.Add(comunicado);
            }
            listafiltro = new ObservableCollection<ComunicadoListCLS>(listacomunicado);

            // ? Mostrar u ocultar el mensaje de "sin comunicados"
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var listView = this.FindByName<ListView>("listViewComunicados");
                var frameNoData = this.FindByName<Frame>("frameNoComunicados");

                if (listacomunicado.Count == 0)
                {
                    // No hay comunicados: mostrar mensaje
                    if (listView != null) listView.IsVisible = false;
                    if (frameNoData != null) frameNoData.IsVisible = true;
                }
                else
                {
                    // Hay comunicados: mostrar lista
                    if (listView != null) listView.IsVisible = true;
                    if (frameNoData != null) frameNoData.IsVisible = false;
                }
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al conectar con la API: " + ex.Message, "OK");
            return;
        }
    }
}