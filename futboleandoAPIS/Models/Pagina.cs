using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Pagina
{
    public int Idpagina { get; set; }

    public string? Mensaje { get; set; }

    public string? Accion { get; set; }

    public int? Habilitado { get; set; }

    public int? Visible { get; set; }

    public int? Ordenmenu { get; set; }
}
