using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Gol
{
    public int Idgol { get; set; }

    public int? Idjuego { get; set; }

    public int? Idequipo { get; set; }

    public int? Idjugador { get; set; }

    public int? Goles { get; set; }

    public int? Habilitado { get; set; }
}
