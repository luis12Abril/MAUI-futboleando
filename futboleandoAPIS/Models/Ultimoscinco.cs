using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Ultimoscinco
{
    public int Idultimoscinco { get; set; }

    public int? Idequipo { get; set; }

    public string? Nombre { get; set; }

    public int? Puntos { get; set; }

    public string? Ultimo { get; set; }

    public string? C2 { get; set; }

    public string? C3 { get; set; }

    public string? C4 { get; set; }

    public string? C5 { get; set; }
}
