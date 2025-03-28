using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest.ViewModel
{
    public class PruefungViewModel
        {
            public GewichtViewModel GewichtVM { get; set; }
            public GewichtZielViewModel GewichtZielVM { get; set; }

            public PruefungViewModel()
            {
                GewichtVM = new GewichtViewModel();
                GewichtZielVM = new GewichtZielViewModel();
            }
        }
    
}
