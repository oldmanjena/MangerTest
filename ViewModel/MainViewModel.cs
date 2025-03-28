using MangerTest;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Configuration;


namespace ManagerTest
{
    public class MainViewModel
    {
        private string connectionString = "Server=DESKTOP-726MH0T;Database=gesundheit;Trusted_Connection=True;";

        // ObservableCollection für die Blutdruckmessungen
        public ObservableCollection<BlutdruckMessungen> BlutdruckMessungen { get; set; }

        public MainViewModel()
        {
            BlutdruckMessungen = new ObservableCollection<BlutdruckMessungen>();
            LoadData();
        }

        // Methode zum Laden der Blutdruckmessungsdaten aus der Datenbank
        private void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["gesundheit"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    {
                        con.Open();

                        // SQL-Query zum Abrufen der Blutdruckmessungen
                        SqlCommand cmd = new SqlCommand("SELECT Blt_id, Datum, Uhrzeit, Systole, Diastole, Puls, Tageszeit, Bemerkung FROM Blutdruck ORDER BY Datum DESC", con);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            BlutdruckMessungen.Add(new BlutdruckMessungen
                            {
                                BltId = reader.GetInt32(0),
                                Datum = reader.GetDateTime(1),
                                Uhrzeit = reader.GetDateTime(2),
                                Systole = reader.GetInt32(3),
                                Diastole = reader.GetInt32(4),
                                Puls = reader.GetInt32(5),
                                Tageszeit = reader.GetString(6),
                                Bemerkung = reader.IsDBNull(7) ? null : reader.GetString(7),  // Falls Bemerkung null ist

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }
    }
}
