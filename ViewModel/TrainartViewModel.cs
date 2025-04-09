using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangerTest.ViewModel
{
    public class TrainartViewModel
    {
        public List<string> Trainingsarten { get; set; }

        public TrainartViewModel()
        {
            Trainingsarten = new List<string> { "Diät", "Masseaufbau", "Definition" };
        }
    }
}
