using ManagerTest;
using MangerTest.ViewModel;
using System.Windows;


namespace MangerTest
{
    /// <summary>
    /// Interaktionslogik für Pruefung.xaml
    /// </summary>
    public partial class Pruefung : Window
    {
        private GewichtViewModel _viewModel;
        public Pruefung()
        {
            
            InitializeComponent();
            var vm = new PruefungViewModel();  // Erstelle ViewModel
            this.DataContext = vm;  // Setze den DataContext
            vm.GewichtVM.LadeGewicht();  // Lade das Gewicht           

          
        }

        public void Gewichtdiff()
        {
           decimal a = Convert.ToDecimal(txtGewicht.Text);
           decimal b = Convert.ToDecimal(txtGewZiel.Text);
           decimal g;

           g = a - b;
          txtGewDiff.Text = g.ToString("0.00");
        }

        private void btnErgebnis_Click(object sender, RoutedEventArgs e)
        {
            Gewichtdiff();
        }
    }
}
