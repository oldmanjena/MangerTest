using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using ManagerTest;
using MangerTest.Klassen;

namespace MangerTest.ViewModel
{
    public class TrainingsViewModel : ViewModelBase
    {
        public ObservableCollection<TrainingsEintrag> TrainingsDaten { get; set; }

        private TrainingsEintrag _neuerEintrag;
        public TrainingsEintrag NeuerEintrag
        {
            get => _neuerEintrag;
            set { _neuerEintrag = value; OnPropertyChanged(); }
        }

        public ICommand EintragenCommand { get; }

        public TrainingsViewModel()
        {
            TrainingsDaten = new ObservableCollection<TrainingsEintrag>();
            NeuerEintrag = new TrainingsEintrag();
            EintragenCommand = new RelayCommand(EintragHinzufuegen);
            TrainingsDaten = new ObservableCollection<TrainingsEintrag>();
            NeuerEintrag = new TrainingsEintrag
            {
                Datum = DateTime.Now // Aktuelles Datum setzen
            };
            EintragenCommand = new RelayCommand(EintragHinzufuegen);
        }

        private void EintragHinzufuegen2(object obj)
        {
            TrainingsDaten.Add(new TrainingsEintrag
            {
                Datum = NeuerEintrag.Datum,
                KW = NeuerEintrag.KW,
                Wochentag = NeuerEintrag.Wochentag,
                Start = NeuerEintrag.Start,
                Dauer = NeuerEintrag.Dauer,
                RPM = NeuerEintrag.RPM,
                Entfernung = NeuerEintrag.Entfernung,
                Kcal = NeuerEintrag.Kcal,
                Puls = NeuerEintrag.Puls,
                Aerober = NeuerEintrag.Aerober,
                Anaerober = NeuerEintrag.Anaerober,
                Regeneration = NeuerEintrag.Regeneration,
                VO2max = NeuerEintrag.VO2max,
                PlanNr = NeuerEintrag.PlanNr,
                Kommentar = NeuerEintrag.Kommentar
            });
        }

        private void EintragHinzufuegen(object obj)
        {
           
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["gesundheit"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Training(Datum, KW, Wochentag, Start, Dauer, RPM, Entfernung, kcal, Puls, Aerober, Anearober, Regenaration, VO2max, Plan_Nr, Kommentar, Per_id) " +
                        "VALUES (@Datum, @KW, @Wochentag, @Start, @Dauer, @RPM, @Entfernung, @kcal, @Puls, @Aerober, @Anearober, @Rege, @VO2max, @Plan_Nr, @Kommentar, @perid)", con))
                    {
                        cmd.Parameters.AddWithValue("@Datum", NeuerEintrag.Datum);
                        cmd.Parameters.AddWithValue("@KW", NeuerEintrag.KW);
                        cmd.Parameters.AddWithValue("@Wochentag", NeuerEintrag.Wochentag);
                        cmd.Parameters.AddWithValue("@Start", NeuerEintrag.Start);
                        cmd.Parameters.AddWithValue("@Dauer", NeuerEintrag.Dauer);
                        cmd.Parameters.AddWithValue("@RPM", NeuerEintrag.RPM);
                        cmd.Parameters.AddWithValue("@Entfernung", NeuerEintrag.Entfernung);
                        cmd.Parameters.AddWithValue("@kcal", NeuerEintrag.Kcal);
                        cmd.Parameters.AddWithValue("@Puls", NeuerEintrag.Puls);
                        cmd.Parameters.AddWithValue("@Aerober", NeuerEintrag.Aerober);
                        cmd.Parameters.AddWithValue("@Anearober", NeuerEintrag.Anaerober);
                        cmd.Parameters.AddWithValue("@Rege", NeuerEintrag.Regeneration);
                        cmd.Parameters.AddWithValue("@VO2max", NeuerEintrag.VO2max);
                        cmd.Parameters.AddWithValue("@Plan_Nr", NeuerEintrag.PlanNr);
                        cmd.Parameters.AddWithValue("@Kommentar", NeuerEintrag.Kommentar);
                        cmd.Parameters.AddWithValue("@perid", "1"); // Fester Wert oder dynamisch setzen

                        cmd.ExecuteNonQuery();
                    }
                }

                TrainingsDaten.Add(new TrainingsEintrag
                {
                    Datum = NeuerEintrag.Datum,
                    KW = NeuerEintrag.KW,
                    Wochentag = NeuerEintrag.Wochentag,
                    Start = NeuerEintrag.Start,
                    Dauer = NeuerEintrag.Dauer,
                    RPM = NeuerEintrag.RPM,
                    Entfernung = NeuerEintrag.Entfernung,
                    Kcal = NeuerEintrag.Kcal,
                    Puls = NeuerEintrag.Puls,
                    Aerober = NeuerEintrag.Aerober,
                    Anaerober = NeuerEintrag.Anaerober,
                    Regeneration = NeuerEintrag.Regeneration,
                    VO2max = NeuerEintrag.VO2max,
                    PlanNr = NeuerEintrag.PlanNr,
                    Kommentar = NeuerEintrag.Kommentar
                });

                MessageBox.Show("Eintrag erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Datenbankfehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
