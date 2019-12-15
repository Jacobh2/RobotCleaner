using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RobotCleaner
{
    public class Robot
    {
        public const int MaxGridPosition = 100_000;
        public const int MinGridPosition = -100_000;
        private const int DirectionPositive = 1;
        private const int DirectionNegative = -1;

        private readonly List<Line> _lines = new List<Line>();
        private int _currentLocationX;
        private int _currentLocationY;
        private short _nextId = 0;

        public Robot(int startCoordinateX, int startCoordinateY)
        {
            _currentLocationX = startCoordinateX;
            _currentLocationY = startCoordinateY;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetHash(long x, long y)
        {
            return (x+100_000)*1_000_000 + y + 100_000;
        }

        public long UniquePositionCount()
        {
            long sum = 0;
            HashSet<long> counted = new HashSet<long>();
            foreach (Line lineA in _lines)
            {
                sum += lineA.Length;
                counted.Clear();
                
                foreach (Line lineB in _lines)
                {
                    // Don't check against ourselves
                    if (lineA.Id == lineB.Id)
                    {
                        continue;
                    }
                    
                    if (lineA.Intersect(lineB, out int x, out int y))
                    {
                        counted.Add(GetHash(x, y));
                    }
                    else
                    {
                        lineA.AddOverlappingCoordinates(lineB, counted);
                    }
                }
                sum -= counted.Count;
            }
            return sum;
        }

        public Tuple<int, int> CurrentLocation => new Tuple<int, int>(_currentLocationX, _currentLocationY);

        public void Execute(string direction, int steps)
        {
            int newPositionX = _currentLocationX;
            int newPositionY = _currentLocationY;
            int dx = DirectionPositive;
            int dy = DirectionPositive;
            switch (direction)
            {
                case "E":
                    newPositionX = Math.Clamp(_currentLocationX + steps, MinGridPosition, MaxGridPosition);
                    steps = newPositionX - _currentLocationX;
                    break;
                case "W":
                    newPositionX = Math.Clamp(_currentLocationX - steps, MinGridPosition, MaxGridPosition);
                    steps = _currentLocationX - newPositionX;
                    dx = DirectionNegative;
                    break;
                case "S":
                    newPositionY = Math.Clamp(_currentLocationY + steps, MinGridPosition, MaxGridPosition);
                    steps = newPositionY - _currentLocationY;
                    break;
                case "N":
                    newPositionY = Math.Clamp(_currentLocationY - steps, MinGridPosition, MaxGridPosition);
                    steps = _currentLocationY - newPositionY;
                    dy = DirectionNegative;
                    break;
            }

            if (steps == 0)
            {
                return;
            }

            _lines.Add(new Line(_currentLocationX, _currentLocationY, newPositionX, newPositionY, dx, dy, _nextId++));
            _currentLocationX = newPositionX;;
            _currentLocationY = newPositionY;
        }
    }
}
