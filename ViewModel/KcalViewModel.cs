using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;


namespace MangerTest.ViewModel
{
    public class KcalViewModel : INotifyPropertyChanged
    {
        private string _kcal;
        public string Kcal
        {
            get => _kcal;
            set
            {
                _kcal = value;
                OnPropertyChanged(nameof(Kcal));
                MessageBox.Show($"Gewicht geändert: {_kcal}"); // Debug-Ausgabe
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    

            public void LadeKcal()
                    {
                        string con = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";
                        using (SqlConnection conn = new SqlConnection(con))
                        {
                            conn.Open();
                            string commandText = "SELECT DATEPART(ISO_WEEK, Datum) AS Woche, " +
                                     "SUM(Entfernung) AS KM, " +
                                     "SUM(kcal) AS Kcal, " +
                                     "CAST(FORMAT(SUM(DATEPART(HOUR, Dauer)) + " +
                                     "SUM(DATEPART(MINUTE, Dauer)) / 60.0 + " +
                                     "SUM(DATEPART(SECOND, Dauer)) / 3600.0, 'N2', 'en-US') " +
                                     "AS DECIMAL(10, 2)) AS ZeitInStunden " +
                                     "FROM Training " +
                                     "WHERE YEAR(Datum) = YEAR(GETDATE()) " +  // Filter für das aktuelle Jahr
                                     "GROUP BY DATEPART(ISO_WEEK, Datum)" +
                                     "ORDER BY Woche DESC;";
                using (SqlCommand command = new SqlCommand(commandText, conn))
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    decimal kcal = reader.GetDecimal(2);
                                    MessageBox.Show($"Geladener Wert aus DB: {kcal}"); // Debug-Ausgabe
                                    Kcal = kcal.ToString("00.00"); // Automatische Aktualisierung der TextBox
                                }
                            }
                        }
                    }
                }
}
