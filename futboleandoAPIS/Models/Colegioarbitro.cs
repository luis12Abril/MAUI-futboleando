using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Colegioarbitro
{
    public int Idcolegioarbitro { get; set; }

    public string? Nombre { get; set; }

    public int? Idpresidente { get; set; }

    public int? Habilitado { get; set; }
}
