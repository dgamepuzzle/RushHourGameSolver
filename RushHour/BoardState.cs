using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class BoardState
    {
        public BoardState Previous { get; set; }  //pointer to the previous node
        public List<Car> CarsOnBoard { get; set; }
        public string StepFromPrevious { get; set; }
        private int size;

        public BoardState() { }

        public BoardState(int size)
        {
            CarsOnBoard = new List<Car>();
            this.size = size;
        }

        public BoardState(int size, BoardState prev)
        {
            CarsOnBoard = new List<Car>();
            Previous = prev;
            this.size = size;
        }

        public override bool Equals(object obj)
        {
            BoardState other = obj as BoardState;
            if (other == null)
                return false;

            Car[] others = other.CarsOnBoard.ToArray();
            for (int i = 0; i < others.Count(); i++)
            {
                if (CarsOnBoard[i].Name != others[i].Name ||
                   CarsOnBoard[i].Position != others[i].Position ||
                   CarsOnBoard[i].Size != others[i].Size ||
                   CarsOnBoard[i].X != others[i].X ||
                   CarsOnBoard[i].Y != others[i].Y)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            int mul = 1;
            foreach (Car c in CarsOnBoard)
            {
                hash += (c.X + c.Y * size) * mul;
                mul += size * size;
            }
            return hash;
        }

        //for debugging purposes
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var car in CarsOnBoard)
            {
                sb.Append("Name: " + car.Name + " , Pos: " + car.Position + ", Size: " + car.Size + ", X: " + car.X + ", Y: " + car.Y);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
