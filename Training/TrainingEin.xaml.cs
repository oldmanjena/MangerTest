using MangerTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
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
using Calendar = System.Globalization.Calendar;

namespace MangerTest.Training
{
    /// <summary>
    /// Interaktionslogik für TrainingEin.xaml
    /// </summary>
    public partial class TrainingEin : Window
    {
        public TrainingEin()
        {
            InitializeComponent();
            DataContext = new CombinedViewModel();
            Cb_Groesse(); // Daten in die ComboBox laden
            //UpdateKalenderwoche(DateTime.Today);


        }

        public class CombinedViewModel
        {
            public TrainingsViewModel TrainingsVM { get; set; } = new();
            public TimePickerViewModel TimePickerVM { get; set; } = new();

           
        }

        private void Cb_Groesse()
        {
            string connectionString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Planung WHERE erledigt < 1";
                SqlCommand cmd = new SqlCommand(query, con);

                int spalten_nr = 4; // Spaltenindex für die gewünschten Werte

                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    cmbPlan.Items.Clear(); // Falls die ComboBox bereits Einträge enthält
                    while (dr.Read())
                    {
                        cmbPlan.Items.Add(dr.GetValue(spalten_nr).ToString());
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der Daten: " + ex.Message);
                }
            }
        }


        private void UpdateKalenderwoche(DateTime datum)
        {
            // Kultur verwenden, z. B. Deutsch (ISO 8601)
            CultureInfo ci = CultureInfo.CurrentCulture;
            Calendar cal = ci.Calendar;

            // Kalenderwoche nach ISO 8601 (erste Woche mit mindestens 4 Tagen)
            CalendarWeekRule rule = CalendarWeekRule.FirstFourDayWeek;
            DayOfWeek firstDay = DayOfWeek.Monday;

            int kw = cal.GetWeekOfYear(datum, rule, firstDay);
            txtKw.Text = $"KW {kw}";
        }

    }
}
