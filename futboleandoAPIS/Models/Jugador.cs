using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Jugador
{
    public int Idjugador { get; set; }

    public string? Nombre { get; set; }

    public string? Appaterno { get; set; }

    public string? Apmaterno { get; set; }

    public DateOnly? Fnacimiento { get; set; }

    public int? Idequipo { get; set; }

    public int? Goles { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Idtorneo { get; set; }
}
