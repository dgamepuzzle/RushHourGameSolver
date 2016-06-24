using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class Car
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public string Position { get; set; }

        public Car() { }
        public Car(string name, int x, int y, string pos, int size)
        {
            Name = name;
            X = x;
            Y = y;
            Size = size;
            Position = pos;
        }
    }
}
