using System.Net.Http.Json;
using futboleandoEntities.Comentario;

namespace futboleando.Service;

public class ComentarioService
{
    private readonly HttpClient _httpClient;

    public ComentarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ComentarioCLS>> ListarComentariosPorJuego(int idJuego)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<ComentarioCLS>>($"api/Comentario/PorJuego/{idJuego}");
            return response ?? new List<ComentarioCLS>();
        }
        catch
        {
            return new List<ComentarioCLS>();
        }
    }

    public async Task<ComentarioCLS?> AgregarComentario(ComentarioCreateCLS comentario)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Comentario", comentario);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ComentarioCLS>();
        }
        catch
        {
            return null;
        }
    }
}
