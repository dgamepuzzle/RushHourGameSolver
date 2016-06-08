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
            RushHour rh = new RushHour(6, 3);
            rh.SolvePuzzles();
            Console.ReadLine();
        }
    }
    
    public class RushHour
    {
        private static int boardSize;
        private static int nextMapInt = 1;
        private static int exitHeight;
        private static Dictionary<string, int> namesMappings;
        private static Dictionary<int, string> toNamesMappings; //display purposes only
        private BoardState firstState;
        private BoardState finishState;
        private Queue<BoardState> allGood;

        public BoardState FirstState //you can get it to check for proper board initialize
        {
            get { return firstState; }
        }

        public RushHour(int size, int exit)
        {
            boardSize = size;
            exitHeight = exit;
            allGood = new Queue<BoardState>();
        }

        public void SolvePuzzles()
        {
            int t = 0;
            string output = "";
            do
            {
                if (!int.TryParse(Console.ReadLine(), out t))
                    Console.WriteLine("Wrong input.");
            } while (t == 0);

            for (int i = 0; i < t; i++)
            {
                int n = 0;
                List<string> cars = new List<string>();
                firstState = new BoardState(boardSize);
                namesMappings = new Dictionary<string, int>();
                toNamesMappings = new Dictionary<int, string>();
                do
                {
                    if (!int.TryParse(Console.ReadLine(), out n))
                        Console.WriteLine("Wrong input.");
                } while (n == 0);
                for (int j = 1; j <= n; j++)
                {
                    cars.Add(Console.ReadLine());
                }
                string score = GetSolutionPath(cars, false); 

                output += score;
                if (i < t - 1)
                    output += "\n";
            }

            Console.Write(output);
        }

        public string GetSolutionPath(List<string> cars, bool displayFirst)
        {
            firstState = new BoardState(boardSize);
            namesMappings = new Dictionary<string, int>();
            toNamesMappings = new Dictionary<int, string>();

            if (!TryFindShortestPath(cars, displayFirst))
                return "No solutions exist!";
            else
            {
                Tuple<int,string> final = GetFinalScore(PrepareOptimal());
                int count = final.Item1;
                string res = final.Item2;
                if (!finishState.StepFromPrevious.Equals("s0"))
                {
                    count++;
                    res += "\n" + UpdateFinishMove();
                }
                return count + "\n" + res;
            }
        }

        private Tuple<int,string> GetFinalScore(List<Tuple<int,string, BoardState>> scores)
        {
            string finalScore = "";
            int min = 10000;
            BoardState finalState = null;
            foreach(var score in scores)
            {
                if(score.Item1 < min)
                {
                    min = score.Item1;
                    finalScore = score.Item2;
                    finalState = score.Item3;
                }
            }

            finishState = finalState;
            return Tuple.Create(min,finalScore);
        }

        private List<Tuple<int,string, BoardState>> PrepareOptimal()
        {
            List<Tuple<int, string, BoardState>> optimized = new List<Tuple<int, string, BoardState>>();
            foreach (var score in allGood)
            {
                Stack<string> result = new Stack<string>();
                string currMove = score.StepFromPrevious;
                BoardState nextState = score.Previous;
                int moveCount = 0;
                
                while(nextState != null)
                {
                    result.Push(currMove);
                    currMove = nextState.StepFromPrevious;
                    nextState = nextState.Previous;
                }

                moveCount = result.Count;
                string aggregated = result.Aggregate((s1, s2) => s1 + "\n" + s2); 
                optimized.Add(Tuple.Create(moveCount, aggregated,score));
            }
            return optimized;
        }

        public string UpdateFinishMove()
        {
            Car car = finishState.CarsOnBoard.FirstOrDefault(c => c.Name == "X");
            if (car != null)
            {
                return "X R " + (boardSize - car.X - car.Size);
            }
            return "Nie ma na tablicy samochodu X!";
        }

        public bool TryFindShortestPath(List<string> cars, bool displayFirst) //BFS algorithm
        {
            GenerateFirstState(cars);

            Queue<BoardState> que = new Queue<BoardState>();
            HashSet<BoardState> set = new HashSet<BoardState>() { firstState };
            que.Enqueue(firstState);

            if (displayFirst)
            {
                Console.WriteLine("First State:");
                DisplayState(firstState);
                Console.WriteLine();
            }

            while (que.Any())
            {
                BoardState next = que.Dequeue();
                if (IsWinningState(next))
                {
                    allGood.Enqueue(next);
                }

                foreach (var succ in GenerateNextNodesGeneration(next))
                {
                    if (!set.Contains(succ))
                    {
                        que.Enqueue(succ);
                        set.Add(succ);
                    }
                    
                }
            }
            if (allGood.Count > 0)
                return true;

            return false;
        }

        public bool IsWinningState(BoardState state)
        {
            Car main = state.CarsOnBoard.FirstOrDefault(c => c.Name == "X");
            if (main != null)
            {
                int[,] startBoard = new int[6, 6];
                FillBoard(startBoard, state.CarsOnBoard);
                if (main.Y == exitHeight)
                {
                    for (int i = main.X + main.Size; i < boardSize; i++)
                    {
                        if (startBoard[boardSize - 1 - exitHeight, i] != 0)
                            return false;
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public Queue<BoardState> GenerateNextNodesGeneration(BoardState oldBs)
        {
            int[,] startBoard = new int[boardSize, boardSize];
            FillBoard(startBoard, oldBs.CarsOnBoard);
            Queue<BoardState> helperQueue = new Queue<BoardState>();

            foreach (var car in oldBs.CarsOnBoard)
            {
                int i = 1;
                bool stillCorrect = false;

                switch (car.Position)
                {
                    case "H":
                        do
                        {
                            if (car.X - i >= 0 && isMovePossible(i,car,"L",startBoard)) //try move left
                            {
                                stillCorrect = true;
                                BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "L", i++);
                                helperQueue.Enqueue(nextGenState);
                            }
                            else
                            {
                                stillCorrect = false;
                                i = 1;
                            }
                        } while (stillCorrect);
                        do
                        {
                            if (car.X + car.Size -1 + i < boardSize && isMovePossible(i, car, "R", startBoard)) //try move right
                            {
                                stillCorrect = true;
                                BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "R",i++);
                                helperQueue.Enqueue(nextGenState);
                            }
                            else
                            {
                                stillCorrect = false;
                                i = 1;
                            }
                        } while (stillCorrect);
                        break;

                    case "V":
                        do
                        {
                            if (car.Y + car.Size - 1 + i < boardSize && isMovePossible(i, car, "U", startBoard)) //try move up
                            {
                                stillCorrect = true;
                                BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "U", i++);
                                helperQueue.Enqueue(nextGenState);
                            }
                            else
                            {
                                stillCorrect = false;
                                i = 1;
                            }
                        } while (stillCorrect);
                        do
                        {
                            if (car.Y - i >= 0 && isMovePossible(i, car, "D", startBoard)) //try move down
                            {
                                stillCorrect = true;
                                BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "D", i++);
                                helperQueue.Enqueue(nextGenState);
                            }
                            else
                            {
                                stillCorrect = false;
                                i = 1;
                            }
                        } while (stillCorrect);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            return helperQueue;
        }

        private bool isMovePossible(int max, Car car, string mode, int[,] board)
        {
                switch (mode)
                {
                    case "L":
                        if (board[boardSize - 1 - car.Y, car.X - max] != 0)
                            return false;
                        break;
                    case "R":
                        if (board[boardSize - 1 - car.Y, car.X + car.Size - 1 + max] != 0)
                            return false;
                        break;
                    case "U":
                        if (board[boardSize - 1 - car.Y - (car.Size - 1 + max), car.X] != 0)
                            return false;
                        break;
                    case "D":
                        if (board[boardSize - 1 - car.Y + max, car.X] != 0)
                            return false;
                        break;
                    default:
                        throw new Exception("Wrong argument.");
                }
            return true;
        }

        private BoardState GenerateNewNode(BoardState oldBs, string carToExclude, string mode, int val)
        {
            BoardState nextNode = new BoardState(boardSize, oldBs);
            List<Car> carList = new List<Car>();
            carList.AddRange(CopyCarsByValue(oldBs.CarsOnBoard, carToExclude, mode, val));
            nextNode.CarsOnBoard = carList;
            nextNode.StepFromPrevious = carToExclude + " " + mode + " " + val;
            return nextNode;
        }

        private List<Car> CopyCarsByValue(List<Car> carsToCopy, string carToExclude, string mode, int val)
        {
            List<Car> resultCars = new List<Car>();
            foreach (var car in carsToCopy)
            {
                switch (mode)
                {
                    case "L":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X - val, car.Y, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "R":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X + val, car.Y, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "U":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X, car.Y + val, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "D":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X, car.Y - val, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                }
            }
            return resultCars;
        }

        public void GenerateFirstState(List<string> cars)
        {
            foreach (var car in cars)
            {
                var matches = Regex.Matches(car, @"\w+");
                namesMappings.Add(matches[0].Value, nextMapInt);
                toNamesMappings.Add(nextMapInt++, matches[0].Value);
                Car tempCar = new Car()
                {
                    Name = matches[0].Value,
                    X = Convert.ToInt32(matches[1].Value),
                    Y = Convert.ToInt32(matches[2].Value),
                    Position = matches[3].Value,
                    Size = Convert.ToInt32(matches[4].Value)
                };
                firstState.CarsOnBoard.Add(tempCar);
            }
            firstState.StepFromPrevious = "s0";
        }

        private static void FillBoard(int[,] board, List<Car> cars)
        {
            foreach (var car in cars)
            {
                FillBoard(board, car);
            }
        }

        private static void FillBoard(int[,] board, Car car)
        {
            int offset;
            int cMapped = MapCarNameToInt(car.Name);
            switch (car.Position)
            {
                case "H":
                    offset = car.X;
                    for (int i = 0; i < car.Size; i++)
                    {
                        board[boardSize - 1 - car.Y, offset++] = cMapped;
                    }
                    break;
                case "V":
                    offset = boardSize - 1 - car.Y;
                    for (int i = 0; i < car.Size; i++)
                    {
                        board[offset--, car.X] = cMapped;
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private static int MapCarNameToInt(string carName)
        {
            int retVal = 0;
            if (namesMappings.TryGetValue(carName, out retVal))
                return retVal;
            else
                throw new InvalidOperationException();
        }

        public static void DisplayState(BoardState state)
        {
            int[,] table = new int[boardSize, boardSize];
            FillBoard(table, state.CarsOnBoard);
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (table[i, j] < 10) //TODO: add support for any size
                        Console.Write(table[i, j] + "   ");
                    else
                        Console.Write(table[i, j] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Legend:");
            foreach (var item in toNamesMappings)
            {
                Console.WriteLine("{0} - {1}", item.Key, item.Value);
            }
            Console.WriteLine();
        }
    }

    public static class StringExtensions
    {
        //removing whitespaces from the string
        public static string ExceptBlanks(this string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                switch (c)
                {
                    case '\r':
                    case '\n':
                    case '\t':
                    case ' ':
                        continue;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        public static string ReplaceAt(this string input, int index, int newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            StringBuilder builder = new StringBuilder(input);
            builder[index] = newChar.ToString()[0];
            return builder.ToString();
        }
    }

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

    public class BoardState
    {
        public BoardState Previous { get; set; }  //łącznik do poprzedniego węzła w grafie
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

        //debugging
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var car in CarsOnBoard)
            {
                sb.Append("Name: " + car.Name + " , Pos: "+ car.Position + ", Size: "+car.Size + ", X: "+car.X + ", Y: "+car.Y);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
