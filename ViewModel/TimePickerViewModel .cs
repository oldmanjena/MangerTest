using MangerTest.Klassen;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace MangerTest.ViewModel
{
    public class TimePickerViewModel : INotifyPropertyChanged
    {
        // START- & ENDZEIT
       

        private TimeSpan? _endzeit;
        public TimeSpan? Endzeit
        {
            get => _endzeit;
            set
            {
                _endzeit = value;
                OnPropertyChanged(nameof(Endzeit));
            }
        }

        // SELECTED DATE
        public TrainingsViewModel TrainingsVM { get; set; }  // Referenz vom Parent-VM

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));

                var date = value ?? DateTime.Now;

                BerechneKalenderwoche(date);
                Wochentag = date.ToString("dddd", new CultureInfo("de-DE"));

                // ← Wichtig: Datum auch im TrainingsViewModel setzen!
                TrainingsVM?.SetDatumExtern(date); // Methode im TrainingsVM
            }
        }

        private int _kw;
        public int KW
        {
            get => _kw;
            set
            {
                _kw = value;
                OnPropertyChanged(nameof(KW));
            }
        }

        // NEUER EINTRAG
        private TrainingsEintrag _neuerEintrag = new TrainingsEintrag();
        public TrainingsEintrag NeuerEintrag
        {
            get => _neuerEintrag;
            set
            {
                _neuerEintrag = value;
                OnPropertyChanged(nameof(NeuerEintrag));
            }
        }

        // TRAININGSDATEN LISTE
        public ObservableCollection<TrainingsEintrag> TrainingsDaten { get; set; } = new ObservableCollection<TrainingsEintrag>();

        // COMMAND
        public ICommand EintragenCommand { get; set; }

        public TimePickerViewModel()
        {
           // Startzeit = DateTime.Now.TimeOfDay;
            SelectedDate = DateTime.Now; // Triggert automatisch die KW-Berechnung

            // Beispiel: Einfacher Command
            EintragenCommand = new RelayCommand(_ =>
            {
                TrainingsDaten.Add(new TrainingsEintrag
                {
                    Datum = SelectedDate ?? DateTime.Now,
                    KW = KW,
                    Wochentag = NeuerEintrag.Wochentag,
                   // Kommentar = NeuerEintrag.Kommentar,
                });
            });
        }

        private void BerechneKalenderwoche(DateTime datum)
        {
            var ci = CultureInfo.CurrentCulture;
            var cal = ci.Calendar;
            var rule = CalendarWeekRule.FirstFourDayWeek;
            var firstDay = DayOfWeek.Monday;

            int kw = cal.GetWeekOfYear(datum, rule, firstDay);
            KW = kw;
            NeuerEintrag.KW = kw;
        }

        //Wochentag

        private string _wochentag;
        public string Wochentag
        {
            get => _wochentag;
            set
            {
                _wochentag = value;
                OnPropertyChanged(nameof(Wochentag));
            }
        }

        private DateTime _datum;
        public DateTime Datum
        {
            get => _datum;
            set
            {
                _datum = value;
                Wochentag = _datum.ToString("dddd");  // Setze den Wochentag
                OnPropertyChanged(nameof(Datum));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name) =>
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
