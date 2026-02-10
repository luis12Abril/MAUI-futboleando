using System.ComponentModel;
using futboleandoEntities.Equipo;

namespace futboleando.Models
{
    public class EquipoIndexed : INotifyPropertyChanged
    {
        public int Index { get; set; }
        public EquipoListCLS Equipo { get; set; }

        private ImageSource _fotoSource;
        public ImageSource FotoSource
        {
            get => _fotoSource;
            set
            {
                if (_fotoSource == value)
                {
                    return;
                }

                _fotoSource = value;
                OnPropertyChanged(nameof(FotoSource));
            }
        }

        private bool _tieneFoto;
        public bool TieneFoto
        {
            get => _tieneFoto;
            set
            {
                if (_tieneFoto == value)
                {
                    return;
                }

                _tieneFoto = value;
                OnPropertyChanged(nameof(TieneFoto));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
