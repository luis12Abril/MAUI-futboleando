using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Usuariotorneo
{
    public int Idusuariotorneo { get; set; }

    public int? Idusuario { get; set; }

    public int? Idtorneo { get; set; }

    public int? Habilitado { get; set; }
}
