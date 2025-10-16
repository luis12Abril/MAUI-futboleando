using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Arbitro
{
    public int Idarbitro { get; set; }

    public string? Nombre { get; set; }

    public string? Appaterno { get; set; }

    public string? Apmaterno { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Idtorneo { get; set; }
}
