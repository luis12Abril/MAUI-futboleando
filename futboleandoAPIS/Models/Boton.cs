using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Boton
{
    public int Idboton { get; set; }

    public string? Nombre { get; set; }

    public int? Habilitado { get; set; }
}
