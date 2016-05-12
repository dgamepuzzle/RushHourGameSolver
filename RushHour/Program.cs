using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    class Program
    {
        static void Main(string[] args)
        {
            //samples sets
            List<string> A = new List<string> //6x6
            {
                "X 0 3 H 2",
                "A 4 1 H 2",
                "C 4 2 V 3",
            };
            List<string> B = new List<string> //6x6
            {
                "X 3 3 H 2",
                "A 0 3 V 3",
                "B 1 5 H 2",
                "C 1 3 V 2",
                "D 2 3 V 2",
                "E 4 4 V 2",
                "F 2 0 V 2",
                "G 3 1 V 2",
                "H 5 3 V 3",
                "I 0 0 H 2",
                "J 0 2 H 3",
                "K 3 0 H 2",
                "L 4 1 H 2"
            };
            List<string> C = new List<string> //6x6
            {
                "X 1 3 H 2",
                "A 2 0 H 2",
                "B 3 2 V 2"
            };
            List<string> D = new List<string> //4x4
            {
                "X 0 2 H 2",
                "A 0 0 V 2",
                "B 1 0 H 3",
                "C 3 1 V 2"
            };
            List<string> E = new List<string> //4x4
            {
                "X 1 2 H 2",
                "A 0 1 V 2",
                "B 0 0 H 3",
                "C 3 1 V 2"
            };

            List<string> A1 = new List<string> //6x6
            {
                "X 3 3 H 2",
                "A 0 5 H 2",
                "B 3 5 H 2",
                "C 4 1 H 2",
                "D 0 2 V 3",
                "E 2 3 V 3",
                "F 3 1 V 2",
                "G 5 2 V 2",
                "H 5 4 V 2",
                "I 1 2 H 2"
            };

            List<string> A2 = new List<string> //6x6
            {
                "X 0 3 H 2",
                "P 1 0 V 2",
                "n 2 1 H 2",
                "f 2 0 H 2",
                "F 4 0 V 3",
                "N 5 0 V 3",
                "Z 2 3 V 3",
                "ż 3 3 V 3",
            };

            List<string> A3 = new List<string> //6x6
            {
                "X 0 3 H 2",
                "Z 0 2 H 2",
                "P 1 0 V 2",
                "N 3 0 H 3",
                "Ż 3 1 V 3",
                "F 5 1 V 3"
            };

            List<string> A4 = new List<string> //6x6
            {
                "X 0 3 H 2",
                "Z 0 0 V 2",
                "P 1 0 V 2",
                "n 2 1 H 2",
                "f 4 0 V 2",
                "N 5 0 V 3",
                "Ż 2 3 V 3",
                "F 3 3 V 3"
            };

            //RushHour rh = new RushHour(4,2); //dla 4x4
            RushHour rh = new RushHour(6, 3); // dla6x6

            Stack<string> path = rh.GetSolutionPath(A);
            if (path != null)
            {
                Console.WriteLine("Koniec");
                foreach (var move in path)
                {
                    Console.WriteLine(move);
                }
            }
            else
            {
                Console.WriteLine("Still something wrong.");
            }

            Console.ReadLine();
            }
        }
    }
