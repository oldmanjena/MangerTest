using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MangerTest.user
{
    /// <summary>
    /// Interaktionslogik für usrZeiterf.xaml
    /// </summary>
    public partial class usrZeiterf : UserControl
    {
        private DispatcherTimer _dispatcherTimer; // Eindeutiger Variablenname
        private DateTime _startZeit;


        private DispatcherTimer timer;

        public usrZeiterf()
        {
            InitializeComponent();
            LadeAktuelleZeit();
            StarteTimer();
            

           

        }



        private void StarteTimer()
        {
            _dispatcherTimer = new DispatcherTimer(); // Richtige Instanz erstellen
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1); // Jede Sekunde aktualisieren
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            LadeAktuelleZeit();
        }

        private void LadeAktuelleZeit()
        {
            txtZeit.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _startZeit = DateTime.Now;
            txtZeit.Text = _startZeit.ToString("HH:mm:ss"); // Startzeit in Textbox setzen
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            DateTime endZeit = DateTime.Now;
            TimeSpan differenz = endZeit - _startZeit;
            txtEnde.Text = differenz.ToString(@"hh\:mm\:ss"); // Zeitdifferenz anzeigen

         //   MessageBox.Show("Stopp-Knopf wurde gedrückt!");
         //   MessageBox.Show("Startzeit: " + _startZeit.ToString("HH:mm:ss"));
         //   MessageBox.Show("Endzeit: " + endZeit.ToString("HH:mm:ss"));
        //    MessageBox.Show("Differenz: " + differenz.ToString(@"hh\:mm\:ss"));
        }
    }
    
}
