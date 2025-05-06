using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest.Klassen
{
    public class WochenEintrag
    {
        public DateTime Wann { get; set; }

        public string Wochentag => Wann.ToString("dddd", new CultureInfo("de-DE"));

        public string Muskelgruppe { get; set; }
        public string Uebung { get; set; }
        public int Satz { get; set; }
        public int Wh { get; set; }

        public string FormatierteZeile => $"{Uebung} - {Satz} Sätze, {Wh} Wh";
    }
}
