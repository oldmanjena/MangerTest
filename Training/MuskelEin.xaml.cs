using MangerTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel; // Für INotifyPropertyChanged
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MangerTest.Training
{
    /// <summary>
    /// Interaktionslogik für MuskelEin.xaml
    /// </summary>
    public partial class MuskelEin : Window, INotifyPropertyChanged
    {
        private string connectionString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true"; // Ersetze dies mit deiner tatsächlichen Verbindungszeichenfolge
        private Dictionary<string, List<string>> ZielDict = new Dictionary<string, List<string>>();
        public ObservableCollection<TrainingEntry> TrainingEntries { get; set; } = new ObservableCollection<TrainingEntry>();
        private CollectionViewSource _letzteTrainingData;
        public CollectionViewSource LetzteTrainingData
        {
            get { return _letzteTrainingData; }
            set
            {
                _letzteTrainingData = value;
                OnPropertyChanged(nameof(LetzteTrainingData));
            }
        }
        int kr;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MuskelEin()
        {
            InitializeComponent();
            DataContext = new TrainartViewModel();
            DataContext = new AktuellesDatumViewModel(); // Hier den neuen Namen verwenden
            FillComboBox(); // Rufe die Methode beim Initialisieren des Fensters auf

            // Fülle das Dictionary mit den Gruppen und den zugehörigen Übungen
            ZielDict.Add("Krank", new List<string> { "krank" });
            ZielDict.Add("Rücken", new List<string> { "Latissimus", "Trapezius", "unterer Rücken" });
            ZielDict.Add("Beine", new List<string> { "Quardrizeps", "Harnstrings" });
            ZielDict.Add("Brust", new List<string> { "Obere Brust", "Mittlere Brust" });
            ZielDict.Add("Bizeps", new List<string> { "gesamter Bizeps" });
            ZielDict.Add("Trizeps", new List<string> { "gesamter Trizeps" });
            ZielDict.Add("Schulter", new List<string> { "vordere", "mittlere", "hintere" });
            ZielDict.Add("Bauch", new List<string> { "oberer", "geamter", "unterer", "schräger" });
            ZielDict.Add("Unterarm", new List<string> { "gesamter" });
            ZielDict.Add("Ganzkörper", new List<string> { "gesamter" });

            // Füge die Gruppen in die ComboBox cmbGruppe hinzu
            foreach (var gruppe in ZielDict.Keys)
            {
                cmbGruppe.Items.Add(gruppe);
            }

            // Combobox auf 0 setzen
            cmbGruppe.SelectedIndex = 0;
            dtgUeber.AutoGeneratingColumn += DtgUeber_AutoGeneratingColumn;
            Daten();
            dtgUeber.ItemsSource = TrainingEntries;
            cmbArt.ItemsSource = new[] { "Definition", "Masseaufbau", "Diät" };
            cmbArt.SelectedIndex = 0; // Wählt "Definition" aus
        }

        private void FillComboBox()
        {
            cmbtrainingsnummer.Items.Clear(); // ComboBox leeren, um Duplikate zu vermeiden

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Planung WHERE erledigt < 1"; // Abfrage, um eindeutige Einträge aus der Spalte "Plan_Nr" zu erhalten
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbtrainingsnummer.Items.Add(reader["Plan_Nr"].ToString()); // Eindeutige Einträge zur ComboBox hinzufügen
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Abrufen der Daten: " + ex.Message);
                }
            }
        }

        private void cmbGruppe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //rdbVergleich.Checked = false;
            // Lösche alle vorhandenen Einträge in cmbUebung
            cmbZiel.Items.Clear();

            // Wenn eine Gruppe ausgewählt ist, fülle cmbUebung mit den entsprechenden Übungen
            if (cmbGruppe.SelectedItem != null)
            {
                string selectedGruppe = cmbGruppe.SelectedItem.ToString();
                if (ZielDict.ContainsKey(selectedGruppe))
                {
                    foreach (var uebung in ZielDict[selectedGruppe])
                    {
                        cmbZiel.Items.Add(uebung);
                    }
                }
            }
            cmbZiel.SelectedIndex = 0;
            //MessageBox.Show("Code wird ausgeführt");
            if (cmbGruppe.SelectedItem != null && cmbGruppe.SelectedItem.ToString() == "Bauch")
            {
                //MessageBox.Show("Bauch ausgewählt");
                // txtGewicht.Visible = false;
                //  txtGewicht.Text = Convert.ToString("0");
                //    lblGruppe.Visible = false;
            }
            else
            {
                // MessageBox.Show("Anderes ausgewählt");
                //    txtGewicht.Visible = true;
                //    lblGruppe.Visible = true;
            }
            //rdbWH6.Checked = false;
        }

        private async void chkletztes_Checked(object sender, RoutedEventArgs e)
        {
            await LetzteAsync();
        }

        private async Task LetzteAsync()
        {
            string selectedUebung = txtUebung.Text;

            string ConString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";
            string CmdString = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    await con.OpenAsync();

                    CmdString = "SELECT TOP 1 CAST(Wann AS DATE) AS Wann, Uebung, Satz, Wh, Gewicht " +
                                "FROM Muskel " +
                                "WHERE Krank = 0 AND Uebung LIKE '%' + @uebung + '%' " +
                                "ORDER BY Wann DESC;";

                    using (SqlCommand cmd = new SqlCommand(CmdString, con))
                    {
                        cmd.Parameters.AddWithValue("@uebung", selectedUebung);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable("letzteTraining");
                            dt.Load(reader);

                            LetzteTrainingData = new CollectionViewSource { Source = dt.DefaultView };
                            dtgUeber.ItemsSource = LetzteTrainingData.View;

                            // Letzten Eintrag in Textboxen einfügen (Gewicht & Wiederholungen)
                            if (dt.Rows.Count > 0)
                            {
                                txtletztGewicht.Text = dt.Rows[0]["Gewicht"].ToString();
                                txtLetztWh.Text = dt.Rows[0]["Wh"].ToString();
                            }
                            else
                            {
                                txtletztGewicht.Text = string.Empty;
                                txtLetztWh.Text = string.Empty;
                            }
                        }
                    }
                }

                dtgUeber.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der letzten Trainingsdaten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Ver_Click(object sender, RoutedEventArgs e)
        {
            {
                string selectedGruppe = cmbGruppe.Text;
                string selectedUebung = txtUebung.Text;
                string selectedArt = cmbArt.Text;

                string connectionString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";

                string query = @"
                                     SELECT TOP 1 Wann, art, Uebung, Zielmuskel, Satz, Wh, Gewicht
                                     FROM Muskel 
                                     WHERE art = @art 
                                     AND muskelgruppe = @muskelgruppe 
                                     AND Uebung LIKE '%' + @uebung + '%'";

                DataTable dataTable = new DataTable();

                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@art", selectedArt);
                    command.Parameters.AddWithValue("@muskelgruppe", selectedGruppe);
                    command.Parameters.AddWithValue("@uebung", selectedUebung);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }

                dtgUeber.ItemsSource = dataTable.DefaultView;
                dtgUeber.Visibility = Visibility.Visible;
                // Spalten anpassen
                foreach (var column in dtgUeber.Columns)
                {
                    column.Width = DataGridLength.SizeToHeader;
                    column.HeaderStyle = new Style(typeof(DataGridColumnHeader))
                    {
                        Setters =
                        {
                            new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Center)
                        }
                    };
                }
            }
        }


        private void Daten()
        {
            string connectionString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";
            string query = "SELECT Uebung, Wann, Wh, Gewicht FROM Muskel WHERE Art = 'Arbeitssatz' AND Gewicht > 0 ORDER BY Uebung, Wann";

            DataTable dataTable = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    TrainingEntry entry = new TrainingEntry
                    {
                        Uebung = row["Uebung"].ToString(),
                        Wann = DateTime.Parse(row["Wann"].ToString()),
                        Wh = int.Parse(row["Wh"].ToString()),
                        Gewicht = double.Parse(row["Gewicht"].ToString())
                    };
                    TrainingEntries.Add(entry);
                }
                dtgUeber.ItemsSource = TrainingEntries;
                dtgUeber.Visibility = Visibility.Visible;

                // Wenn du Analyse brauchst:
                // var results = AnalyzeProgressiveDevelopment(TrainingEntries.ToList());
                // dtgUeber.ItemsSource = results;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Datenbankfehler: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Allgemeiner Fehler: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class TrainingEntry
        {
            public string Uebung { get; set; }
            public DateTime Wann { get; set; }
            public int Wh { get; set; }
            public double Gewicht { get; set; }

            // Neue Eigenschaft für Anzeigezwecke
            //  public string WannFormatted => Wann.ToString("dd.MM.yyyy");
        }

        private void DtgUeber_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Wann")
            {
                var col = e.Column as DataGridTextColumn;
                if (col != null)
                {
                    col.Binding = new Binding("Wann")
                    {
                        StringFormat = "dd.MM.yyyy"
                    };
                }
            }
        }


        private void chkKrank_Checked(object sender, RoutedEventArgs e)
        {
            if (chkKrank.IsChecked == true)
            {
                chkNein.IsChecked = false;
                kr = 1;
                txtGewicht.Text = "0"; // Direkte Zuweisung als String ist besser in WPF
                cmbGruppe.SelectedIndex = 9;
                MessageBox.Show(kr.ToString()); // MessageBox gehört zu WinForms, in WPF nutzt man z.B. eigene Dialoge oder Logging
            }
        }
        private void chkNein_Checked(object sender, RoutedEventArgs e)
        {
            if (chkNein.IsChecked == true)
            {
                chkKrank.IsChecked = false;
                kr = 0;
                MessageBox.Show(kr.ToString()); // MessageBox gehört zu WinForms
            }
        }

        private void btn_Ende_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ver()
        {

            decimal a = 0;
            decimal b = 0;



            decimal i = Convert.ToDecimal((string)txtGewicht.Text);
            decimal o = Convert.ToDecimal((string)txtletztGewicht.Text);

            decimal e = Convert.ToDecimal((string)txtWh.Text);
            decimal f = Convert.ToDecimal((string)txtLetztWh.Text);
            decimal p, percentChange;
            decimal u, percentChange1;

            //  MessageBox.Show("i: " + i);
            //  MessageBox.Show("o: " + o);

            if (o == 0)
            {
                txtWh.Text = a.ToString();
                txtVerPro.Text = b.ToString();
            }
            else
            {
                p = i - o; // absolute Veränderung
                percentChange = (p / o) * 100; // prozentuale Veränderung

                txtVer.Text = p.ToString("00.00"); // absolute Veränderung
                txtVerPro.Text = percentChange.ToString("00.00") + "%"; // prozentuale Veränderung
            }

            if (f == 0)
            {
                txtLetztWh.Text = e.ToString();
                txtWHVer.Text = f.ToString();
            }
            else
            {
                u = e - f; // absolute Veränderung
                percentChange1 = (u / f) * 100; // prozentuale Veränderung

                txtWHVer.Text = u.ToString("00.00"); // absolute Veränderung
                txtWhProz.Text = percentChange1.ToString("00.00") + "%"; // prozentuale Veränderung
            }

        }

        private void txtGewicht_TextChanged(object sender, TextChangedEventArgs e)
        {
            ver();
        }

        private async void txtUebung_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LetzteAsync();
        }
    }
}