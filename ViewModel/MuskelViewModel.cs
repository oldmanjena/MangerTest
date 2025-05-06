using MangerTest.Klassen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics;
using System.Windows.Data;

namespace MangerTest.ViewModel
{
    public class MuskelViewModel : INotifyPropertyChanged
    {
        public TimePickerViewModel TimePickerVM { get; set; }
        public ObservableCollection<MuskelEintrag> MuskelDaten { get; set; } = new ObservableCollection<MuskelEintrag>();
        // public RelayCommand EintragenCommand { get; set; } // Statt ICommand

        public ICommand EintragenCommand { get; set; }
        public ICommand AnzeigenAufwaermenCommand { get; set; }

        public MuskelViewModel MuskelVM { get; set; }



        // Properties für UI-Bindings
        public DateTime Wann { get; set; } = DateTime.Now;
        public string Zielmuskel { get; set; }
        public string Krank { get; set; }
      //  public int TrainNr { get; set; }

        

        // Weitere Properties für Eingabewerte
        private string _muskelgruppe;
        private string _uebung;
        private int _satz;
        private int _wiederholungen;
        private decimal _gewicht;
        private decimal _veraenderung;
        private string _art;
        private string _technik;

        public string Muskelgruppe
        {
            get => _muskelgruppe;
            set
            {
                if (_muskelgruppe != value)
                {
                    _muskelgruppe = value;
                    OnPropertyChanged();

                    // Ziele setzen und sicherstellen, dass sie korrekt gesetzt wird
                    if (ZielDict.TryGetValue(value, out var neueZiele))
                    {
                        Ziele = new ObservableCollection<string>(neueZiele);
                        Debug.WriteLine($"Neue Ziele gesetzt: {string.Join(", ", neueZiele)}");
                    }
                    else
                    {
                        Ziele = new ObservableCollection<string>();
                        Debug.WriteLine("Keine Ziele gefunden.");
                    }
                }
            }
        }



        private ObservableCollection<string> _gruppen;
        public ObservableCollection<string> Gruppen
        {
            get => _gruppen;
            set
            {
                if (_gruppen != value)
                {
                    Debug.WriteLine($"[Setter Gruppen] Setze neue Werte: {string.Join(", ", value ?? new ObservableCollection<string>())}");
                    _gruppen = value;
                    OnPropertyChanged();  // Benachrichtige UI über Änderung
                }
            }
        }



        public string Uebung
        {
            get => _uebung;
            set
            {
                if (_uebung != value)
                {
                    _uebung = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Satz
        {
            get => _satz;
            set
            {
                if (_satz != value)
                {
                    _satz = value;
                    Debug.WriteLine($"Satz wurde gesetzt: {_satz}");
                    OnPropertyChanged(); // ✅ jetzt korrekt, wenn Methode angepasst wurde
                }
            }
        }

        public int Wiederholungen
        {
            get => _wiederholungen;
            set
            {
                if (_wiederholungen != value)
                {
                    _wiederholungen = value;
                    Debug.WriteLine($"Wiederholungen wurde gesetzt: {_wiederholungen}");
                    OnPropertyChanged();
                }
            }
        }

        public decimal Gewicht
        {
            get => _gewicht;
            set
            {
                if (_gewicht != value)
                {
                    _gewicht = value;
                    Debug.WriteLine($"Gewicht wurde gesetzt: {_gewicht}");
                    OnPropertyChanged();
                }
            }
        }

        public decimal Veraenderung
        {
            get => _veraenderung;
            set
            {
                if (_veraenderung != value)
                {
                    _veraenderung = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Art
        {
            get => _art;
            set
            {
                if (_art != value)
                {
                    _art = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _trainNr;  // Private Variable zur Speicherung
        public int TrainNr  // Public Property
        {
            get => _trainNr;
            set
            {
                if (_trainNr != value)
                {
                    _trainNr = value;
                    OnPropertyChanged();  // Benachrichtige die UI über Änderungen
                }
            }
        }

        private ObservableCollection<string> _trainingsnummern;
        public ObservableCollection<string> Trainingsnummern
        {
            get => _trainingsnummern;
            set
            {
                _trainingsnummern = value;
                OnPropertyChanged();
            }
        }

        public string Technik
        {
            get => _technik;
            set
            {
                if (_technik != value)
                {
                    _technik = value;
                    OnPropertyChanged();
                }
            }
        }

        // Dictionary mit Gruppen und Zielmuskeln
        public Dictionary<string, List<string>> ZielDict { get; set; } = new()
        {
            { "Krank", new List<string> { "krank" } },
            { "Rücken", new List<string> { "Latissimus", "Trapezius", "unterer Rücken" } },
            { "Beine", new List<string> { "Quadrizeps", "Harnstrings" } },
            { "Brust", new List<string> { "Obere Brust", "Mittlere Brust" } },
            { "Bizeps", new List<string> { "gesamter Bizeps" } },
            { "Trizeps", new List<string> { "gesamter Trizeps" } },
            { "Schulter", new List<string> { "vordere", "mittlere", "hintere" } },
            { "Bauch", new List<string> { "oberer", "gesamter", "unterer", "schräger" } },
            { "Unterarm", new List<string> { "gesamter" } },
            { "Ganzkörper", new List<string> { "gesamter" } }
        };

        // Nur die Keys als Liste (für ComboBox)
       




        // Konstruktor
        public MuskelViewModel()
        {
            Debug.WriteLine("MuskelViewModel wurde instanziert");
            //MuskelDaten = new ObservableCollection<MuskelEintrag>();
            // TrainingsDaten = MuskelDaten; // Nur für Klarheit (nicht notwendig, aber sauber)
            Gruppen = new ObservableCollection<string>(ZielDict.Keys);
          //  OnPropertyChanged(nameof(Gruppen));
            Debug.WriteLine("Gruppen wurden gesetzt: " + string.Join(", ", Gruppen));
            // Commands für das Abrufen oder senden von Daten
            EintragenCommand = new RelayCommand(EintragHinzufuegen);
            ZeigeAndereDatenCommand = new RelayCommand(AusfuehrenZeigeAndereDaten);
            AnzeigenAufwaermenCommand = new RelayCommand(AusfuehrenAnzeigenAufwaermen);


            TimePickerVM = new TimePickerViewModel();
            Art = Arten[0];
            Technik = Techniken[0];
            Muskelgruppe = "Bauch";
            SelectedDate = DateTime.Now;
            

            // LetzteTrainingData = CollectionViewSource.GetDefaultView(MuskelDaten);
            // Beispiel: letzten 5 Einträge nach Datum sortiert
            //LetzteTrainingData.SortDescriptions.Add(new SortDescription("Wann", ListSortDirection.Descending));
        }

        // Checkt, ob ein Eintrag gespeichert werden kann
        private bool KannEintragen(object obj)
        {
            Debug.WriteLine("KannEintragen wurde aufgerufen");
            return true;
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    // Hier könntest du weitere Logik ausführen, wenn sich das Datum ändert
                }
            }
        }

        // Eintrag hinzufügen
        private void EintragHinzufuegen(object obj)
        {
            Debug.WriteLine("EintragHinzufuegen wurde aufgerufen");
            try
            {
                MessageBox.Show("Eintrag wird ausgeführt.", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);

                string connectionString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;
                using SqlConnection con = new(connectionString);
                con.Open();

                using SqlCommand cmd = new("INSERT INTO Muskel(wann, Muskelgruppe, Uebung, Zielmuskel, Satz, Wh, gewicht, veraenderung, art, krank, Technik, Trainnr) " +
                                          "VALUES (@wann, @Muskelgruppe, @Uebung, @Zielmuskel, @Satz, @Wh, @gewicht, @veraenderung, @art, @krank, @Technik, @Trainnr)", con);
                cmd.Parameters.AddWithValue("@wann", Wann);
                cmd.Parameters.AddWithValue("@Muskelgruppe", Muskelgruppe);
                cmd.Parameters.AddWithValue("@Uebung", Uebung);
                cmd.Parameters.AddWithValue("@Zielmuskel", Zielmuskel);
                cmd.Parameters.AddWithValue("@Satz", Satz);
                cmd.Parameters.AddWithValue("@Wh", Wiederholungen);
                cmd.Parameters.AddWithValue("@gewicht", Gewicht);
                cmd.Parameters.AddWithValue("@veraenderung", Veraenderung);
                cmd.Parameters.AddWithValue("@art", Art);
                cmd.Parameters.AddWithValue("@krank", IstKrank ? 1 : 0);
                cmd.Parameters.AddWithValue("@Technik", Technik);
                cmd.Parameters.AddWithValue("@Trainnr", TrainNr);

                cmd.ExecuteNonQuery();

                MuskelDaten.Add(new MuskelEintrag
                {
                    Wann = Wann,
                    Muskelgruppe = Muskelgruppe,
                    Uebung = Uebung,
                    Zielmuskel = Zielmuskel,
                    Satz = Satz,
                    Wiederholungen = Wiederholungen,
                    Gewicht = Gewicht,
                    Veraenderung = Veraenderung,
                    Art = Art,
                    Technik = Technik,
                    Krank = Krank,
                    TrainNr = TrainNr
                });

                MessageBox.Show("Muskeleintrag erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Datenbankfehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Beispiel für RelayCommand
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

            public void Execute(object parameter) => _execute(parameter);

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
        }

        // In deinem MuskelViewModel

        private bool _istKrank;
        public bool IstKrank
        {
            get => _istKrank;
            set
            {
                if (_istKrank != value)
                {
                    _istKrank = value;
                    OnPropertyChanged();

                    // Gegenseite automatisch deaktivieren
                    if (value)
                        IstNichtKrank = false;
                }
            }
        }

        private bool _istNichtKrank;
        public bool IstNichtKrank
        {
            get => _istNichtKrank;
            set
            {
                if (_istNichtKrank != value)
                {
                    _istNichtKrank = value;
                    OnPropertyChanged();

                    // Gegenseite automatisch deaktivieren
                    if (value)
                        IstKrank = false;
                }
            }
        }




        // Die Bindings für die ComboBoxen
        public List<string> Arten { get; } = new() { "Definition", "Masseaufbau", "Diät", "Normal" };
        public List<string> Techniken { get; } = new() { "Aufwärmen", "Arbeitssatz", "Cool Down" };



        public ObservableCollection<MuskelEintrag> MuskelDaten2 { get; set; } = new();

        // Neue Property für letzte Trainingsdaten (z.B. Filter oder View)
        private ICollectionView _letzteTrainingData;
        public ICollectionView LetzteTrainingData
        {
            get => _letzteTrainingData;
            set
            {
                _letzteTrainingData = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<string> _ziele = new ObservableCollection<string>();

        public ObservableCollection<string> Ziele
        {
            get => _ziele;
            set
            {
                _ziele = value;
                OnPropertyChanged();
                Debug.WriteLine($"[VM] Neue Ziele gesetzt: {string.Join(", ", _ziele)}");
            }
        }

        private bool _istDtgUeberSichtbar = true; // Standardmäßig das erste DataGrid sichtbar
        public bool IstDtgUeberSichtbar
        {
            get { return _istDtgUeberSichtbar; }
            set
            {
                if (_istDtgUeberSichtbar != value)
                {
                    _istDtgUeberSichtbar = value;
                    OnPropertyChanged(nameof(IstDtgUeberSichtbar));
                }
            }
        }

        private bool _istDtgAndereDatenSichtbar = false; // Standardmäßig das zweite DataGrid ausgeblendet
        public bool IstDtgAndereDatenSichtbar
        {
            get { return _istDtgAndereDatenSichtbar; }
            set
            {
                if (_istDtgAndereDatenSichtbar != value)
                {
                    _istDtgAndereDatenSichtbar = value;
                    OnPropertyChanged(nameof(IstDtgAndereDatenSichtbar));
                }
            }
        }

        public ICommand ZeigeAndereDatenCommand { get; }



        private void AusfuehrenZeigeAndereDaten(object obj)
        {
            IstDtgUeberSichtbar = false;
            IstDtgAndereDatenSichtbar = true;
            // Hier könntest du auch Daten für das zweite DataGrid laden, falls nötig.
        }


        private ObservableCollection<AufwaermSatz> _aufwaermDaten = new ObservableCollection<AufwaermSatz>();
        public ObservableCollection<AufwaermSatz> AufwaermDaten
        {
            get => _aufwaermDaten;
            set
            {
                _aufwaermDaten = value;
                OnPropertyChanged(nameof(AufwaermDaten));
            }
        }

        private ICollectionView _aufwaermDatenView;
        public ICollectionView AufwaermDatenView
        {
            get => _aufwaermDatenView;
            set
            {
                _aufwaermDatenView = value;
                OnPropertyChanged(nameof(AufwaermDatenView));
            }
        }

        private void AusfuehrenAnzeigenAufwaermen(object obj)
        {
            string selectedGruppe = Muskelgruppe; // Verwende die gebundene Property
            string selectedUebung = Uebung;       // Verwende die gebundene Property
            string selectedArt = Art;             // Verwende die gebundene Property

            string conString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;
            string cmdString;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();

                    cmdString = "SELECT Uebung, MaxGewicht, " +
                                "MaxGewicht * 0.4 AS Satz1, " +
                                "MaxGewicht * 0.5 AS Satz2, " +
                                "MaxGewicht * 0.6 AS Satz3 " +
                                "FROM (SELECT Uebung, MAX(training) AS MaxGewicht " +
                                "FROM Max " +
                                "WHERE Muskel = @muskelgruppe " +
                                "AND Uebung LIKE '%' + @uebung + '%' " +
                                "GROUP BY Uebung) AS MaxGewichtSubquery " +
                                "ORDER BY Uebung DESC;";

                    SqlCommand cmd = new SqlCommand(cmdString, con);
                    cmd.Parameters.AddWithValue("@muskelgruppe", selectedGruppe);
                    cmd.Parameters.AddWithValue("@uebung", selectedUebung);
                    cmd.Parameters.AddWithValue("@art", selectedArt);

                    SqlDataReader reader = cmd.ExecuteReader();
                    AufwaermDaten.Clear(); // Alte Daten entfernen

                    while (reader.Read())
                    {
                        AufwaermDaten.Add(new AufwaermSatz
                        {
                            Uebung = reader["Uebung"].ToString(),
                            MaxGewicht = Convert.ToDecimal(reader["MaxGewicht"]),
                            Satz1 = Convert.ToDecimal(reader["Satz1"]),
                            Satz2 = Convert.ToDecimal(reader["Satz2"]),
                            Satz3 = Convert.ToDecimal(reader["Satz3"])
                        });
                    }

                    AufwaermDatenView = CollectionViewSource.GetDefaultView(AufwaermDaten);
                    IstDtgUeberSichtbar = false;     // Blende das erste DataGrid aus
                    IstDtgAndereDatenSichtbar = true; // Zeige das zweite DataGrid (für Aufwärmen)
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Datenbankfehler beim Abrufen der Aufwärmdaten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class AufwaermSatz // Hilfsklasse für die Daten
        {
            public string Uebung { get; set; }
            public decimal MaxGewicht { get; set; }
            public decimal Satz1 { get; set; }
            public decimal Satz2 { get; set; }
            public decimal Satz3 { get; set; }
        }



    }



}

