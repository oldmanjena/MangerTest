using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MangerTest.ViewModel
{
    public class GridViewModel : INotifyPropertyChanged
    {
        private UIElement _cell10;
        private UIElement _cell13;
        private UIElement _cell11;
        private UIElement _cell02;
        public UIElement Cell10
        {
            get { return _cell10; }
            set { _cell10 = value; OnPropertyChanged(); }
        }

        // Ähnliche Eigenschaften für Cell01, Cell02, ..., Cell33
       
        public UIElement Cell11
        {
            get { return _cell11; }
            set { _cell11 = value; OnPropertyChanged(); }
            
        }

        public UIElement Cell02
        {
            get { return _cell02; }
            set { _cell02 = value; OnPropertyChanged(); }

        }

        public UIElement Cell13
        {
            get { return _cell13; }
            set { _cell13 = value; OnPropertyChanged(); }

        }

        //... alle weiteren Cell Eigenschaften

        public GridViewModel()
        {
            // Beispielinhalte für die Zellen
            Cell13 = new Button { Content = "Eintragen" };
            Cell11 = new TextBox { Text = "Text 01" };
            Cell10 = new Label { Content = "Label 12" };
            Cell02 = new DatePicker { SelectedDate = DateTime.Now };
            // ... weitere Inhalte nach Bedarf
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
