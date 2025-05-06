using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;
using MangerTest.Klassen;
using MangerTest.Converter;
using System.Data;

namespace MangerTest.ViewModel
{
    public class ZielEinViewModel
    {
        private DateTime _datumAn = DateTime.Today;
        private DateTime _datumEr = DateTime.Today;
        public ICommand EintragCommand { get; }
        public DateTime DatumAn
        {
            get => _datumAn;
            set
            {
                _datumAn = value;
                OnPropertyChanged();
            }
        }

        public DateTime DatumEr
        {
            get => _datumEr;
            set
            {
                _datumEr = value;
                OnPropertyChanged();
            }
        }
        //erledigt
        private string _einheit;
        public string Einheit
        {
            get => _einheit;
            set
            {
                _einheit = value;
                OnPropertyChanged(nameof(_einheit));
            }
        }

        public ZielEinViewModel()
        {
            EintragCommand = new RelayCommand(Speichern);
           
        }
        private string _richtung;
        public string Richtung
        {
            get => _richtung;
            set
            {
                _richtung = value;
                OnPropertyChanged(nameof(_richtung));
            }
        }

        private string _was;
        public string Was
        {
            get => _was;
            set
            {
                _was = value;
                OnPropertyChanged(nameof(_was));
            }
        }

        private string _notiz;      
        public string Notiz
        {
            get => _notiz;
            set
            {
                _notiz = value;
                OnPropertyChanged(nameof(_notiz));
            }
        }

        private decimal _zielwert;
        public decimal Zielwert
        {
            get => _zielwert;
            set
            {
                _zielwert = value;
                OnPropertyChanged(nameof(_zielwert));
            }
        }
      

        private decimal _erfasst;
        public decimal Erfasst
        {
            get => _erfasst;
            set
            {
                _erfasst = value;
                OnPropertyChanged(nameof(_erfasst));
            }
        }

        private decimal _veraenderung;
        public decimal Veraenderung
        {
            get => _veraenderung;
            set
            {
                _veraenderung = value;
                OnPropertyChanged(nameof(_veraenderung));
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        private void Speichern(object parameter)
        {
            try
            {


                using (SqlConnection con = new SqlConnection("data source=DESKTOP-726MH0T;initial catalog=managment;trusted_connection=true"))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = @"
             INSERT INTO Ziele
                 (DatumAn, Was, Einheit, Zielwert, DatumEr, Richtung, wert, Notiz)
             VALUES
                 (@datumAn, @was, @einheit, @ziel, @datumer, @richtung, @wert,  @notiz)";

                        cmd.Parameters.AddWithValue("@datumAn", DatumAn);
                        cmd.Parameters.AddWithValue("@was", Was);
                        cmd.Parameters.AddWithValue("@einheit", Einheit);
                        cmd.Parameters.AddWithValue("@ziel", Zielwert);
                        cmd.Parameters.AddWithValue("@datumer", DatumEr);
                        cmd.Parameters.AddWithValue("@richtung", Richtung);
                        cmd.Parameters.AddWithValue("@wert", Veraenderung);
                        cmd.Parameters.AddWithValue("@notiz", Notiz);

                        cmd.ExecuteNonQuery();
                    }
                }

                System.Windows.MessageBox.Show("Eintrag erfolgreich gespeichert.");
            }
            catch (SqlException ex)
            {
                System.Windows.MessageBox.Show($"Fehler beim Speichern: {ex.Message}");
            }
        }

    }
}
