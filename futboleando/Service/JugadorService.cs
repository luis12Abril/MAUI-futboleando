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
            listajugador = new ObservableCollection<JugadorListCLS>()
            {
                new JugadorListCLS{idjugador=1 , nombre="Luis", appaterno="Barreras"},
                new JugadorListCLS{idjugador=2 , nombre="Angel", appaterno="Garcia"}
            };

        }

        public void notificarChange()
        {
            Onchange?.Invoke();
        }

        public void notificarGet(int id)
        {
            OnGet?.Invoke(id);
        }

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
            //return listajugador;
        }

        public async Task<JugadorFormCLS> recuperarJugadorPorId(int idjugador)
        {
            try
            {
                JugadorFormCLS oJugadorFormCLS = new JugadorFormCLS();
                JugadorListCLS oJugadorListCLS = listajugador.Where(p => p.idjugador == idjugador).FirstOrDefault();
                oJugadorFormCLS.nombre = oJugadorListCLS.nombre;
                oJugadorFormCLS.appaterno = oJugadorListCLS.appaterno;
                return oJugadorFormCLS;
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
