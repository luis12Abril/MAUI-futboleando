using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Jornadum
{
    public int Idjornada { get; set; }

    public string? Nombre { get; set; }

    public DateOnly? Finiciojornada { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Idtorneo { get; set; }
}
