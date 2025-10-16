using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Liga
{
    public int Idliga { get; set; }

    public string? Nombre { get; set; }

    public int? Idmunicipio { get; set; }

    public int? Habilitado { get; set; }
}
