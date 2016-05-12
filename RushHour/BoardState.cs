using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class BoardState
    {
        public int[,] State { get; set; }
        public BoardState Previous { get; set; }  //łącznik do poprzedniego węzła w grafie
        public List<Car> CarsOnBoard { get; set; }
        public string StepFromPrevious { get; set; }
        public int DistanceToInitial { get; set; }


        public BoardState() { }

        public BoardState(int size)
        {
            State = new int[size, size];
            CarsOnBoard = new List<Car>();
        }

        public BoardState(int size, BoardState prev)
        {
            State = new int[size, size];
            CarsOnBoard = new List<Car>();
            Previous = prev;
        }

        //public bool Equals(BoardState other)
        //{
        //    if (State.GetLength(0) != other.State.GetLength(0) || State.GetLength(1) != other.State.GetLength(1)
        //        || CarsOnBoard.Count != other.CarsOnBoard.Count)
        //        return false;
        //    for (int i = 0; i < other.CarsOnBoard.Count; i++)
        //    {
        //        Car one = other.CarsOnBoard[0]; Car two = CarsOnBoard[i];
        //        if (one.Name != two.Name || one.Position != two.Position || one.Size != two.Size
        //            || one.X != two.X || one.Y != two.Y)
        //            return false;
        //    }
        //    return true;
        //}

        //public override bool Equals(object obj)
        //{
        //    return Equals((BoardState)obj);
        //}

        //public override int GetHashCode()
        //{
        //    int hash = 17;
        //    foreach (var car in CarsOnBoard)
        //    {
        //        int spCount = car.Name.ToCharArray().Sum(chr => Convert.ToInt32(chr));
        //        int posCount = car.Position.ToCharArray().Sum(chr => Convert.ToInt32(chr));
        //        hash = hash + car.X + car.Y + spCount + posCount + car.Size;
        //    }
        //    return hash;
        //}
    }
}
