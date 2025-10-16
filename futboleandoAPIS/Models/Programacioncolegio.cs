using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Programacioncolegio
{
    public int Idprogramacioncolegio { get; set; }

    public int? Idarbitrocolegio { get; set; }

    public int? Idcolegioarbitro { get; set; }

    public int? Idequipocolegio01 { get; set; }

    public int? Idequipocolegio02 { get; set; }

    public int? Idcampocolegio { get; set; }

    public DateTime? Fjuegocolegio { get; set; }

    public string? Comentario { get; set; }

    public int? Habilitado { get; set; }
}
