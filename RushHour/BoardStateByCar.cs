using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class BoardStateByCar : IEqualityComparer<BoardState>
    {
        public bool Equals(BoardState x, BoardState y)
        {
            if (x.CarsOnBoard.Count != y.CarsOnBoard.Count)
                return false;
            for (int i = 0; i < x.CarsOnBoard.Count; i++)
            {
                Car one = x.CarsOnBoard[i]; Car two = y.CarsOnBoard[i];
                if (one.Name != two.Name || one.Position != two.Position || one.Size != two.Size
                    || one.X != two.X || one.Y != two.Y)
                    return false;
            }
            return true;
        }

        public int GetHashCode(BoardState obj)
        {
            int hash = 17;
            foreach (var car in obj.CarsOnBoard)
            {
                int spCount = car.Name.ToCharArray().Sum(chr => Convert.ToInt32(chr));
                int posCount = car.Position.ToCharArray().Sum(chr => Convert.ToInt32(chr));
                hash = hash + car.X + car.Y + spCount + posCount + car.Size;
            }
            return hash;
        }
    }
}
