using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleandoEntities.Equipo
{
    public class EquipoListCLS
    {
        public int idequipo { get; set; }
        public string nombre { get; set; }
        public string representante { get; set; }
        public string foto { get; set; }  // ✅ Cambiado de byte[] a string
        public int? golesfavor { get; set; }
        public int? golescontra { get; set; }
        public int? puntos { get; set; }
        public int? puntosextras { get; set; }
        public int? diferenciagoles { get; set; }
        
        // Campos adicionales para tabla de posiciones
        public int? jugados { get; set; }
        public int? ganados { get; set; }
        public int? perdidos { get; set; }
        public int? empatados { get; set; }
        public int? empatadosganados { get; set; }
        public int? golesafavor { get; set; }
        public int? golesencontra { get; set; }
        public int? difgoles { get; set; }
    }
}
