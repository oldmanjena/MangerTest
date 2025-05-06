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
using MangerTest.ViewModel;

namespace MangerTest.Ziel
{
    /// <summary>
    /// Interaktionslogik für ZielEin.xaml
    /// </summary>
    public partial class ZielEin : Window
    {
        public ZielEin()
        {
            InitializeComponent();
           dtpAn.SelectedDate = DateTime.Today;
           dtpEr.SelectedDate = DateTime.Today;
            //  txtZeit.Text = DateTime.Now.ToString("HH:mm");
            DataContext = new ZielEinViewModel();
            txtWertEr.Visibility = Visibility.Hidden;
            lblErfasst.Visibility = Visibility.Hidden;

           
        }

        private void btnEnde_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnErf_Click(object sender, RoutedEventArgs e)
        {
           // txtWertEr.Visibility = Visibility.Visible;
           // lblErfasst.Visibility = Visibility.Visible;
        }

       
    }
}
