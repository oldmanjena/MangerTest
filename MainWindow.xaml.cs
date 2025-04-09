using ManagerTest.Views;
using MangerTest.Anzeigen;
using MangerTest.Blut;
using MangerTest.Essen;
using MangerTest.Training;
using System.Windows;

namespace MangerTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   
    public MainWindow()
    {
        InitializeComponent();
       
        //MainFrame.Navigate(new Startseite()); // Standardseite
        // this.Content = new EinBlutdruck(); // Setze das UserControl als Inhalt des Fensters

    }

    

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        //DrawerHost.IsLeftDrawerOpen = !DrawerHost.IsLeftDrawerOpen; // Menü ein-/ausklappen
    }



    private void Blutdruck_Click(object sender, RoutedEventArgs e)
    {
        ErfassBlut Druck = new ErfassBlut();
        Druck.Show();
    }

    private void Auswertung_Click(object sender, RoutedEventArgs e)
    {
        AuswertungWindow Aus = new AuswertungWindow();
        Aus.Show();
    }

    private void SuBlut_Click(object sender, RoutedEventArgs e)
    {
        MangerTest.Blut.SuBlut SB = new MangerTest.Blut.SuBlut();
        SB.Show();
    }

    private void EinEssen_Click(object sender, RoutedEventArgs e)
    {
        EinEssen EE = new EinEssen();
        EE.Show();
    }

    private void Verschieden_Click(object sender, RoutedEventArgs e)
    {
        GridAnzeigen GA = new GridAnzeigen();
        GA.Show();
    }
      

    private void TraiEin_Click(object sender, RoutedEventArgs e)
    {
        TrainingEin TE = new TrainingEin();
        TE.Show();
    }

    private void Pruef_Click(object sender, RoutedEventArgs e)
    {
        Pruefung PR = new Pruefung();
        PR.Show();
    }

    private void Muskeln_Click(object sender, RoutedEventArgs e)
    {
        MuskelEin MSK = new MuskelEin();
        MSK.Show();
    }
}



