using MaterialDesignThemes.Wpf.AddOns.Utils.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MangerTest.Blut

{
    /// <summary>
    /// Interaktionslogik für SuBlut.xaml
    /// </summary>
    
    public partial class SuBlut : Window
    {
        private List<SuBlutdruck> SuBLutListe = new List<SuBlutdruck>();
        public SuBlut()
        {
            InitializeComponent();
            LadeDatenbankDaten();
        }


        private void LadeDatenbankDaten()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["gesundheit"].ConnectionString;

            

            string query = "SELECT blt_id, Datum, Uhrzeit, Systole, Diastole, Puls, Tageszeit, Bemerkung, PatientID FROM Blutdruck";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SuBLutListe.Add(new SuBlutdruck
                    {
                        BltId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),  // Standardwert 0 für NULL
                        Datum = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1),
                        Uhrzeit = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                        Systole = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Diastole = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                        Puls = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        Tageszeit = reader.IsDBNull(6) ? "" : reader.GetString(6),
                        Bemerkung = reader.IsDBNull(7) ? "" : reader.GetString(7),
                        PatientID = reader.IsDBNull(8) ? 0 : reader.GetInt32(8)
                    });
                }
            }

            dtgSuche.ItemsSource = SuBLutListe;
        }

        private void txtSuche_TextChanged(object sender, TextChangedEventArgs e)
        {

            string suchbegriff = txtSuche.Text.ToLower();
            var gefilterteListe = SuBLutListe
                .Where(m => m.Tageszeit.ToLower().Contains(suchbegriff) ||
                            m.Bemerkung.ToLower().Contains(suchbegriff) ||
                            m.PatientID.ToString().Contains(suchbegriff))
                .ToList();

            dtgSuche.ItemsSource = gefilterteListe;
        }
    }
}
