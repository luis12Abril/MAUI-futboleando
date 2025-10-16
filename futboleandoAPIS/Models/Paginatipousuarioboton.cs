using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Paginatipousuarioboton
{
    public int Idpaginatipousuarioboton { get; set; }

    public int? Idpaginatipousuario { get; set; }

    public int? Idboton { get; set; }

    public int? Habilitado { get; set; }
}
