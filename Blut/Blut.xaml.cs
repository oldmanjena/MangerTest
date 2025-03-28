using MangerTest.todo;
using MangerTest.user;
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

namespace MangerTest
{
    /// <summary>
    /// Interaktionslogik für Blutdruck.xaml
    /// </summary>
    public partial class Blutdruck : Window
    {
        public Blutdruck()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AddTab("Blutdruck", new EinBlutdruck());
            AddTab("Todo", new TodoListControl());
            AddTab("Zeit", new usrZeiterf());

        }

        private void AddTab(string header, UserControl content)
        {
            // Prüfen, ob ein Tab mit diesem Header bereits existiert
            foreach (TabItem item in tabControl.Items)
            {
                if (item.Header.ToString() == header)
                {
                    tabControl.SelectedItem = item; // Falls es existiert, Tab aktivieren
                    return;
                }
            }

            // Neues Tab erstellen, weil es noch nicht existiert
            TabItem newTab = new TabItem();
            newTab.Header = header;
            newTab.Content = content;

            tabControl.Items.Add(newTab);
            tabControl.SelectedItem = newTab; // Direkt das neue Tab auswählen
        }
    }
}
