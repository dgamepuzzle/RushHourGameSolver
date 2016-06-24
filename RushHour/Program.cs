using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RushHour
{
    class Program
    {
        static void Main(string[] args)
        {
            //initialize new board of 6x6; mark height of the exit as 3
            RushHour rh = new RushHour(6, 3);
            rh.SolvePuzzles();
        }
    }
}
