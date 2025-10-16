using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string? Nombre { get; set; }

    public string? Contraseña { get; set; }

    public int? Idpersona { get; set; }

    public int? Idtipousuario { get; set; }

    public string? Token { get; set; }

    public int? Habilitado { get; set; }

    public int? Visitas { get; set; }

    public int? Visitascel { get; set; }

    public int? Idarbitrocolegio { get; set; }

    public DateTime? Fechaalta { get; set; }

    public string? Origenalta { get; set; }
}
