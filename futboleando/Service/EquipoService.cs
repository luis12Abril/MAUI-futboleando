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

        // ✅ Método anterior - devuelve todos los equipos
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

        public async Task<ObservableCollection<EquipoListCLS>> listarEquipoResumen()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<EquipoListCLS>>("api/Equipo/Resumen");
                if (response != null)
                {
                    return new ObservableCollection<EquipoListCLS>(response);
                }

                return new ObservableCollection<EquipoListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<EquipoListCLS>();
            }
        }

        // ✅ Nuevo método - devuelve equipos por torneo
        public async Task<ObservableCollection<EquipoListCLS>> listarEquipoPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<EquipoListCLS>>($"api/Equipo/PorTorneo/{idTorneo}");
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

        public async Task<ObservableCollection<EquipoListCLS>> listarEquipoPorTorneoResumen(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<EquipoListCLS>>($"api/Equipo/PorTorneo/{idTorneo}/Resumen");
                if (response != null)
                {
                    return new ObservableCollection<EquipoListCLS>(response);
                }

                return new ObservableCollection<EquipoListCLS>();
            }
            catch (Exception)
            {
                return new ObservableCollection<EquipoListCLS>();
            }
        }

        public async Task<string> ObtenerFotoEquipo(int idEquipo)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<string>($"api/Equipo/{idEquipo}/Foto");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
