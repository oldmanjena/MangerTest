﻿using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using ManagerTest;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MangerTest.Anzeigen
{
    /// <summary>
    /// Interaktionslogik für GridAnzeigen.xaml
    /// </summary>
    public partial class GridAnzeigen : Window
    {
       

        private string CmdString = "";
        private readonly string connectionString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true;";

        private readonly string a = "SELECT DATEPART(Day, Datum) AS Tag, " +
                                     "SUM(Entfernung) AS KM, " +
                                     "SUM(kcal) AS Kcal, " +
                                     "CAST(FORMAT(SUM(DATEPART(HOUR, Dauer)) + " +
                                     "SUM(DATEPART(MINUTE, Dauer)) / 60.0 + " +
                                     "SUM(DATEPART(SECOND, Dauer)) / 3600.0, 'N2', 'en-US') " +
                                     "AS DECIMAL(10, 2)) AS ZeitInStunden " +
                                     "FROM Training " +
                                     "WHERE YEAR(Datum) = YEAR(GETDATE()) AND MONTH(Datum) = MONTH(GETDATE()) " +  // Filter für das aktuelle Jahr
                                     "GROUP BY DATEPART(Day, Datum)" +
                                     "ORDER BY Tag DESC;";
        private readonly string b = "SELECT DATEPART(ISO_WEEK, Datum) AS Woche, CAST(AVG(Gewicht) AS DECIMAL(10, 2)) AS Diffenenz, CAST(AVG(Körperfett) AS DECIMAL(10, 2)) AS Körperfett FROM Fitdays WHERE YEAR(Datum) = YEAR(GETDATE())  GROUP BY DATEPART(ISO_WEEK, Datum) ORDER BY Woche";
        private readonly string c = "SELECT DATEPART(ISO_WEEK, Datum) AS Woche, " +
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


        public GridAnzeigen()
        {
            InitializeComponent();

            AnzeigenGrid.ItemsSource = new List<string> { "Summe Pro Tag", "Abfrage 2", "Summe Woche" };
           
        }


             private void FillDataGrid()

             {

                    string ConString = "data source=DESKTOP-726MH0T;initial catalog=gesundheit;trusted_connection=true";

                    string CmdString = string.Empty;

                    using (SqlConnection con = new SqlConnection(ConString))
                    {

                        CmdString = "select Wann, Muskel, Uebung, training, RM, Veraenderung from Max ORDER BY  RM_id DESC ";

                        SqlCommand cmd = new SqlCommand(CmdString, con);

                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable("maximal");

                        sda.Fill(dt);
                        //dtgGewicht.AutoGeneratingColumn += Dtg_Gewicht_AutoGeneratedColumns;
                        DatenAnzeigen.ItemsSource = dt.DefaultView;

                    }
             }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void AnzeigenGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AnzeigenGrid.SelectedItem != null)
            {
                // Abfrage basierend auf der Auswahl setzen
                switch (AnzeigenGrid.SelectedIndex)
                {
                    case 0: CmdString = a; break;
                    case 1: CmdString = b; break;
                    case 2: CmdString = c; break;
                }

                // Abfrage ausführen
                ExecuteQuery(CmdString);
            }
        }

        private void ExecuteQuery(string query)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Daten ausgeben (z. B. in einer DataGrid-Komponente)
                        DatenAnzeigen.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Fehler: " + ex.Message);
            }
        }
    }
}
