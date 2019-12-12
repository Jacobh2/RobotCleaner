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

        private readonly HashSet<long> _uniquePositions = new HashSet<long>();
        private int _currentLocationX;
        private int _currentLocationY;

        public Robot(int startCoordinateX, int startCoordinateY)
        {
            _currentLocationX = startCoordinateX;
            _currentLocationY = startCoordinateY;
            _uniquePositions.Add(GetHash(_currentLocationX, _currentLocationY));
        }

        public int UniquePositionCount => _uniquePositions.Count;

        public Tuple<int, int> CurrentLocation => new Tuple<int, int>(_currentLocationX, _currentLocationY);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetHash(long x, long y)
        {
            return (x+100_000)*1_000_000 + y + 100_000;
        }

        public void Execute(string direction, int steps)
        {
            int x = _currentLocationX;
            int y = _currentLocationY;
            int dx = DirectionPositive;
            int dy = DirectionPositive;
            int stepsX = 0;
            int stepsY = 0;

            switch (direction)
            {
                case "E":
                    x = Math.Clamp(_currentLocationX + steps, MinGridPosition, MaxGridPosition);
                    stepsX = x - _currentLocationX;
                    break;
                case "W":
                    x = Math.Clamp(_currentLocationX - steps, MinGridPosition, MaxGridPosition);
                    stepsX = _currentLocationX - x;
                    dx = DirectionNegative;
                    break;
                case "S":
                    y = Math.Clamp(_currentLocationY + steps, MinGridPosition, MaxGridPosition);
                    stepsY = y - _currentLocationY;
                    break;
                case "N":
                    y = Math.Clamp(_currentLocationY - steps, MinGridPosition, MaxGridPosition);
                    stepsY = _currentLocationY - y;
                    dy = DirectionNegative;
                    break;
            }

            int intermediateLocationX;
            for (int i = 1; i < stepsX; ++i)
            {
                intermediateLocationX = _currentLocationX + i * dx;
                _uniquePositions.Add(GetHash(intermediateLocationX, _currentLocationY));
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocationY = _currentLocationY + i * dy;
                _uniquePositions.Add(GetHash(_currentLocationX, intermediateLocationY));
            }

            _uniquePositions.Add(GetHash(x, y));
            _currentLocationX = x;
            _currentLocationY = y;
        }
    }
}
