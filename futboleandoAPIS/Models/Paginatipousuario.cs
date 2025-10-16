using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Paginatipousuario
{
    public int Idpaginatipousuario { get; set; }

    public int? Idpagina { get; set; }

    public int? Idtipousuario { get; set; }

    public int? Habilitado { get; set; }
}
