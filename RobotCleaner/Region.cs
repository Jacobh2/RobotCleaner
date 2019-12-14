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
        
        public void Execute(string direction, int steps, int locationX, int locationY, out int newLocationX, out int newLocationY)
        {
            bool isOnEdgeX = _isEdgeX && locationX == _maxGridPosition && direction.Equals("E");
            bool isOnEdgeY = _isEdgeY && locationY == _maxGridPosition && direction.Equals("S");
            
            if (isOnEdgeX || isOnEdgeY)
            {
                newLocationX = locationX;
                newLocationY = locationY;
                return;
            }

            _actions.AddLast(new Tuple<int, int, string, int>(locationX, locationY, direction, steps));
            CalculateUniqueCount(out newLocationX, out newLocationY);
        }

        private void Add(HashSet<long> uniqueLocations, int x, int y)
        {
            uniqueLocations.Add(GetHash(x, y));
        }

        private void CalculateUniqueCount(out int newLocationX, out int newLocationY)
        {
            HashSet<long> uniqueLocations = new HashSet<long>();
            
            if (_isStartRegion)
            {
                Add(uniqueLocations,_startPosition.Item1, _startPosition.Item2);                
            }

            newLocationX = 0;
            newLocationY = 0;

            foreach (Tuple<int, int, string, int> action in _actions)
            {    
                CalculateSteps(action.Item1, action.Item2, action.Item3, action.Item4, uniqueLocations, out newLocationX, out newLocationY);
            }
            UniqueCount = uniqueLocations.Count;
        }

        public void CalculateSteps(int currentLocationX, int currentLocationY, string direction, int steps, HashSet<long> uniqueLocations, out int newX, out int newY)
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
                    x += steps;
                    stepsX = x - currentLocationX;
                    break;
                case "W":
                    x -= steps;
                    stepsX = currentLocationX - x;
                    dx = DirectionNegative;
                    break;
                case "S":
                    y += steps;
                    stepsY = y - currentLocationY;
                    break;
                case "N":
                    y -= steps;
                    stepsY = currentLocationY - y;
                    dy = DirectionNegative;
                    break;
            }

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
            newX = x;
            newY = y;
        }

        public override string ToString()
        {
            return $"({Square.Left},{Square.Top})->({Square.Right},{Square.Bottom})";
        }
    }
}