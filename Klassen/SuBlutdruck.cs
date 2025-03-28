using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest.Blut

{
    class SuBlutdruck
    {
        public int BltId { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Uhrzeit { get; set; }
        public int Systole { get; set; }
        public int Diastole { get; set; }
        public int Puls { get; set; }
        public string Tageszeit { get; set; }
        public string Bemerkung { get; set; }
        public int PatientID { get; set; }
    }
}
