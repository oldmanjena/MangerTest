using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;


namespace MangerTest.ViewModel
{
    public class GewichtZielViewModel : INotifyPropertyChanged
    {
        private string _wert;

        public string Wert
        {
            get => _wert;
            set
            {
                if (_wert != value)  // Prüfe, ob sich der Wert wirklich geändert hat
                {
                    _wert = value;
                   // MessageBox.Show($"Wert changed: {value}");  // Prüfe den Wert hier
                    OnPropertyChanged(nameof(Wert));  // Benachrichtige die UI, dass sich der Wert geändert hat
                }
            }
        }

        public GewichtZielViewModel()
        {
            LadeGewicht();  // 🚀 Hier wird die Funktion automatisch beim Start aufgerufen
        }

        public void LadeGewicht()
        {
            string con = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                string commandText = "Select Wert from Ziele where Bezeichnung = 'Gewicht';";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal wert = reader.GetDecimal(0);
                        Wert = wert.ToString("00.00");  // Wert setzen, OnPropertyChanged wird aufgerufen
                        //MessageBox.Show("Neuer Wert: " + Wert);  // Prüfe den Wert in der Konsole
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            // Sicherstellen, dass der propertyName korrekt übergeben wird
           // MessageBox.Show($"Property {propertyName} has changed.");  // Prüfe die Ausgabe in der Konsole
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
