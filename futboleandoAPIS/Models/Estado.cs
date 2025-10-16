using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Estado
{
    public int Idestado { get; set; }

    public string? Nombre { get; set; }

    public int? Habilitado { get; set; }
}
