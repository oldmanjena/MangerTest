using MangerTest.Klassen;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MangerTest.ViewModel
{
    public class KoerperdatenViewModel : INotifyPropertyChanged
    {
        private Brush _ampelRotPrivate = Brushes.Gray;  // private Feldvariable mit anderem Namen
        private Brush _ampelGelbPrivate = Brushes.Gray;
        private Brush _ampelGruenPrivate = Brushes.Gray;
        public ICommand SpeichernCommand { get; }
        private KoerperdatenModel _daten = new KoerperdatenModel();
        public KoerperdatenModel Daten
        {
            get => _daten;
            set
            {
                _daten = value;
                OnPropertyChanged(nameof(Daten));
            }
        }
        //Funktion zum Speichern der Daten in die Datenbank
        public KoerperdatenViewModel()
        {
            AmpelRot = Brushes.Red;
            AmpelGelb = Brushes.Yellow;
            AmpelGruen = Brushes.Green;

            SpeichernCommand = new RelayCommand(
                obj => Speichern(),
                obj => KannGespeichertWerden()
                  );

        }

        private bool KannGespeichertWerden()
        {
            return Gewicht != 0 &&
                   BMI != 0 &&
                   Koerperfett != 0 &&
                   Fettmasse != 0 &&
                   FettfreiesKoerpergewicht != 0 &&
                   Muskelmasse != 0 &&
                   Muskelfrequenz != 0 &&
                   Skelettmuskulatur != 0 &&
                   Knochenmasse != 0 &&
                   Eiweissmenge != 0 &&
                   Proteine != 0 &&
                   Wassergehalt != 0 &&
                   Koerperwasser != 0 &&
                   Unterhautfettgewebe != 0 &&
                   ViszeralesFett != 0 &&
                   BMR != 0;
        }

        private void Speichern()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;

            string sql = @"
        INSERT INTO Fitdays (
            Datum, Gewicht, BMI, Körperfett, Fettmasse, Fettfrei,
            Muskelmasse, Muskelrate, Skelettmuskulatur, Knochenmasse,
            Eiweismenge, Protein, Wasseergehalt, Körperwasser,
            Unterhautfett, Vizeralfett, BMR)
        VALUES (
            @Datum, @Gewicht, @BMI, @Körperfett, @Fettmasse, @Fettfrei,
            @Muskelmasse, @Muskelrate, @Skelettmuskulatur, @Knochenmasse,
            @Eiweismenge, @Protein, @Wasseergehalt, @Körperwasser,
            @Unterhautfett, @Vizeralfett, @BMR)";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Datum", DateTime.Now);
                cmd.Parameters.AddWithValue("@Gewicht", Gewicht);
                cmd.Parameters.AddWithValue("@BMI", BMI);
                cmd.Parameters.AddWithValue("@Körperfett", Koerperfett);
                cmd.Parameters.AddWithValue("@Fettmasse", Fettmasse);
                cmd.Parameters.AddWithValue("@Fettfrei", FettfreiesKoerpergewicht);
                cmd.Parameters.AddWithValue("@Muskelmasse", Muskelmasse);
                cmd.Parameters.AddWithValue("@Muskelrate", Muskelfrequenz);
                cmd.Parameters.AddWithValue("@Skelettmuskulatur", Skelettmuskulatur);
                cmd.Parameters.AddWithValue("@Knochenmasse", Knochenmasse);
                cmd.Parameters.AddWithValue("@Eiweismenge", Eiweissmenge);
                cmd.Parameters.AddWithValue("@Protein", Proteine);
                cmd.Parameters.AddWithValue("@Wasseergehalt", Wassergehalt);
                cmd.Parameters.AddWithValue("@Körperwasser", Koerperwasser);
                cmd.Parameters.AddWithValue("@Unterhautfett", Unterhautfettgewebe);
                cmd.Parameters.AddWithValue("@Vizeralfett", ViszeralesFett);
                cmd.Parameters.AddWithValue("@BMR", BMR);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Daten wurden erfolgreich gespeichert.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Speichern: " + ex.Message);
                }
            }
        }


        // Weiterleitung der Properties für Binding
        public decimal? Gewicht
        {
            get => Daten.Gewicht;
            set
            {
                Daten.Gewicht = value;
                OnPropertyChanged(nameof(Gewicht));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? BMI
        {
            get => Daten.BMI;
            set
            {
                Daten.BMI = value;
                OnPropertyChanged(nameof(BMI));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Koerperfett
        {
            get => Daten.Koerperfett;
            set
            {
                Daten.Koerperfett = value;
                OnPropertyChanged(nameof(Koerperfett));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Fettmasse
        {
            get => Daten.Fettmasse;
            set
            {
                Daten.Fettmasse = value;
                OnPropertyChanged(nameof(Fettmasse));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? FettfreiesKoerpergewicht
        {
            get => Daten.FettfreiesKoerpergewicht;
            set
            {
                Daten.FettfreiesKoerpergewicht = value;
                OnPropertyChanged(nameof(FettfreiesKoerpergewicht));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Muskelmasse
        {
            get => Daten.Muskelmasse;
            set
            {
                Daten.Muskelmasse = value;
                OnPropertyChanged(nameof(Muskelmasse));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Muskelfrequenz
        {
            get => Daten.Muskelfrequenz;
            set
            {
                Daten.Muskelfrequenz = value;
                OnPropertyChanged(nameof(Muskelfrequenz));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Skelettmuskulatur
        {
            get => Daten.Skelettmuskulatur;
            set
            {
                Daten.Skelettmuskulatur = value;
                OnPropertyChanged(nameof(Skelettmuskulatur));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Knochenmasse
        {
            get => Daten.Knochenmasse;
            set
            {
                Daten.Knochenmasse = value;
                OnPropertyChanged(nameof(Knochenmasse));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Eiweissmenge
        {
            get => Daten.Eiweissmenge;
            set
            {
                Daten.Eiweissmenge = value;
                OnPropertyChanged(nameof(Eiweissmenge));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Proteine
        {
            get => Daten.Proteine;
            set
            {
                Daten.Proteine = value;
                OnPropertyChanged(nameof(Proteine));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Wassergehalt
        {
            get => Daten.Wassergehalt;
            set
            {
                Daten.Wassergehalt = value;
                OnPropertyChanged(nameof(Wassergehalt));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Koerperwasser
        {
            get => Daten.Koerperwasser;
            set
            {
                Daten.Koerperwasser = value;
                OnPropertyChanged(nameof(Koerperwasser));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal? Unterhautfettgewebe
        {
            get => Daten.Unterhautfettgewebe;
            set
            {
                Daten.Unterhautfettgewebe = value;
                OnPropertyChanged(nameof(Unterhautfettgewebe));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private decimal? _viszeralesFett;
        public decimal? ViszeralesFett
        {
            get => _viszeralesFett;
            set
            {
                _viszeralesFett = value;
                OnPropertyChanged(nameof(ViszeralesFett));
                if (value.HasValue)
                    AktualisiereAmpel(value.Value);
                else
                    SetzeAmpelGrau(); // z. B. alle Farben auf Grau
            }
        }

        public decimal? BMR
        {
            get => Daten.BMR;
            set
            {
                Daten.BMR = value;
                OnPropertyChanged(nameof(BMR));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        //Implementierung der Ampel
        public Brush AmpelRot
        {
            get => _ampelRotPrivate;  // nutzt die private Variable
            set
            {
                _ampelRotPrivate = value;
                OnPropertyChanged(nameof(AmpelRot));
            }
        }

        public Brush AmpelGelb
        {
            get => _ampelGelbPrivate;  // nutzt die private Variable
            set
            {
                _ampelGelbPrivate = value;
                OnPropertyChanged(nameof(AmpelGelb));
            }
        }

        public Brush AmpelGruen
        {
            get => _ampelGruenPrivate;  // nutzt die private Variable
            set
            {
                _ampelGruenPrivate = value;
                OnPropertyChanged(nameof(AmpelGruen));
            }
        }

        public void AktualisiereAmpel(decimal vizeralfett)
        {
            AmpelRot = Brushes.Gray;
            AmpelGelb = Brushes.Gray;
            AmpelGruen = Brushes.Gray;

            if (vizeralfett < 14)
                AmpelGruen = Brushes.Green;
            else if (vizeralfett < 16)
                AmpelGelb = Brushes.Yellow;
            else
                AmpelRot = Brushes.Red;
        }

        private void AktualisiereAmpelAusText(string text)
        {
            if (decimal.TryParse(text, out decimal wert))
            {
                AktualisiereAmpel(wert); // z. B. grün/gelb/rot je nach Wert
            }
            else
            {
                // Ungültiger Wert: alles auf Grau setzen
                AmpelRot = Brushes.Gray;
                AmpelGelb = Brushes.Gray;
                AmpelGruen = Brushes.Gray;
            }
        }

        private void SetzeAmpelGrau()
        {
            AmpelRot = Brushes.Gray;
            AmpelGelb = Brushes.Gray;
            AmpelGruen = Brushes.Gray;
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
