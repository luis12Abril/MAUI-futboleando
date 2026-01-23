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
    public class JugadorService
    {
        private ObservableCollection<JugadorListCLS> listajugador;

        public event Func<Task> Onchange;

        public event Func<int, Task> OnGet;
        private readonly HttpClient _httpClient;
        public JugadorService(HttpClient httpClient)
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

        // ✅ Método anterior - devuelve todos los jugadores
        public async Task<ObservableCollection<JugadorListCLS>> listarJugador()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JugadorListCLS>>("api/Jugador");
                if (response != null)
                {
                    return new ObservableCollection<JugadorListCLS>(response);
                }
                return new ObservableCollection<JugadorListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JugadorListCLS>();
            }
        }

        // ✅ Nuevo método - devuelve jugadores por torneo
        public async Task<ObservableCollection<JugadorListCLS>> listarJugadorPorTorneo(int idTorneo)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<JugadorListCLS>>($"api/Jugador/PorTorneo/{idTorneo}");
                if (response != null)
                {
                    return new ObservableCollection<JugadorListCLS>(response);
                }
                return new ObservableCollection<JugadorListCLS>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<JugadorListCLS>();
            }
        }

        public async Task<JugadorFormCLS> recuperarJugadorPorId(int idjugador)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<JugadorFormCLS>("api/Jugador" + idjugador);
                if (response != null)
                {
                    return response;
                }
                return new JugadorFormCLS();
            }
            catch (Exception ex)
            {
                return new JugadorFormCLS();
            }
        }

        public async Task<int> guardarJugador(JugadorFormCLS oJugadorFormCLS)
        {
            try
            {
                listajugador.Add(new JugadorListCLS
                {
                    nombre = oJugadorFormCLS.nombre,
                    appaterno = oJugadorFormCLS.appaterno
                });
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> eliminarJugador(int idjugador)
        {
            try
            {
                listajugador = new ObservableCollection<JugadorListCLS>(
                    listajugador.Where(p => p.idjugador != idjugador));
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
