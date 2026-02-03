namespace futboleandoEntities.Visitas
{
    public class VisitasTorneoTotalesCLS
    {
        public int totalVisitasWeb { get; set; }
        public int totalVisitasApp { get; set; }
    }

    public class VisitasTorneoCLS
    {
        public int idtorneo { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int visible { get; set; }
        public int visitas { get; set; }
        public int visitascel { get; set; }
        public int totalVisitas { get; set; }
        public string visibleTexto { get; set; } = "No";
    }
}
