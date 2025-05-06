using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MangerTest.ViewModel
{
    public class MuskelCombinedViewModel : INotifyPropertyChanged
    {
        private MuskelViewModel _muskelVM;
        public MuskelViewModel MuskelVM
        {
            get => _muskelVM;
            set
            {
                if (_muskelVM != value)
                {
                    _muskelVM = value;
                    OnPropertyChanged(); // Benachrichtigt die UI bei einer Änderung
                }
            }
        }

        private TimePickerViewModel _timePickerVM;
        public TimePickerViewModel TimePickerVM
        {
            get => _timePickerVM;
            set
            {
                if (_timePickerVM != value)
                {
                    _timePickerVM = value;
                    OnPropertyChanged(); // Benachrichtigt die UI bei einer Änderung
                }
            }
        }

        // Konstruktor
        public MuskelCombinedViewModel()
        {
            MuskelVM = new MuskelViewModel();
            TimePickerVM = new TimePickerViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
