using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Municipio
{
    public int Idmunicipio { get; set; }

    public string? Nombre { get; set; }

    public int? Idestado { get; set; }

    public int? Habilitado { get; set; }
}
