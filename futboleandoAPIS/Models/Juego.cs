using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Juego
{
    public int Idjuego { get; set; }

    public int? Idjornada { get; set; }

    public int? Idequipo01 { get; set; }

    public int? Idequipo02 { get; set; }

    public int? Idcampo { get; set; }

    public int? Idarbitro { get; set; }

    public DateTime? Fhorario { get; set; }

    public int? Golesequipo01 { get; set; }

    public int? Golesequipo02 { get; set; }

    public int? Idestatusjuego { get; set; }

    public string? Resequipo01 { get; set; }

    public string? Resequipo02 { get; set; }

    public int? Puntosequipo01 { get; set; }

    public int? Puntosequipo02 { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Cuentaparapuntos { get; set; }

    public int? Cuentaparagoles { get; set; }

    public int? Idtorneo { get; set; }

    public int? Peequipo01 { get; set; }

    public int? Peequipo02 { get; set; }
}
