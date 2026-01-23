namespace futboleandoEntities.Torneo
{
    public class TorneoListCLS
    {
        public int idtorneo { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string clavetorneo { get; set; } = string.Empty;
        public int idliga { get; set; }
        public string nombreliga { get; set; } = string.Empty;
        public int visible { get; set; }
    }
}
