namespace futboleandoEntities.Comentario;

public class ComentarioCLS
{
    public int idcomentario { get; set; }
    public int idjuego { get; set; }
    public string comentario { get; set; } = string.Empty;
    public int idusuario { get; set; }
    public string nombreusuario { get; set; } = string.Empty;
    public DateTime fechacomentario { get; set; }
}
