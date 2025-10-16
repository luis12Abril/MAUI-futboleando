using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Campocolegio
{
    public int Idcampocolegio { get; set; }

    public string? Nombre { get; set; }

    public int? Idcolegioarbitro { get; set; }

    public string? Ubicacion { get; set; }

    public int? Habilitado { get; set; }
}
