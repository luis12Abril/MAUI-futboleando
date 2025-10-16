using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Torneo
{
    public int Idtorneo { get; set; }

    public string? Nombre { get; set; }

    public string? Clavetorneo { get; set; }

    public int? Habilitado { get; set; }

    public int? Visible { get; set; }

    public int? Ordentorneo { get; set; }

    public int? Visitas { get; set; }

    public int? Visitascel { get; set; }

    public int? Idliga { get; set; }
}
