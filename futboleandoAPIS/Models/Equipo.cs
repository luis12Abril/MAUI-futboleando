using System;
using System.Collections.Generic;

namespace futboleandoAPIS.Models;

public partial class Equipo
{
    public int Idequipo { get; set; }

    public string? Nombre { get; set; }

    public string? Representante { get; set; }

    public string? Fotoequipo { get; set; }

    public int? Jugados { get; set; }

    public int? Ganados { get; set; }

    public int? Empatados { get; set; }

    public int? Perdidos { get; set; }

    public int? Empatadosganados { get; set; }

    public int? Empatadosperdidos { get; set; }

    public int? Ganadosadmo { get; set; }

    public int? Perdidosadmo { get; set; }

    public int? Golesafavor { get; set; }

    public int? Golesencontra { get; set; }

    public int? Difgoles { get; set; }

    public int? Puntos { get; set; }

    public int? Habilitado { get; set; }

    public string? Torneo { get; set; }

    public int? Idtorneo { get; set; }

    public int? Puntosextras { get; set; }

    public string? Usuequipo { get; set; }

    public string? Claequipo { get; set; }

    public DateTime? Vigencia { get; set; }
}
