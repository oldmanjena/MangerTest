using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MangerTest.Klassen;

namespace MangerTest.ViewModel
{
    public class PlanFreiesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _ausgewaehlteTechnik;
        private string _ausgewaehlteWoche;
        private string _was;
        private decimal _testenWert;
        private string _differenz;
        private DateTime _startDatum = DateTime.Today;
        private DateTime _endDatum;
        private int _trainingstage;
        private int _gesamtTage;
        private int _trainingseinheiten;
        private string _kommentar;
        private ICommand _eintragSpeichernCommand;
        private int _tageGesamt;
        private int _tageEffektiv;
        private readonly string ConnectionString = "data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true";

        public PlanFreiesViewModel()
        {
            AusgewaehlteTechnik = Split.FirstOrDefault();
            LetzteNummerLaden();
        }

        public ObservableCollection<string> WochenOptionen { get; } = new()
        {
            "4 Wochen", "6 Wochen", "8 Wochen", "10 Wochen", "12 Wochen", "14 Wochen", "16 Wochen"
        };

        public List<string> Split { get; } = new() { "2er", "3er", "4er", "5er" };

        public string AusgewaehlteTechnik
        {
            get => _ausgewaehlteTechnik;
            set
            {
                if (_ausgewaehlteTechnik != value)
                {
                    _ausgewaehlteTechnik = value;
                    OnPropertyChanged();
                    BerechneTrainingseinheiten();
                }
            }
        }

        public string AusgewaehlteWoche
        {
            get => _ausgewaehlteWoche;
            set
            {
                if (_ausgewaehlteWoche != value)
                {
                    _ausgewaehlteWoche = value;
                    OnPropertyChanged();
                    BerechneTrainingseinheiten();
                }
            }
        }

        public string Kommentar
        {
            get => _kommentar;
            set
            {
                if (_kommentar != value)
                {
                    _kommentar = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Trainingseinheiten
        {
            get => _trainingseinheiten;
            set
            {
                _trainingseinheiten = value;
                OnPropertyChanged();
            }
        }

        public int Trainingstage
        {
            get => _trainingstage;
            set
            {
                _trainingstage = value;
                OnPropertyChanged();
            }
        }

        public int GesamtTage
        {
            get => _gesamtTage;
            set
            {
                _gesamtTage = value;
                OnPropertyChanged();
            }
        }

        public string Differenz
        {
            get => _differenz;
            set
            {
                _differenz = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDatum
        {
            get => _startDatum;
            set
            {
                _startDatum = value;
                OnPropertyChanged();
                AktualisiereEndDatum();
            }
        }

        public DateTime EndDatum
        {
            get => _endDatum;
            set
            {
                _endDatum = value;
                OnPropertyChanged();
            }
        }

        public string Was
        {
            get => _was;
            set
            {
                if (_was != value)
                {
                    _was = value;
                    OnPropertyChanged();
                    Debug.WriteLine($"Was wurde gesetzt: '{_was}'");

                    if (!string.IsNullOrEmpty(_was))
                    {
                        LetzteNummerLaden();
                    }
                    else
                    {
                        Debug.WriteLine("Was ist leer, keine Nummer geladen.");
                    }
                }
            }
        }

        public decimal TestenWert
        {
            get => _testenWert;
            set
            {
                if (_testenWert != value)
                {
                    _testenWert = value;
                    OnPropertyChanged();
                }
            }
        }

        private void BerechneTrainingseinheiten()
        {
            if (string.IsNullOrEmpty(AusgewaehlteTechnik) || string.IsNullOrEmpty(AusgewaehlteWoche))
                return;

            if (int.TryParse(AusgewaehlteTechnik[0].ToString(), out int tageProWoche))
            {
                string nurZahl = new(AusgewaehlteWoche.Where(char.IsDigit).ToArray());
                if (int.TryParse(nurZahl, out int wochen))
                {
                    _tageGesamt = wochen * 7;
                    _tageEffektiv = tageProWoche * wochen;

                    Trainingseinheiten = _tageEffektiv;
                    GesamtTage = _tageGesamt;
                    Trainingstage = _tageEffektiv;
                    Differenz = _tageEffektiv.ToString();
                    AktualisiereEndDatum();
                }
            }
        }

        private void AktualisiereEndDatum()
        {
            EndDatum = StartDatum.AddDays(_tageGesamt);
        }

        public async Task LetzteNummerLaden()
        {
            Debug.WriteLine("LetzteNummerLaden() wurde aufgerufen");
            try
            {
                using SqlConnection connection = new(ConnectionString);
                await connection.OpenAsync();

                string query = "SELECT MAX(Plan_Nr) FROM Planung WHERE Was LIKE '%' + @was + '%'";
                Debug.WriteLine($"SQL Abfrage wird ausgeführt mit Was = '{Was}'");
                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@was", string.IsNullOrEmpty(Was) ? (object)DBNull.Value : "%" + Was + "%");

                object result = await command.ExecuteScalarAsync();
                Debug.WriteLine($"SQL-Ergebnis: {result}");

                if (result != null && int.TryParse(result.ToString(), out int neueNummer))
                {
                    TestenWert = neueNummer + 1;
                    Debug.WriteLine($"Neue Nummer geladen: {TestenWert}");
                }
                else
                {
                    TestenWert = 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"FEHLER in LetzteNummerLaden: {ex.Message}");
                TestenWert = 1;
            }
        }

        public ICommand EintragSpeichernCommand => _eintragSpeichernCommand ??= new RelayCommand(o => EintragSpeichern());

        public void EintragSpeichern()
        {
            string connString = "data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true";

            using SqlConnection con = new(connString);
            con.Open();
            using SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Planung 
                (Was, Start, Ende, Plan_Nr, Einheiten, Tage, Wochen, erledigt, Kommentar) 
                VALUES 
                (@was, @start, @ende, @nr, @einheiten, @tage, @wochen, @erledigt, @kommentar)";

            cmd.Parameters.AddWithValue("@was", Was ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@start", StartDatum);
            cmd.Parameters.AddWithValue("@ende", EndDatum);
            cmd.Parameters.AddWithValue("@nr", TestenWert);
            cmd.Parameters.AddWithValue("@einheiten", GesamtTage);
            cmd.Parameters.AddWithValue("@tage", Trainingstage);
            cmd.Parameters.AddWithValue("@wochen", new string(AusgewaehlteWoche.Where(char.IsDigit).ToArray()));
            cmd.Parameters.AddWithValue("@erledigt", false);
            cmd.Parameters.AddWithValue("@kommentar", Kommentar ?? string.Empty);

            cmd.ExecuteNonQuery();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
