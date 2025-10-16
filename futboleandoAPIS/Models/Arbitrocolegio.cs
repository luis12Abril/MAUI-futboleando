using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Arbitrocolegio
{
    public int Idarbitrocolegio { get; set; }

    public string? Nombre { get; set; }

    public string? Appaterno { get; set; }

    public string? Apmaterno { get; set; }

    public DateTime? Fnacimiento { get; set; }

    public int? Peso { get; set; }

    public string? Fotoarbitro { get; set; }

    public int? Idcolegioarbitro { get; set; }

    public string? Nomusuario { get; set; }

    public string? Codigoactual { get; set; }

    public int? Habilitado { get; set; }
}
