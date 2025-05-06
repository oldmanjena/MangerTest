using ManagerTest;
using MangerTest.ViewModel;
using System;
using System.Collections.Generic;
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

namespace MangerTest.Schmerz
{
    /// <summary>
    /// Interaktionslogik für schmerzaufzeichnungen.xaml
    /// </summary>
    public partial class schmerzaufzeichnungen : Window
    {
        public schmerzaufzeichnungen()
        {
            InitializeComponent();
            dtpHeute.SelectedDate = DateTime.Today;
            txtZeit.Text = DateTime.Now.ToString("HH:mm");
            DataContext = new SchmerzAufViewModel();

            this.Loaded += (s, e) =>
            {
                rtxSchmerz.Document = (this.DataContext as SchmerzAufViewModel)?.SchmerzenBeschreibung;
            };
        }

       


        private void btnEnde_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chkMedi_Checked(object sender, RoutedEventArgs e)
        {
            pnMedikamente.Visibility = chkMedi.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            var viewModel = (SchmerzAufViewModel)this.DataContext;
            viewModel.MediEingenommen = chkMedi.IsChecked == true;
        }

        private void chkDetail_Checked(object sender, RoutedEventArgs e)
        {
            pnDetail.Visibility = chkDetail.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }
        
    }
}

