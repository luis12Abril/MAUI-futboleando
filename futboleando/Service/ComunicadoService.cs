using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using futboleandoEntities.Comunicado;
using futboleandoEntities.Equipo;

namespace futboleando.Service
{

    public class ComunicadoService
    {
        private ObservableCollection<ComunicadoListCLS> listacomunicado;

        public event Func<Task> Onchange;

        public event Func<int, Task> OnGet;
        private readonly HttpClient _httpClient;

        public ComunicadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void notificarChange()
        {
            Onchange?.Invoke();
        }

        public void notificarGet(int id)
        {
            OnGet?.Invoke(id);
        }

        public async Task<ObservableCollection<ComunicadoListCLS>> listarComunicado()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ComunicadoListCLS>>("api/Comunicado");
                if (response != null)
                {
                    return new ObservableCollection<ComunicadoListCLS>(response);
                }
                return new ObservableCollection<ComunicadoListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<ComunicadoListCLS>();
            }
        }
    } 
}
