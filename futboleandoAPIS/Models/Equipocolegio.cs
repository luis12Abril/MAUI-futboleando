using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Equipocolegio
{
    public int Idequipocolegio { get; set; }

    public string? Nombre { get; set; }

    public int? Idtorneocolegio { get; set; }

    public int? Idcolegioarbitro { get; set; }

    public int? Habilitado { get; set; }
}
