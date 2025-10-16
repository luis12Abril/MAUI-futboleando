using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Tipousuario
{
    public int Idtipousuario { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Habilitado { get; set; }
}
