using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using ManagerTest.Views;
using MangerTest;

namespace MangerTest.Blut
{
    /// <summary>
    /// Interaktionslogik für ErfassBlut.xaml
    /// </summary>
    public partial class ErfassBlut : Window
    {
        public ErfassBlut()
        {
            InitializeComponent();

            txtZeit.Text = DateTime.Now.ToString("HH:mm");
            DataContext = this; // Stelle sicher, dass der DataContext gesetzt wird
        }

        private string BerechneTageszeit(DateTime zeit)
        {
            int stunde = zeit.Hour;
            return stunde switch
            {
                >= 5 and < 12 => "Morgen",
                >= 12 and < 14 => "Mittag",
                >= 14 and < 18 => "Nachmittag",
                >= 18 and < 22 => "Abend",
                _ => "Nacht"
            };
        }

        private void Eintragen()
        {



            if (!int.TryParse(txtSys.Text, out int sys) ||
                !int.TryParse(txtDia.Text, out int dia) ||
                !int.TryParse(txtPuls.Text, out int puls))
            {
                MessageBox.Show("Bitte gültige Werte für Blutdruck und Puls eingeben.");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["gesundheit"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Blutdruck(Datum, Uhrzeit, Systole, Diastole, Puls, Tageszeit, Bemerkung) VALUES (@wann, @zeit, @sys, @dia, @puls, @tag, @bemer)", con))
                {
                    cmd.Parameters.AddWithValue("@wann", dtpDatum.SelectedDate ?? DateTime.Now);
                    DateTime zeit;
                    if (DateTime.TryParseExact(txtZeit.Text, "HH:mm", null, System.Globalization.DateTimeStyles.None, out zeit))
                    {
                        // Der Benutzer hat eine gültige Uhrzeit eingegeben
                        cmd.Parameters.AddWithValue("@zeit", zeit);
                    }
                    else
                    {
                        // Ungültige Eingabe oder leeres Textfeld, daher aktuelle Zeit verwenden
                        cmd.Parameters.AddWithValue("@zeit", DateTime.Now);
                    }
                    cmd.Parameters.AddWithValue("@sys", sys);
                    cmd.Parameters.AddWithValue("@dia", dia);
                    cmd.Parameters.AddWithValue("@puls", puls);
                    cmd.Parameters.AddWithValue("@tag", txtTageszeit.Text);
                    cmd.Parameters.AddWithValue("@bemer", txtBemerkung.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["gesundheit"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Datum, Uhrzeit, Systole, Diastole, Puls, Tageszeit, Bemerkung FROM Blutdruck ORDER BY Datum DESC, Uhrzeit DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Die Daten an das DataGrid binden
                dtgDaten.ItemsSource = dt.DefaultView;
            }
        }

        private void BtnEnde_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void txtZeit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DateTime.TryParse(txtZeit.Text, out DateTime zeit)) // Überprüfung auf gültige Zeit
            {
                txtTageszeit.Text = BerechneTageszeit(zeit);
            }
            else
            {
                txtTageszeit.Text = "Ungültige Zeit"; // Fehlerbehandlung
            }
        }

        private void btnEintragen_Click_1(object sender, RoutedEventArgs e)
        {
            Eintragen();
            MessageBox.Show("Daten wurden aktualisiert!");
        }


        private void CheckSystolicBloodPressure()
        {
            if (int.TryParse(txtSys.Text, out int systolic))
            {
                if (systolic >= 140) // Zu hoch nach WHO
                {
                    txtSys.Background = new SolidColorBrush(Colors.Red);
                    txtSys.Foreground = new SolidColorBrush(Colors.White);
                    UpdateTextBox("Systole", "zu hoch.");
                }
                else if (systolic >= 90 && systolic < 140) // Normal nach WHO
                {
                    txtSys.Background = new SolidColorBrush(Colors.Green);
                    txtSys.Foreground = new SolidColorBrush(Colors.White);
                    txtSys.FontWeight = FontWeights.Bold;
                    UpdateTextBox("Systole", "normal.");
                }
                else if (systolic < 90) // Zu niedrig nach WHO
                {
                    txtSys.Background = new SolidColorBrush(Colors.Yellow);
                    txtSys.Foreground = new SolidColorBrush(Colors.Black);
                    UpdateTextBox("Systole", "zu niedrig.");
                }
            }
            else
            {
                txtSys.Background = SystemColors.WindowBrush;
                txtSys.Foreground = SystemColors.WindowTextBrush;
                UpdateTextBox("Systole", "Ungültige Eingabe.");
            }
        }

        private void CheckDiastolicBloodPressure()
        {
            if (int.TryParse(txtDia.Text, out int diastolic))
            {
                if (diastolic >= 90) // Zu hoch nach WHO
                {
                    txtDia.Background = new SolidColorBrush(Colors.Red);
                    txtDia.Foreground = new SolidColorBrush(Colors.White);
                    UpdateTextBox("Diastole", "zu hoch.");
                }
                else if (diastolic >= 60 && diastolic < 90) // Normal nach WHO
                {
                    txtDia.Background = new SolidColorBrush(Colors.Green);
                    txtDia.Foreground = new SolidColorBrush(Colors.White);
                    txtDia.FontWeight = FontWeights.Bold;  // Verwende WPF spezifische Schriftstile
                    UpdateTextBox("Diastole",  "normal.");
                }
                else if (diastolic < 60) // Zu niedrig nach WHO
                {
                    txtDia.Background = new SolidColorBrush(Colors.Yellow);
                    txtDia.Foreground = new SolidColorBrush(Colors.Black);
                    UpdateTextBox("Diastole", "zu niedrig.");
                }
            }
            else
            {
                txtDia.Background = SystemColors.WindowBrush;
                txtDia.Foreground = SystemColors.WindowTextBrush;
                UpdateTextBox("Diastole", "Ungültige Eingabe.");
            }
        }

        private void CheckPulse()
        {
            if (int.TryParse(txtPuls.Text, out int pulse))
            {
                if (pulse > 100) // Zu hoch nach Standard
                {
                    txtPuls.Background = new SolidColorBrush(Colors.Red);
                    txtPuls.Foreground = new SolidColorBrush(Colors.White);
                    UpdateTextBox("Puls", "Puls zu hoch.");
                }
                else if (pulse >= 60 && pulse <= 100) // Normaler Puls
                {
                    txtPuls.Background = new SolidColorBrush(Colors.Green);
                    txtPuls.Foreground = new SolidColorBrush(Colors.White);
                    txtPuls.FontWeight = FontWeights.Bold;  // Verwende WPF spezifische Schriftstile
                    UpdateTextBox("Puls", "Puls normal.");
                }
                else if (pulse < 60) // Zu niedrig
                {
                    txtPuls.Background = new SolidColorBrush(Colors.Yellow);
                    txtPuls.Foreground = new SolidColorBrush(Colors.Black);
                    UpdateTextBox("Puls", "Puls zu niedrig.");
                }
            }
            else
            {
                txtPuls.Background = SystemColors.WindowBrush;
                txtPuls.Foreground = SystemColors.WindowTextBrush;
                UpdateTextBox("Puls", "Ungültige Eingabe.");
            }
        }

        private void UpdateTextBox(string category, string message)
        {
            var lines = txtBemerkung.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Alte Einträge für die gleiche Kategorie entfernen
            lines.RemoveAll(line => line.StartsWith(category));

            // Neue Nachricht hinzufügen
            lines.Add($"{category}: {message}");

            // Aktualisierten Text setzen
            txtBemerkung.Text = string.Join(Environment.NewLine, lines);
        }



        private void txtSys_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckSystolicBloodPressure();
        }

        private void txtDia_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckDiastolicBloodPressure();
        }

        private void txtPuls_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPulse();
        }

       

        private void btnPruefen_Click(object sender, RoutedEventArgs e)
        {
            AuswertungWindow auswertung = new AuswertungWindow();
            auswertung.ShowDialog(); // Jetzt funktioniert ShowDialog()
        }

        

        private void txtBemerkung_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void FillDataGrid()

        {

            string ConString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";

            string CmdString = string.Empty;

            using (SqlConnection con = new SqlConnection(ConString))
            {

                CmdString = "select * from Blutdruck ORDER BY Datum DESC ";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("maximal");

                sda.Fill(dt);
                //dtgGewicht.AutoGeneratingColumn += Dtg_Gewicht_AutoGeneratedColumns;
                dtgDaten.ItemsSource = dt.DefaultView;

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

      

        private void Blutdruck_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }
       

       

        private void icoVorhof_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBemerkung.Text))
            {
                txtBemerkung.AppendText(Environment.NewLine + "Vorhofflimmern");
            }
            else
            {
                txtBemerkung.Text = "Vorhofflimmern";
            }
        }
    }
}
