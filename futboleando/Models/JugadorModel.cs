using futboleandoEntities.Jugador;
using futboleando.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace futboleando.Models
{
    public class JugadorModel : BaseBinding
    {

        private JugadorFormCLS _oJugadorFormCLS;

        public event Func<Task> OnChange;
        public JugadorFormCLS oJugadorFormCLS
        {
            get
            {
                return _oJugadorFormCLS;
            }
            set
            {
                SetValue(ref _oJugadorFormCLS, value);
            }

        }
    }
}
