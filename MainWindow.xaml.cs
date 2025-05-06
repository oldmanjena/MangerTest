using ManagerTest.Views;
using MangerTest.Anzeigen;
using MangerTest.Planung;
using MangerTest.Blut;
using MangerTest.Essen;
using MangerTest.Schmerz;
using MangerTest.Ziel;
using MangerTest.Training;
using System.Windows;
using MangerTest.Auswertungen;
using MangerTest.Gewicht;

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

    private void Training_Click(object sender, RoutedEventArgs e)
    {
        TraiPlanung plan = new TraiPlanung();
        plan.Show();
    }

    private void PlanUpdate_Click(object sender, RoutedEventArgs e)
    {
        TraiPlanUpdate pud = new TraiPlanUpdate();
        pud.Show();
    }

    private void Schmerz_Click(object sender, RoutedEventArgs e)
    {
        schmerzaufzeichnungen schmerz = new schmerzaufzeichnungen();
        schmerz.Show();
    }

    private void ZielEin_Click(object sender, RoutedEventArgs e)
    {
        ZielEin ziel = new ZielEin();
        ziel.Show();
    }

    private void Woche_Click(object sender, RoutedEventArgs e)
    {
        WochenGruppen wochen = new WochenGruppen();
        wochen.Show();
    }

    private void Koerpergewicht_Click(object sender, RoutedEventArgs e)
    {
        KoerperdatenView koerperdaten = new KoerperdatenView();
        koerperdaten.Show();
    }
}



