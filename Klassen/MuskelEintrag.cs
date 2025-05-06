using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest.Klassen
{
    public class MuskelEintrag
    {
        public DateTime Wann { get; set; } = DateTime.Now;
        public string Muskelgruppe { get; set; }
        public string Uebung { get; set; }
        public string Zielmuskel { get; set; }
        public int Satz { get; set; }
        public int Wiederholungen { get; set; }
        public decimal Gewicht { get; set; }
        public decimal Veraenderung { get; set; }
        public string Art { get; set; }
        public string Technik { get; set; }
        public string Krank { get; set; }
        public int TrainNr { get; set; }
    }
}
