using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;

namespace MangerTest.ViewModel
{
    public class GewichtViewModel : INotifyPropertyChanged
    {
        private string _gewicht;
        public string Gewicht
        {
            get => _gewicht;
            set
            {
                _gewicht = value;
                OnPropertyChanged(nameof(Gewicht));
               // MessageBox.Show($"Gewicht geändert: {_gewicht}"); // Debug-Ausgabe
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void LadeGewicht()
        {
            string con = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string commandText = "SELECT TOP 1 Gewicht FROM Fitdays ORDER BY Datum DESC;";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal gewicht = reader.GetDecimal(0);
                        //MessageBox.Show($"Geladener Wert aus DB: {gewicht}"); // Debug-Ausgabe
                        Gewicht = gewicht.ToString("00.00"); // Automatische Aktualisierung der TextBox
                    }
                }
            }
        }
    }
}
