using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Comunicado
{
    public int Idcomunicado { get; set; }

    public DateOnly? Fechacomunicado { get; set; }

    public string? Comunicadocorto { get; set; }

    public string? Comunicadolargo { get; set; }

    public int? Idtorneo { get; set; }

    public int? Habilitado { get; set; }
}
