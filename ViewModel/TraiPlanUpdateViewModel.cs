using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MangerTest.Klassen;

namespace MangerTest.ViewModel
{

    public class TrainingEintrag : INotifyPropertyChanged
    {
        public ICommand SpeichernCommand { get; }

       
        public int ID { get; set; }

        private string _was;
        public string Was
        {
            get => _was;
            set => SetField(ref _was, value);
        }

        private DateTime? _start;
        public DateTime? Start
        {
            get => _start;
            set => SetField(ref _start, value);
        }

        private DateTime? _ende;
        public DateTime? Ende
        {
            get => _ende;
            set => SetField(ref _ende, value);
        }

        public int Plan_Nr { get; set; }

        private decimal _einheiten;
        public decimal Einheiten
        {
            get => _einheiten;
            set => SetField(ref _einheiten, value);
        }

        private int _tage;
        public int Tage
        {
            get => _tage;
            set => SetField(ref _tage, value);
        }

        private int _wochen;
        public int Wochen
        {
            get => _wochen;
            set => SetField(ref _wochen, value);
        }

        private int _erledigt;
        public int Erledigt
        {
            get => _erledigt;
            set => SetField(ref _erledigt, value);
        }

        private string _kommentar;
        public string Kommentar
        {
            get => _kommentar;
            set => SetField(ref _kommentar, value);
        }

        private decimal _startgewicht;
        public decimal Startgewicht
        {
            get => _startgewicht;
            set => SetField(ref _startgewicht, value);
        }

        private decimal _endgewicht;
        public decimal Endgewicht
        {
            get => _endgewicht;
            set => SetField(ref _endgewicht, value);
        }

        private bool _isModified;
        public bool IsModified
        {
            get => _isModified;
            set => SetField(ref _isModified, value, false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, bool markModified = true, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value)) return false;
            field = value;
            if (markModified) IsModified = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }

    public class TrainPlanUpdateViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = "data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true";

        public ObservableCollection<TrainingEintrag> TrainingsListe { get; set; } = new();

        private TrainingEintrag _ausgewaehlterEintrag;
        public TrainingEintrag AusgewaehlterEintrag
        {
            get => _ausgewaehlterEintrag;
            set
            {
                _ausgewaehlterEintrag = value;
                OnPropertyChanged();
            }
        }

        public ICommand SpeichernCommand { get; }

        public TrainPlanUpdateViewModel()
        {
            SpeichernCommand = new RelayCommand(_ => Speichern(), _ => CanSpeichern());
            _ = LadeTrainingsAsync();
        }

        private bool CanSpeichern()
        {
            return TrainingsListe.Any(t => t.IsModified); // Beispielbedingung: Es gibt Einträge, die geändert wurden
        }
        public async Task LadeTrainingsAsync()
        {
            const string query = "SELECT * FROM Planung ORDER BY ID DESC";

            try
            {
                using SqlConnection con = new(_connectionString);
                using SqlCommand cmd = new(query, con);
                await con.OpenAsync();
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();
                TrainingsListe.Clear();

                while (await reader.ReadAsync())
                {
                    TrainingsListe.Add(new TrainingEintrag
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        Was = reader["Was"]?.ToString(),
                        Start = reader["Start"] as DateTime?,
                        Ende = reader["Ende"] as DateTime?,
                        Plan_Nr = Convert.ToInt32(reader["Plan_Nr"]),
                        Einheiten = reader["Einheiten"] as decimal? ?? 0,
                        Tage = reader["Tage"] as int? ?? 0,
                        Wochen = reader["Wochen"] as int? ?? 0,
                        Erledigt = reader["erledigt"] as int? ?? 0,
                        Kommentar = reader["Kommentar"]?.ToString(),
                        Startgewicht = reader["startgewicht"] as decimal? ?? 0,
                        Endgewicht = reader["endgewicht"] as decimal? ?? 0,
                        IsModified = false
                    });
                }
            }
            catch (Exception ex)
            {
                // Logge den Fehler oder benachrichtige den Benutzer
                Console.WriteLine("Fehler beim Laden der Trainingsdaten: " + ex.Message);
            }
        }

        private void Speichern()
        {
            using SqlConnection con = new(_connectionString);
            con.Open();

            foreach (var eintrag in TrainingsListe.Where(t => t.IsModified))
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Planung SET 
                        Was = @Was,
                        Kommentar = @Kommentar,
                        erledigt = @Erledigt,
                        startgewicht = @Startgewicht,
                        endgewicht = @Endgewicht,
                        Ende = @Ende
                    WHERE ID = @ID", con);

                cmd.Parameters.AddWithValue("@Was", eintrag.Was ?? "");
                cmd.Parameters.AddWithValue("@Kommentar", eintrag.Kommentar ?? "");
                cmd.Parameters.AddWithValue("@Erledigt", eintrag.Erledigt);
                cmd.Parameters.AddWithValue("@Startgewicht", eintrag.Startgewicht);
                cmd.Parameters.AddWithValue("@Endgewicht", eintrag.Endgewicht);
                cmd.Parameters.AddWithValue("@Ende", (object?)eintrag.Ende ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ID", eintrag.ID);

                cmd.ExecuteNonQuery();
                eintrag.IsModified = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
