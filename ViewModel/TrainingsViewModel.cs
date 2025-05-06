using System;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using ManagerTest;
using MangerTest.Klassen;

namespace MangerTest.ViewModel
{
    public class TrainingsViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingsEintrag> TrainingsDaten { get; set; }

        public ICommand EintragenCommand { get; }

        

        public TrainingsViewModel()
        {
            TrainingsDaten = new ObservableCollection<TrainingsEintrag>();
            EintragenCommand = new RelayCommand(EintragHinzufuegen);
        }

        // Properties gebunden an UI
        public DateTime Datum { get; set; } = DateTime.Now;
        public int KW { get; set; }
        public DateTime Wochentag { get; set; } = DateTime.Now; // In DB als datetime? -> prüfen!
        public TimeSpan? Start { get; set; } 
        public TimeSpan? Dauer { get; set; }

        public int RPM { get; set; }
      //  public double Entfernung { get; set; }
       // public decimal Kcal { get; set; }  // Anpassung
        public int Puls { get; set; }
        public double Aerober { get; set; }
        public double Anaerober { get; set; } // Anearober = Anaerober
     //   public decimal? Regeneration { get; set; }
        public int VO2max { get; set; }
      //  public int PlanNr { get; set; }  // statt int
      //  public string Kommentar { get; set; } = "";

        public int Per_id { get; set; }
        public double Puls_Max { get; set; }

        private void EintragHinzufuegen(object obj)
        {
            try
            {
                //var dauer = (Ende - Start).TotalMinutes;

                string connectionString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Training(Datum, KW, Wochentag, Start, Dauer, RPM, Entfernung, kcal, Puls, Aerober, Anearober, Regenaration, VO2max, Plan_Nr, Kommentar, Per_id) " +
                        "VALUES (@Datum, @KW, @Wochentag, @Start, @Dauer, @RPM, @Entfernung, @kcal, @Puls, @Aerober, @Anearober, @Rege, @VO2max, @Plan_Nr, @Kommentar, @perid)", con))
                    {
                        cmd.Parameters.AddWithValue("@Datum", Datum);
                        cmd.Parameters.AddWithValue("@KW", KW);
                        cmd.Parameters.AddWithValue("@Wochentag", Wochentag);
                        cmd.Parameters.Add("@Start", SqlDbType.Time).Value = Start.HasValue ? (object)Start.Value : DBNull.Value;                        
                        cmd.Parameters.Add("@Dauer", SqlDbType.Time).Value = Dauer.HasValue ? (object)Dauer.Value : DBNull.Value;
                        cmd.Parameters.AddWithValue("@RPM", RPM);
                        cmd.Parameters.AddWithValue("@Entfernung", Entfernung);
                        cmd.Parameters.AddWithValue("@kcal", (object)Kcal ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Puls", Puls);
                        cmd.Parameters.AddWithValue("@Aerober", Aerober);
                        cmd.Parameters.AddWithValue("@Anearober", Anaerober);
                        cmd.Parameters.AddWithValue("@Rege", (object)Regeneration ?? DBNull.Value); // nullable decimal handling
                        cmd.Parameters.AddWithValue("@VO2max", VO2max);
                        cmd.Parameters.AddWithValue("@Plan_Nr", PlanNr);
                        cmd.Parameters.AddWithValue("@Kommentar", Kommentar);
                        cmd.Parameters.AddWithValue("@perid", "1"); // Fester Platzhalterwert

                        foreach (SqlParameter param in cmd.Parameters)
                        {
                            Debug.WriteLine($"{param.ParameterName} = {param.Value} ({param.Value?.GetType()})");
                        }
                        Debug.WriteLine($"Dauer intern: {Dauer?.ToString() ?? "NULL"}");
                        cmd.ExecuteNonQuery();
                    }
                }

                TrainingsDaten.Add(new TrainingsEintrag
                {
                    Datum = Datum,
                    KW = KW,
                    Wochentag = Wochentag,
                    Start = Start,
                    Dauer = Dauer,
                    RPM = RPM,
                    Entfernung = (double)Entfernung,
                    Kcal = (decimal)Kcal,
                    Puls = Puls,
                    Aerober = Aerober,
                    Anaerober = Anaerober,
                    Regeneration = (decimal)Regeneration,
                    VO2max = VO2max,
                    PlanNr = PlanNr,
                    Kommentar = Kommentar
                });

                MessageBox.Show("Eintrag erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Datenbankfehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SetDatumExtern(DateTime datum)
        {
            Datum = datum;
            KW = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
           // Wochentag = datum.ToString("dddd", new CultureInfo("de-DE"));
        }

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

        private string _dauerString;
        public string DauerString
        {
            get => _dauerString;
            set
            {
                _dauerString = value;
                OnPropertyChanged(nameof(DauerString));

                if (TimeSpan.TryParseExact(value, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan result))
                {
                    Dauer = result;
                    Debug.WriteLine($"Dauer gesetzt (TryParseExact): {Dauer?.ToString() ?? "NULL"}"); // Füge diese Debug-Ausgabe hinzu
                }
                else
                {
                    Dauer = null;
                    Debug.WriteLine($"Dauer auf NULL gesetzt (TryParseExact fehlgeschlagen): {value}"); // Füge diese Debug-Ausgabe hinzu
                }

                Debug.WriteLine($"Dauer gesetzt (Property): {Dauer?.ToString() ?? "NULL"}"); // Füge diese Debug-Ausgabe hinzu
            }
        }


        private string _startstring;
        public string Startstring
        {
            get => _startstring;
            set
            {
                _startstring = value;
                OnPropertyChanged(nameof(Startstring));

                if (TimeSpan.TryParseExact(value, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan result))
                {
                    Start = result;
                    Debug.WriteLine($"Start gesetzt (TryParseExact): {Start?.ToString() ?? "NULL"}"); // Füge diese Debug-Ausgabe hinzu
                }
                else
                {
                    Start = null;
                    Debug.WriteLine($"Start auf NULL gesetzt (TryParseExact fehlgeschlagen): {value}"); // Füge diese Debug-Ausgabe hinzu
                }

                Debug.WriteLine($"Start gesetzt (Property): {Start?.ToString() ?? "NULL"}"); // Füge diese Debug-Ausgabe hinzu
            }
        }


        private string _kommentar;
        public string Kommentar
        {
            get => _kommentar;
            set
            {
                if (_kommentar != value)
                {
                    _kommentar = value;
                    OnPropertyChanged(); // OnPropertyChanged für die Kommentar-Property aufrufen
                    Debug.WriteLine($"Kommentar (Property Setter): {_kommentar}"); // Füge diese Debug-Ausgabe hinzu
                }
            }
        }

        private decimal _entfernung;
        public decimal Entfernung
        {
            get => _entfernung;
            set
            {
                if (_entfernung != value)
                {
                    _entfernung = value;
                    OnPropertyChanged(nameof(Entfernung));
                }
            }
        }


        private int _planNr;
        public int PlanNr
        {
            get => _planNr;
            set
            {
                if (_planNr != value)
                {
                    _planNr = value;
                    OnPropertyChanged(nameof(PlanNr));
                }
            }
        }


        private decimal _regeneration;
        public decimal Regeneration
        {
            get => _regeneration;
            set
            {
                if (_regeneration != value)
                {
                    _regeneration = value;
                    OnPropertyChanged(nameof(Regeneration));
                    Debug.WriteLine($"🧠 Regeneration gesetzt: {_regeneration}");
                }
            }
        }


        public ObservableCollection<string> PlanNrList { get; set; } = new();

        public async void LadePlaene()
        {
            string connectionString = "data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Planung WHERE erledigt < 1";
                SqlCommand cmd = new SqlCommand(query, con);

                int spalten_nr = 4; // z. B. "PlanNummer"

                try
                {
                    await con.OpenAsync();
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    PlanNrList.Clear();
                    while (await dr.ReadAsync())
                    {
                        PlanNrList.Add(dr.GetValue(spalten_nr).ToString());
                    }

                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der Daten: " + ex.Message);
                }
            }
        }


        private decimal _kcal;
        public decimal Kcal
        {
            get => _kcal;
            set
            {
                if (_kcal != value)
                {
                    _kcal = value;
                    Debug.WriteLine($"🔥 Kcal gesetzt: {_kcal}");
                    OnPropertyChanged(nameof(Kcal));
                }
            }
        }



    }
}