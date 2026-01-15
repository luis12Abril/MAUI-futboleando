using futboleandoEntities.Equipo;
using futboleandoEntities.Jugador;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Service
{
    public  class EquipoService
    {
        private ObservableCollection<EquipoListCLS> listaequipo;

        public event Func<Task> Onchange;

        public event Func<int, Task> OnGet;
        private readonly HttpClient _httpClient;

        public EquipoService(HttpClient httpClient) 
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

        public async Task<ObservableCollection<EquipoListCLS>> listarEquipo()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<EquipoListCLS>>("api/Equipo");
                if (response != null)
                {
                    return new ObservableCollection<EquipoListCLS>(response);
                }
                return new ObservableCollection<EquipoListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<EquipoListCLS>();
            }          
        }
    }
}
