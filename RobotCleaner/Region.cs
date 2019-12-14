using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using QuadTrees.QTreeRect;

namespace RobotCleaner
{
    public class Region
    {
        private const int DirectionPositive = 1;
        private const int DirectionNegative = -1;
        private readonly int _maxGridPosition;
        
        public Square Square { get; }

        private bool _isEdgeX;
        private bool _isEdgeY;
        // Holds start position (x, y), direction and steps for each action
        private readonly LinkedList<Tuple<int, int, string, int>> _actions = new LinkedList<Tuple<int, int, string, int>>();
        public int UniqueCount { get; private set; }
        private bool _isStartRegion = false;
        private Tuple<int, int> _startPosition;

        public Region(Square square, int maxGridPosition)
        {
            Square = square;
            _isEdgeX = square.Right > maxGridPosition;
            _isEdgeY = square.Bottom > maxGridPosition;
            _maxGridPosition = maxGridPosition;
        }

        public int GetUniqueCount => UniqueCount;

        public void SetIsStartRegion(bool value, int x, int y)
        {
            _isStartRegion = value;
            _startPosition = new Tuple<int, int>(x, y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetHash(long x, long y)
        {
            return (x+100_000)*1_000_000 + y + 100_000;
        }
        
        public Tuple<int, int> Execute(string direction, int steps, int locationX, int locationY)
        {
            //Don't do anything if we are at the edge
            if (_isEdgeX && locationX == _maxGridPosition && direction.Equals("E"))
            {
                return new Tuple<int, int>(locationX, locationY);
            }

            if (_isEdgeY && locationY == _maxGridPosition && direction.Equals("S"))
            {
                return new Tuple<int, int>(locationX, locationY);
            }
            //Add new action
            Console.WriteLine($"In a region: Want to go {direction}{steps} from {locationX}, {locationY}");
            _actions.AddLast(new Tuple<int, int, string, int>(locationX, locationY, direction, steps));
            //Update unique count and current location
            var temp = CalculateUniqueCount();
            Console.WriteLine($"This yielded {UniqueCount} unique positions with new location {temp}");
            return temp;
        }

        private void Add(HashSet<long> uniqueLocations, int x, int y)
        {
            Console.WriteLine($"Adding ({x},{y}). Now have {uniqueLocations.Count}");
            uniqueLocations.Add(GetHash(x, y));
            Console.WriteLine($"After have {uniqueLocations.Count}");
        }

        private Tuple<int, int> CalculateUniqueCount()
        {
            HashSet<long> uniqueLocations = new HashSet<long>();
            
            Console.WriteLine($"This region is start region: {_isStartRegion}");
            if (_isStartRegion)
            {
                Add(uniqueLocations,_startPosition.Item1, _startPosition.Item2);                
            }
            
            Tuple<int, int> currentLocation = null;
            foreach (Tuple<int, int, string, int> action in _actions)
            {    
                currentLocation = CalculateSteps(action.Item1, action.Item2, action.Item3, action.Item4, uniqueLocations);
            }
            UniqueCount = uniqueLocations.Count;
            return currentLocation;
        }

        public Tuple<int, int> CalculateSteps(int currentLocationX, int currentLocationY, string direction, int steps, HashSet<long> uniqueLocations)
        {
            int x = currentLocationX;
            int y = currentLocationY;
            int dx = DirectionPositive;
            int dy = DirectionPositive;
            int stepsX = 0;
            int stepsY = 0;

            switch (direction)
            {
                case "E":
                    Console.WriteLine($"Will go E {steps} steps");
                    x += steps;
                    stepsX = x - currentLocationX;
                    break;
                case "W":
                    Console.WriteLine($"Will go W {steps} steps");
                    x -= steps;
                    stepsX = currentLocationX - x;
                    dx = DirectionNegative;
                    break;
                case "S":
                    Console.WriteLine($"Will go S {steps} steps");
                    y += steps;
                    stepsY = y - currentLocationY;
                    break;
                case "N":
                    Console.WriteLine($"Will go N {steps} steps");
                    y -= steps;
                    stepsY = currentLocationY - y;
                    dy = DirectionNegative;
                    break;
            }
            Console.WriteLine($"In Region will go stepX {stepsX}, stepY {stepsY}. New x:{x}, new y:{y} from {currentLocationX}, {currentLocationY}");

            int intermediateLocationX;
            for (int i = 1; i < stepsX; ++i)
            {
                intermediateLocationX = currentLocationX + i * dx;
                Add(uniqueLocations, intermediateLocationX, currentLocationY);
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocationY = currentLocationY + i * dy;
                Add(uniqueLocations,currentLocationX, intermediateLocationY);
            }

            Add(uniqueLocations,x, y);
            return new Tuple<int, int>(x,y);
        }

        public override string ToString()
        {
            return $"({Square.Left},{Square.Top})->({Square.Right},{Square.Bottom})";
        }
    }
}