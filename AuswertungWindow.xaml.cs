using MangerTest;
using System.Windows;

namespace ManagerTest.Views  // Der Namespace muss hier genau dem x:Class in der XAML entsprechen!
{
    public partial class AuswertungWindow : Window
    {
        public AuswertungWindow()
        {
            InitializeComponent();  // Sollte funktionieren, wenn alles korrekt ist
            frameAuswertung.Content = new Auswertung();  // Setzt die Page im Frame
        }
    }
}