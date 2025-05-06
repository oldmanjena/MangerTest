using MangerTest.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel; // Für INotifyPropertyChanged
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

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

        public ObservableCollection<string> Muskelgruppen { get; set; }
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
            // DataContext = new TrainartViewModel();
            //  DataContext = new AktuellesDatumViewModel(); // Hier den neuen Namen verwenden
            // DataContext = new MuskelViewModel(); // Direkt setzen
            this.DataContext = new MuskelViewModel();
           // Debug.WriteLine("DataContext: " + this.DataContext);

            FillComboBox(); // Rufe die Methode beim Initialisieren des Fensters auf
             Muskelgruppen = new ObservableCollection<string>(ZielDict.Keys);
            if (DataContext is MuskelViewModel viewModel)
            {
                Debug.WriteLine("MuskelViewModel wurde korrekt zugewiesen");

                // Die ComboBox ItemsSource binden
               // cmbGruppe.ItemsSource = viewModel.Muskelgruppe;

                // Optional: Kombobox auf das erste Element setzen
                cmbGruppe.SelectedIndex = 0;
            }
            else
            {
                Debug.WriteLine("DataContext konnte nicht zugewiesen werden");
            }
            Debug.WriteLine("MuskelViewModel initialisiert, Gruppen: " + string.Join(", ", Muskelgruppen));

            // Fülle das Dictionary mit den Gruppen und den zugehörigen Übungen
            ZielDict = new Dictionary<string, List<string>>
                {
                    { "Krank", new List<string> { "krank" } },
                    { "Rücken", new List<string> { "Latissimus", "Trapezius", "unterer Rücken" } },
                    { "Beine", new List<string> { "Quadrizeps", "Harnstrings" } },
                    { "Brust", new List<string> { "Obere Brust", "Mittlere Brust" } },
                    { "Bizeps", new List<string> { "gesamter Bizeps" } },
                    { "Trizeps", new List<string> { "gesamter Trizeps" } },
                    { "Schulter", new List<string> { "vordere", "mittlere", "hintere" } },
                    { "Bauch", new List<string> { "oberer", "gesamter", "unterer", "schräger" } },
                    { "Unterarm", new List<string> { "gesamter" } },
                    { "Ganzkörper", new List<string> { "gesamter" } }
                };

            Debug.WriteLine("MuskelViewModel initialisiert, Gruppen: " + string.Join(", ", Muskelgruppen));

            // Combobox auf 0 setzen
            cmbGruppe.SelectedIndex = 0;

            dtgUeber.AutoGeneratingColumn += DtgUeber_AutoGeneratingColumn;
            Daten();
            dtgUeber.ItemsSource = TrainingEntries;
            cmbArt.ItemsSource = new[] { "Definition", "Masseaufbau", "Diät", "Normal" };
            cmbArt.SelectedIndex = 3; // Wählt "Definition" aus
            cmbTechnik.ItemsSource = new[] { "Aufwärmen", "Arbeitssatz", "Cool Down" };
            cmbTechnik.SelectedIndex = 0; // Wählt "Definition" aus
        }
        
        private void FillComboBox()
        {
            var viewModel = (MuskelViewModel)this.DataContext;
            viewModel.Trainingsnummern = new ObservableCollection<string>(); // Initialisiere die ObservableCollection

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Planung WHERE erledigt < 1";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        viewModel.Trainingsnummern.Add(reader["Plan_Nr"].ToString()); // Füge Plan_Nr der ObservableCollection hinzu
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
           //cmbZiel.Items.Clear();

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
                                // **HIER DIE ÄNDERUNG EINFÜGEN**
                                if (decimal.TryParse(dt.Rows[0]["Gewicht"].ToString().Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal letztesGewicht))
                                {
                                    txtletztGewicht.Text = letztesGewicht.ToString("0.00", CultureInfo.InvariantCulture); // Speichere es im invarianten Format
                                }
                                else
                                {
                                    txtletztGewicht.Text = string.Empty; // Fehlerbehandlung, falls das Parsen fehlschlägt
                                }
                                txtLetztWh.Text = dt.Rows[0]["Wh"].ToString(); // Annahme: Wh ist eine ganze Zahl oder wird korrekt formatiert
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
                // ver(); // Der Aufruf von ver() erfolgt im txtGewicht_TextChanged Event
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
               // txtGewicht.Text = "0"; // Direkte Zuweisung als String ist besser in WPF
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
            var viewModel = (MuskelViewModel)this.DataContext;
            decimal i = 0; // Initialisiere i mit einem Standardwert
            decimal o = 0; // Initialisiere o mit einem Standardwert
            decimal e = 0; // Initialisiere e mit einem Standardwert
            decimal f = 0; // Initialisiere f mit einem Standardwert
            decimal p, percentChange;
            decimal u, percentChange1;

            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            // Versuche den aktuellen Gewichtswert zu parsen
            decimal.TryParse(txtGewicht.Text, NumberStyles.Any, invariantCulture, out i);
            bool validI = true; // Da i jetzt initialisiert ist, können wir validI immer als true annehmen (oder eine genauere Prüfung durchführen)

            // Versuche den letzten Gewichtswert zu parsen
            bool validO = !string.IsNullOrWhiteSpace(txtletztGewicht.Text) &&
                          decimal.TryParse(txtletztGewicht.Text, NumberStyles.Any, invariantCulture, out o);
            if (string.IsNullOrWhiteSpace(txtletztGewicht.Text))
            {
                // Handle den Fall, dass kein letztes Gewicht vorhanden ist
                txtVer.Text = "";
                txtVerPro.Text = "";
                // o ist bereits mit 0 initialisiert
            }

            // Versuche die Werte für Wiederholungen zu parsen
            decimal.TryParse(txtWh.Text, NumberStyles.Any, invariantCulture, out e);
            bool validE = true;

            bool validF = !string.IsNullOrWhiteSpace(txtLetztWh.Text) &&
                          decimal.TryParse(txtLetztWh.Text, NumberStyles.Any, invariantCulture, out f);
            if (string.IsNullOrWhiteSpace(txtLetztWh.Text))
            {
                txtWHVer.Text = "";
                txtWhProz.Text = "";
                // f ist bereits mit 0 initialisiert
            }

            if (validI && validO) // Berechne die Veränderung nur, wenn beide Gewichtswerte gültig sind
            {
                if (o != 0)
                {
                   // MessageBox.Show($"i (txtGewicht): {i}, o (txtletztGewicht): {o}");
                    p = i - o;
                    percentChange = (p / o) * 100;
                    viewModel.Veraenderung = p;
                    txtVerPro.Text = percentChange.ToString("0.00") + "%";
                }
                else
                {
                    viewModel.Veraenderung = 0;
                    txtVerPro.Text = "";
                }
            }
            else
            {
                viewModel.Veraenderung = 0;
                txtVerPro.Text = "";
            }

            if (validE && validF) // Berechne die Veränderung der Wiederholungen
            {
                if (f != 0)
                {
                    u = e - f;
                    percentChange1 = (u / f) * 100;
                    txtWHVer.Text = u.ToString("0.00");
                    txtWhProz.Text = percentChange1.ToString("0.00") + "%";
                }
                else
                {
                    txtWHVer.Text = "";
                    txtWhProz.Text = "";
                }
            }
            else
            {
                txtWHVer.Text = "";
                txtWhProz.Text = "";
            }
        }


        private void txtGewicht_TextChanged(object sender, TextChangedEventArgs e)
        {           
           
           // ver();
        }

        private async void txtUebung_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUebung.Text) && txtUebung.Text.Length > 2)
            {
                await LetzteAsync();
            }
        }

        private void cmbGruppe_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Ausgewählte Gruppe: " + ((ComboBox)sender).SelectedItem);
        }

        private void txtGewicht_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ver(); // deine Methode
        }
    }
}