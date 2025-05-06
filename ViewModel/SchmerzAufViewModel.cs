using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;
using MangerTest.Klassen;

namespace MangerTest.ViewModel
{
    public class SchmerzAufViewModel : INotifyPropertyChanged
    {
        // Beispiel-Properties
        private DateTime _ausgewaehltesDatum = DateTime.Today;
        private TimeSpan _ausgewaehlteZeit = DateTime.Now.TimeOfDay;
        private int _schmerzintensitaet;
        public ICommand EintragCommand { get; }

        public ObservableCollection<string> FussBereiche { get; set; }
        public ObservableCollection<string> BeinBereiche { get; set; }
        public ObservableCollection<string> RueckenBereiche { get; set; }
        public ObservableCollection<string> Bauchbereich { get; set; }
        public ObservableCollection<string> Kniebereich { get; set; }
        public string AusgewaehltesKnie { get; set; }
        public string AusgewaehlterBauch { get; set; }
        public string AusgewaehlterRuecken { get; set; }
        public string AusgewaehltesBein { get; set; }
        public string AusgewaehlterFuss { get; set; }
        public string AusgewaehlteBereiche { get; set; }


        public ObservableCollection<string> Bereiche { get; set; }


      




        //deklaration 

        private readonly Dictionary<string, List<string>> _bereichsDaten = new()
        {
            ["Fuss"] = new List<string> { "ohne", "Spann", "linke Amputation", "Großer Zeh Amputation" },
            ["Bein"] = new List<string> { "ohne", "Oberschenkel", "Knie", "Unterschenkel" },
            ["Ruecken"] = new List<string> { "ohne", "Lendenwirbelsäule", "Brustwirbelsäule", "Halswirbelsäule" },
            ["Bauch"] = new List<string> { "ohne", "gerade", "schräg", "untere" },
            ["Knie"] = new List<string> { "ohne", "vorne", "seite", "hinten" },
            ["Bereiche"] = new List<string> { "ohne", "Fuss", "Bein", "Ruecken", "Bauch", "Knie" }
            // usw.
        };

        // Diese werden an die ComboBoxen gebunden
       

        public DateTime AusgewaehltesDatum
        {
            get => _ausgewaehltesDatum;
            set
            {
                _ausgewaehltesDatum = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan AusgewaehlteZeit
        {
            get => _ausgewaehlteZeit;
            set
            {
                _ausgewaehlteZeit = value;
                OnPropertyChanged();
            }
        }

        public int Schmerzintensitaet
        {
            get => _schmerzintensitaet;
            set
            {
                _schmerzintensitaet = value;
                OnPropertyChanged();
            }
        }

        // Beispiel-Command für später (Speichern etc.)
        public ICommand SpeichernCommand { get; }

        public SchmerzAufViewModel()
        {
            // Command kann hier initialisiert werden, z. B. mit RelayCommand oder DelegateCommand
            FussBereiche = new ObservableCollection<string>(_bereichsDaten["Fuss"]);
            AusgewaehlterFuss = FussBereiche.FirstOrDefault();
            BeinBereiche = new ObservableCollection<string>(_bereichsDaten["Bein"]);
            AusgewaehltesBein = BeinBereiche.FirstOrDefault();
            RueckenBereiche = new ObservableCollection<string>(_bereichsDaten["Ruecken"]);
            AusgewaehlterRuecken = RueckenBereiche.FirstOrDefault();
            Bauchbereich = new ObservableCollection<string>(_bereichsDaten["Bauch"]);
            AusgewaehlterBauch = Bauchbereich.FirstOrDefault();
            Kniebereich = new ObservableCollection<string>(_bereichsDaten["Knie"]);
            AusgewaehltesKnie = Kniebereich.FirstOrDefault();
            Bereiche = new ObservableCollection<string>(_bereichsDaten["Bereiche"]);
            AusgewaehlteBereiche = Bereiche.FirstOrDefault();
            LadeMedikamente();
            EintragCommand = new RelayCommand(Speichern);
            SchmerzenBeschreibung = new FlowDocument(new Paragraph(new Run("")));
        }
             
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        private ObservableCollection<string> _medikamentenListe;
        public ObservableCollection<string> MedikamentenListe
        {
            get => _medikamentenListe;
            set
            {
                _medikamentenListe = value;
                OnPropertyChanged(nameof(MedikamentenListe));
            }
        }

        private string _medikament;
        public string Medikament
        {
            get => _medikament;
            set
            {
                _medikament = value;
                OnPropertyChanged(nameof(Medikament));
            }
        }

        public void LadeMedikamente()
        {
            MedikamentenListe = new ObservableCollection<string>();

            string con = "data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string commandText = "SELECT Medikamentenname FROM Medikamente WHERE Einsatz = 'schmerz';";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string medikament = reader.GetString(0);
                        MedikamentenListe.Add(medikament);
                    }
                }
            }

            // Optional: ersten Eintrag direkt als Standard setzen
            if (MedikamentenListe.Count > 0)
                Medikament = MedikamentenListe[0];
        }

        private string _ausgewaehlterBereich;
        public string AusgewaehlterBereich
        {
            get => _ausgewaehlterBereich;
            set
            {
                _ausgewaehlterBereich = value;
                OnPropertyChanged(nameof(AusgewaehlterBereich));
                LadeBereich(value); // hier wird’s verwendet
                AktualisiereAktiveBereiche();
            }
        }

        private bool _istBeinAktiv;
        public bool IstBeinAktiv
        {
            get => _istBeinAktiv;
            set
            {
                if (_istBeinAktiv != value)
                {
                    _istBeinAktiv = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _istFussAktiv;
        public bool IstFussAktiv
        {
            get => _istFussAktiv;
            set
            {
                if (_istFussAktiv != value)
                {
                    _istFussAktiv = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _istRueckenAktiv;
        public bool IstRueckenAktiv
        {
            get => _istRueckenAktiv;
            set
            {
                if (_istRueckenAktiv != value)
                {
                    _istRueckenAktiv = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _istBauchAktiv;
        public bool IstBauchAktiv
        {
            get => _istBauchAktiv;
            set
            {
                if (_istBauchAktiv != value)
                {
                    _istBauchAktiv = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _istKnieAktiv;
        public bool IstKnieAktiv
        {
            get => _istKnieAktiv;
            set
            {
                if (_istKnieAktiv != value)
                {
                    _istKnieAktiv = value;
                    OnPropertyChanged();
                }
            }
        }

       




        private void AktualisiereAktiveBereiche()
        {
            IstBeinAktiv = AusgewaehlterBereich == "Bein";
            IstFussAktiv = AusgewaehlterBereich == "Fuss";
            IstRueckenAktiv = AusgewaehlterBereich == "Ruecken";
            IstBauchAktiv = AusgewaehlterBereich == "Bauch";
            IstKnieAktiv = AusgewaehlterBereich == "Knie";

            OnPropertyChanged(nameof(IstBeinAktiv));
            OnPropertyChanged(nameof(IstFussAktiv));
            OnPropertyChanged(nameof(IstRueckenAktiv));
            OnPropertyChanged(nameof(IstBauchAktiv));
            OnPropertyChanged(nameof(IstKnieAktiv));
        }

        private void LadeBereich(string bereich)
        {
            if (_bereichsDaten.ContainsKey(bereich))
            {
                var daten = _bereichsDaten[bereich];

                switch (bereich)
                {
                    case "Fuss":
                        FussBereiche = new ObservableCollection<string>(daten);
                        AusgewaehlterFuss = FussBereiche.FirstOrDefault();
                        OnPropertyChanged(nameof(FussBereiche));
                        break;
                    case "Bein":
                        BeinBereiche = new ObservableCollection<string>(daten);
                        AusgewaehltesBein = BeinBereiche.FirstOrDefault();
                        OnPropertyChanged(nameof(BeinBereiche));
                        break;
                    case "Ruecken":
                        RueckenBereiche = new ObservableCollection<string>(daten);
                        AusgewaehlterRuecken = RueckenBereiche.FirstOrDefault();
                        OnPropertyChanged(nameof(RueckenBereiche));
                        break;
                    case "Bauch":
                        Bauchbereich = new ObservableCollection<string>(daten);
                        AusgewaehlterBauch = Bauchbereich.FirstOrDefault();
                        OnPropertyChanged(nameof(Bauchbereich));
                        break;
                    case "Knie":
                        Kniebereich = new ObservableCollection<string>(daten);
                        AusgewaehltesKnie = Kniebereich.FirstOrDefault();
                        OnPropertyChanged(nameof(Kniebereich));
                        break;
                }
            }
        }

        //Alles für das Insert in die Datenbank
        private bool _mediEingenommen;
        public bool MediEingenommen
        {
            get => _mediEingenommen;
            set
            {
                _mediEingenommen = value;
                OnPropertyChanged();
            }
        }

        private string _medikamentA;
        public string MedikamentA
        {
            get => _medikamentA;
            set
            {
                _medikamentA = value;
                OnPropertyChanged(nameof(MedikamentA));
            }
        }

        private int _mediVorhanden;
        public int MediVorhanden
        {
            get => _mediVorhanden;
            set
            {
                _mediVorhanden = value;
                OnPropertyChanged(); // Informiert das ViewModel über die Änderung
            }
        }

        private void UpdateMediVorhanden()
        {
            // Wenn MediEingenommen wahr ist, setze mediVorhanden auf 2, andernfalls auf 1
            MediVorhanden = MediEingenommen ? 2 : 1;
        }


        private string _medikamentB;
        public string MedikamentB
        {
            get => _medikamentB;
            set
            {
                _medikamentB = value;
                OnPropertyChanged(nameof(MedikamentB));
            }
        }



        private int _medikamentAAnzahl;
        public int MedikamentAAnzahl
        {
            get => _medikamentAAnzahl;
            set
            {
                _medikamentAAnzahl = value;
                OnPropertyChanged();
            }
        }


        private int _medikamentBAnzahl;
        public int MedikamentBAnzahl
        {
            get => _medikamentBAnzahl;
            set
            {
                _medikamentBAnzahl = value;
                OnPropertyChanged();
            }
        }
        private FlowDocument _schmerzenBeschreibung;
        public FlowDocument SchmerzenBeschreibung
        {
            get => _schmerzenBeschreibung;
            set
            {
                _schmerzenBeschreibung = value;
                OnPropertyChanged();
            }
        }

       


        public string Lokalisierung
        {
            get
            {
                return AusgewaehlterBereich switch
                {
                    "Fuss" => AusgewaehlterFuss,
                    "Bein" => AusgewaehltesBein,
                    "Ruecken" => AusgewaehlterRuecken,
                    "Bauch" => AusgewaehlterBauch,
                    "Knie" => AusgewaehltesKnie,
                    _ => "ohne"
                };
            }
        }



        private void Speichern(object parameter)
        {
            try
            {
                int MediVorhanden = MediEingenommen ? 2 : 1;

                using (SqlConnection con = new SqlConnection("data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true"))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = @"
                    INSERT INTO schmerzen
                        (Datum, Uhrzeit, Medikamenta, Anzahla, Medikamentb, Anzahlb, Schmerzen, Bereich, Lokalisierung, Details, Medi)
                    VALUES
                        (@was, @start, @meta, @nra, @einheiten, @muskel, @kw, @differenz, @lokal, @detail, @medi)";

                        cmd.Parameters.AddWithValue("@was", AusgewaehltesDatum);
                        cmd.Parameters.AddWithValue("@start", AusgewaehlteZeit);
                       

                        cmd.Parameters.AddWithValue("@meta", string.IsNullOrWhiteSpace(MedikamentA) ? (object)DBNull.Value : MedikamentA);
                        cmd.Parameters.AddWithValue("@nra", MedikamentAAnzahl);
                        cmd.Parameters.AddWithValue("@einheiten", string.IsNullOrWhiteSpace(MedikamentB) ? (object)DBNull.Value : MedikamentB);
                        cmd.Parameters.AddWithValue("@muskel", MedikamentBAnzahl);
                        string text = new TextRange(SchmerzenBeschreibung.ContentStart, SchmerzenBeschreibung.ContentEnd).Text;
                        cmd.Parameters.AddWithValue("@kw", text);

                        cmd.Parameters.AddWithValue("@differenz", string.IsNullOrWhiteSpace(AusgewaehlterBereich) ? "ohne" : AusgewaehlterBereich);
                        cmd.Parameters.AddWithValue("@lokal", string.IsNullOrWhiteSpace(Lokalisierung) ? "ohne" : Lokalisierung);
                        cmd.Parameters.AddWithValue("@detail", "leer"); // kannst du später noch dynamisch machen
                         cmd.Parameters.AddWithValue("@medi", MediVorhanden);

                        cmd.ExecuteNonQuery();
                    }
                }

                System.Windows.MessageBox.Show("Eintrag erfolgreich gespeichert.");
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show($"Fehler beim Speichern: {ex.Message}");
            }
        }





    }
}

