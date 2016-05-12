using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHour
{
    public class RushHour
    {
        /*input będzie postaci
        1           (ilość test case'ów)
        3           (ilosć samochodów do wczytania)
        X 0 3 H 2   (auto X, od punktu 0,0 -> 0j. w poziomie, 3j. w pionie, stan:poziomy, długość:2)
        A 4 1 H 2   (auto A, od punktu 0,0 -> 1j. w poziomie, 1j. w pionie, stan:poziomy, długość:2)
        C 4 2 V 3   (auto C, od punktu 0,0 -> 2j. w poziomie, 2j. w pionie, stan:pionowy, długość:3)
        */

        /* Istotne założenia:
        - plansza będzie przedstawiona jako tablica char'ów,
        - punkt (0,0) jest w lewym dolnym rogu, punkt (5,5) w prawym górnym rogu
        */

        private int boardSize;
        private int nextMapInt = 1;
        private int exitHeight;
        private BoardState firstState;
        private BoardState finishState;
        private Dictionary<string, int> namesMappings;

        //for debugging purposes
        public BoardState FirstState
        {
            get { return firstState; }
        }

        public BoardState LasState
        {
            get { return LasState; }
        }

        public RushHour(int size, int exit)
        {
            boardSize = size;
            firstState = new BoardState(size);
            namesMappings = new Dictionary<string, int>();
            exitHeight = exit;
        }

        public Stack<string> GetSolutionPath(List<string> cars)
        {
            if (!TryFindShortestPath(cars))
                return null;
            else
            {
                Stack<string> result = new Stack<string>();
                if(!finishState.StepFromPrevious.Equals("s0"))
                    result.Push(UpdateFinishMove());
                string currMove = finishState.StepFromPrevious;
                BoardState nextState = finishState.Previous;
                int sum = 1;
                string nextMove = "";

                while (nextState != null)
                {
                    nextMove = nextState.StepFromPrevious;
                    if (nextMove.Equals(currMove))
                    {
                        sum++;
                        nextState = nextState.Previous;
                    }
                    else
                    {
                        if (sum > 1)
                        {
                            string toAdd = StringExtensions.ReplaceAt(currMove, 4, sum);
                            result.Push(toAdd);
                            currMove = nextState.StepFromPrevious;
                            nextState = nextState.Previous;
                            sum = 1;
                        }
                        else
                        {
                            result.Push(currMove);
                            currMove = nextState.StepFromPrevious;
                            nextState = nextState.Previous;
                        }
                    }
                }
                return result;
            }
        }

        public string UpdateFinishMove()
        {
            Car car = finishState.CarsOnBoard.FirstOrDefault(c => c.Name == "X");
            if(car != null)
            {
                return "X R " + (boardSize - car.X - car.Size);
            }
            return "Nie ma na tablicy samochodu X!";
        }

        public bool TryFindShortestPath(List<string> cars) //BFS algorithm
        {
            GenerateFirstState(cars);
            if (IsWinningState(firstState))
            {
                finishState = firstState;
                return true;
            };
            Queue<BoardState> que = new Queue<BoardState>();
            que.Enqueue(firstState);
            HashSet<BoardState> set = new HashSet<BoardState>(new BoardStateByCar()) { firstState };
                Console.WriteLine("Początek:");
                DisplayState(firstState);
                Console.WriteLine();
            while (que.Any())
            {
                BoardState next = que.Dequeue();
                foreach(var succ in GenerateNextNodesGeneration(next))
                {
                    if (IsWinningState(succ))
                    {
                        finishState = succ;
                        return true;
                    }
                    if (!set.Contains(succ))
                    {
                        que.Enqueue(succ);
                        set.Add(succ);
                    }
                }
            }
            return false;
        }

        public bool IsWinningState(BoardState state)
        {
            Car main = state.CarsOnBoard.FirstOrDefault(c => c.Name == "X");
            if (main.Y == exitHeight)
            {
                if (main != null)
                {
                    for (int i = main.X + main.Size; i < boardSize; i++)
                    {
                        if (state.State[boardSize - 1 - exitHeight, i] != 0)
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

        //jakis bug
        public Queue<BoardState> GenerateNextNodesGeneration(BoardState oldBs)
        {
            int[,] startBoard = oldBs.State;
            Queue<BoardState> helperQueue = new Queue<BoardState>();
            foreach (var car in oldBs.CarsOnBoard)
            {
                switch (car.Position)
                {
                    case "H":
                        if(car.X - 1 >= 0 && startBoard[boardSize - 1 - car.Y, car.X - 1] == 0) //try move left
                        {
                            BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "L");
                                helperQueue.Enqueue(nextGenState); //add new node to temporary list of nodes
                        }
                        if (car.X + car.Size < boardSize && startBoard[boardSize - 1 - car.Y, car.X + car.Size] == 0) //try move right
                        {
                            BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "R");
                                helperQueue.Enqueue(nextGenState);
                        }
                        break;
                    case "V":
                        if (car.Y + car.Size < boardSize && startBoard[boardSize - 1 - car.Y - car.Size, car.X] == 0) //try move up
                        {
                            BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "U");
                                helperQueue.Enqueue(nextGenState);
                        }
                        if (car.Y - 1 >= 0 && startBoard[boardSize - 1 - car.Y + 1, car.X] == 0) //try move down
                        {
                            BoardState nextGenState = GenerateNewNode(oldBs, car.Name, "D");
                                helperQueue.Enqueue(nextGenState);
                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
            return helperQueue;
        }

        private BoardState GenerateNewNode(BoardState oldBs, string carToExclude, string mode)
        {
            BoardState nextNode = new BoardState(boardSize, oldBs);
            List<Car> carList = new List<Car>();
            int[,] updatedBoard = new int[boardSize, boardSize];
            carList.AddRange(CopyCarsByValue(oldBs.CarsOnBoard, carToExclude, mode));
            FillBoard(updatedBoard, carList);
            nextNode.CarsOnBoard = carList;
            nextNode.State = updatedBoard;
            nextNode.StepFromPrevious = carToExclude + " "+ mode+ " " + 1;
            nextNode.DistanceToInitial = oldBs.DistanceToInitial + 1;
            return nextNode;
        }

        private List<Car> CopyCarsByValue(List<Car> carsToCopy, string carToExclude, string mode)
        {
            List<Car> resultCars = new List<Car>();
            foreach (var car in carsToCopy)
            {
                switch (mode)
                {
                    case "L":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X - 1, car.Y, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "R":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X + 1, car.Y, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "U":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X, car.Y + 1, car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                    case "D":
                        if (car.Name == carToExclude)
                            resultCars.Add(new Car(car.Name, car.X, car.Y - 1 , car.Position, car.Size));
                        else
                            resultCars.Add(new Car(car.Name, car.X, car.Y, car.Position, car.Size));
                        break;
                }
            }
            return resultCars;
        }

        public void GenerateFirstState(List<string> cars)
        {
            int[,] board = new int[boardSize, boardSize];
            foreach(var car in cars)
            {
                char[] divided = car.ExceptBlanks().ToCharArray(); // [X,0,3,H,2]
                string nameToMap = divided[0].ToString();
                namesMappings.Add(nameToMap, nextMapInt++); //change string to int so that operation was performed on ints
                Car tempCar = new Car(nameToMap,(int)char.GetNumericValue(divided[1]),
                    (int)char.GetNumericValue(divided[2]), divided[3].ToString(),(int)char.GetNumericValue(divided[4])); 
                FillBoard(board, tempCar); //update board
                firstState.CarsOnBoard.Add(tempCar);
            }
            firstState.State = board;
            firstState.StepFromPrevious = "s0";
            firstState.DistanceToInitial = 0;
        }

        private void FillBoard(int [,] board, List<Car> cars)
        {
            foreach(var car in cars)
            {
                FillBoard(board, car);
            }
        }

        private void FillBoard(int[,] board, Car car)
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

        private int MapCarNameToInt(string carName)
        {
            int retVal = 0;
            if (namesMappings.TryGetValue(carName, out retVal))
                return retVal;
            else
                throw new InvalidOperationException();
        }

        public static void DisplayState(BoardState state)
        {
            for (int i = 0; i < state.State.GetLength(0); i++)
            {
                for (int j = 0; j < state.State.GetLength(1); j++)
                {
                    if(state.State[i,j]<10)
                        Console.Write(state.State[i, j]+"   ");
                    else
                        Console.Write(state.State[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }
    }
}
