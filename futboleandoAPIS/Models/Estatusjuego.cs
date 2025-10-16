using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Estatusjuego
{
    public int Idestatusjuego { get; set; }

    public string? Nombre { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Idtorneo { get; set; }
}
