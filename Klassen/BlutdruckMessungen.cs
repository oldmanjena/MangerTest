using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest
{
    public class BlutdruckMessungen
    {
        public int BltId { get; set; }         // blt_id
        public DateTime Datum { get; set; }    // Datum
        public DateTime Uhrzeit { get; set; }  // Uhrzeit
        public int Systole { get; set; }       // Systole
        public int Diastole { get; set; }      // Diastole
        public int Puls { get; set; }          // Puls
        public string Tageszeit { get; set; }  // Tageszeit
        public string Bemerkung { get; set; }  // Bemerkung
        public int PatientId { get; set; }     // PatientID
    }
}
