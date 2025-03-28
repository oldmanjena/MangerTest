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
using ManagerTest;
using MangerTest.ViewModel;

namespace MangerTest.Essen
{
    /// <summary>
    /// Interaktionslogik für EinEssen.xaml
    /// </summary>
    public partial class EinEssen : Window
    {
        public GridViewModel GridViewModel { get; set; }
      
        public EinEssen()
        {
            InitializeComponent();
            GridViewModel = new GridViewModel();
            this.DataContext = this;
        }
    }
}
