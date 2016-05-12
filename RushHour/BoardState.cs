using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class BoardState
    {
        public BoardState Previous { get; set; }  //łącznik do poprzedniego węzła w grafie
        public List<Car> CarsOnBoard { get; set; }
        public string StepFromPrevious { get; set; }

        public BoardState() { }

        public BoardState(int size)
        {
            CarsOnBoard = new List<Car>();
        }

        public BoardState(int size, BoardState prev)
        {
            CarsOnBoard = new List<Car>();
            Previous = prev;
        }
    }
}
