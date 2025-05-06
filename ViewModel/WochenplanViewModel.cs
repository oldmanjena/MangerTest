using MangerTest.Klassen;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using MigraDoc.DocumentObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;

namespace MangerTest.ViewModel
{
    public class WochenplanViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WochenplanZeile> Wochenplan { get; set; }
        public ObservableCollection<string> Muskelgruppen { get; set; } = new();
        public ICommand ExportierePdfCommand { get; }

        private string _ausgewaehlteMuskelgruppe;
        public string AusgewaehlteMuskelgruppe
        {
            get => _ausgewaehlteMuskelgruppe;
            set
            {
                if (_ausgewaehlteMuskelgruppe != value)
                {
                    _ausgewaehlteMuskelgruppe = value;
                    OnPropertyChanged();
                    LadeWochenDaten(); // reload bei Änderung
                }
            }
        }



        public WochenplanViewModel()
        {
            Wochenplan = new ObservableCollection<WochenplanZeile>();
            try
            {
                LadeMuskelgruppen();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Muskelgruppen: " + ex.Message);
            }
            ExportierePdfCommand = new RelayCommand(ExportiereWochenplanAlsPdf);

            LadeMuskelgruppen();
        }

        private void LadeMuskelgruppen()
        {
            string conString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;
            string cmdString = "SELECT DISTINCT Muskelgruppe FROM Muskel";

            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(cmdString, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Muskelgruppen.Add(reader.GetString(0));
                    }
                }
            }

            // Optional: erste Auswahl automatisch setzen
            if (Muskelgruppen.Count > 0)
                AusgewaehlteMuskelgruppe = Muskelgruppen[0];
            else
                return;
        }

        public void LadeWochenDaten()
        {
            if (Wochenplan == null)
                Wochenplan = new ObservableCollection<WochenplanZeile>();
            else
                Wochenplan.Clear();

            if (string.IsNullOrEmpty(AusgewaehlteMuskelgruppe))
                return;

            List<MangerTest.Klassen.WochenEintrag> daten = new();

            string conString = ConfigurationManager.ConnectionStrings["managment"].ConnectionString;
            string cmdString = "SELECT Wann, Muskelgruppe, Uebung, Satz, Wh FROM Muskel WHERE Muskelgruppe = @gruppe AND Wann >= @start AND Wann <= @ende";

            var heute = DateTime.Today;
            var start = heute.AddDays(-(int)heute.DayOfWeek + (int)DayOfWeek.Monday);
            var ende = start.AddDays(6);

            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(cmdString, con))
            {
                cmd.Parameters.AddWithValue("@gruppe", AusgewaehlteMuskelgruppe);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@ende", ende);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        daten.Add(new WochenEintrag
                        {
                            Wann = reader.GetDateTime(0),
                            Muskelgruppe = reader.GetString(1),
                            Uebung = reader.GetString(2),
                            Satz = reader.GetInt32(3),
                            Wh = reader.GetInt32(4),
                        });
                    }
                }
            }

            var gruppiert = daten
                  .GroupBy(d => d.Wochentag)
                  .ToDictionary(g => g.Key, g => g.Select(x => x.FormatierteZeile).ToList());

            var maxCount = gruppiert.Values.Any() ? gruppiert.Values.Max(l => l.Count) : 0;

            for (int i = 0; i < maxCount; i++)
            {
                var zeile = new WochenplanZeile
                {
                    Montag = gruppiert.ContainsKey("Montag") && gruppiert["Montag"].Count > i ? gruppiert["Montag"][i] : "",
                    Dienstag = gruppiert.ContainsKey("Dienstag") && gruppiert["Dienstag"].Count > i ? gruppiert["Dienstag"][i] : "",
                    Mittwoch = gruppiert.ContainsKey("Mittwoch") && gruppiert["Mittwoch"].Count > i ? gruppiert["Mittwoch"][i] : "",
                    Donnerstag = gruppiert.ContainsKey("Donnerstag") && gruppiert["Donnerstag"].Count > i ? gruppiert["Donnerstag"][i] : "",
                    Freitag = gruppiert.ContainsKey("Freitag") && gruppiert["Freitag"].Count > i ? gruppiert["Freitag"][i] : "",
                    Samstag = gruppiert.ContainsKey("Samstag") && gruppiert["Samstag"].Count > i ? gruppiert["Samstag"][i] : "",
                    Sonntag = gruppiert.ContainsKey("Sonntag") && gruppiert["Sonntag"].Count > i ? gruppiert["Sonntag"][i] : "",
                };
                Wochenplan.Add(zeile);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ExportiereWochenplanAlsPdf()
        {
            // Datum der Woche
            var heute = DateTime.Today;
            var montag = heute.AddDays(-(int)heute.DayOfWeek + (int)DayOfWeek.Monday);
            var sonntag = montag.AddDays(6);

            var doc = new Document();
            var section = doc.AddSection();
            section.PageSetup.Orientation = Orientation.Landscape;

            // Überschrift
            var title = section.AddParagraph($"Wochenplan ({montag:dd.MM.yyyy} – {sonntag:dd.MM.yyyy})", "Heading1");
            title.Format.SpaceAfter = "1cm";
            title.Format.Alignment = ParagraphAlignment.Center;

            // Tabelle erzeugen
            var table = new Table();
            table.Borders.Width = 0.75;

            string[] tage = { "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag", "Sonntag" };

            foreach (var tag in tage)
            {
                var column = table.AddColumn(Unit.FromCentimeter(3.5));
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            var header = table.AddRow();
            for (int i = 0; i < tage.Length; i++)
            {
                header.Cells[i].AddParagraph(tage[i]);
                header.Cells[i].Format.Font.Bold = true;
                header.Cells[i].Shading.Color = Colors.LightGray;
                header.Cells[i].Format.Alignment = ParagraphAlignment.Center;
            }

            foreach (var zeile in Wochenplan)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(zeile.Montag ?? "");
                row.Cells[1].AddParagraph(zeile.Dienstag ?? "");
                row.Cells[2].AddParagraph(zeile.Mittwoch ?? "");
                row.Cells[3].AddParagraph(zeile.Donnerstag ?? "");
                row.Cells[4].AddParagraph(zeile.Freitag ?? "");
                row.Cells[5].AddParagraph(zeile.Samstag ?? "");
                row.Cells[6].AddParagraph(zeile.Sonntag ?? "");
            }

            // Tabelle in zentrierten Absatz einfügen
            // Tabelle zentriert hinzufügen
            table.Format.Alignment = ParagraphAlignment.Center;
            section.Add(table);


            // PDF exportieren
            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string dateiname = $"Wochenplan_{montag:yyyyMMdd}_{sonntag:yyyyMMdd}.pdf";
            string pfad = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dateiname);

            renderer.Save(pfad);
            Process.Start("explorer.exe", pfad);
        }






        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            private readonly Func<bool>? _canExecute;

            public RelayCommand(Action execute, Func<bool>? canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
            public void Execute(object? parameter) => _execute();
        }
    }
}
