namespace futboleando.Models;

public class UltimosCincoJuegosEquipoModel
{
    public string EquipoNombre { get; set; } = string.Empty;
    public int Puntos { get; set; }
    public string Ultimo { get; set; } = "-";
    public string Juego2 { get; set; } = "-";
    public string Juego3 { get; set; } = "-";
    public string Juego4 { get; set; } = "-";
    public string Juego5 { get; set; } = "-";
}
