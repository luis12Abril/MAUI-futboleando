using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Comentario
{
    public int Idcomentario { get; set; }

    public int? Idjuego { get; set; }

    public string? Comentario1 { get; set; }

    public int? Idusuario { get; set; }

    public DateTime? Fechacomentario { get; set; }

    public int? Habilitado { get; set; }
}
