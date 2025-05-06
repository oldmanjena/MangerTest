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

namespace MangerTest.Planung
{
    /// <summary>
    /// Interaktionslogik für ZieleEin.xaml
    /// </summary>
    public partial class ZieleEin : Window
    {
        public ZieleEin()
        {
            InitializeComponent();
            //dtpHeute.SelectedDate = DateTime.Today;
           // txtZeit.Text = DateTime.Now.ToString("HH:mm");
            DataContext = new ZielEinViewModel();
        }
    }
}
