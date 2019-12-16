using System;
using System.Collections.Generic;
using System.Drawing;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int LineLength(Tuple<Point, Point> line)
        {
            return line.Item1.X == line.Item2.X
                ? Math.Abs(line.Item2.Y - line.Item1.Y) + 1
                : Math.Abs(line.Item2.X - line.Item1.X) + 1;
        }

        public long UniquePositionCount()
        {
            long sum = 0;
            LineLight counted = new LineLight();
            foreach (Line lineA in _lines)
            {
                Console.WriteLine($" \n *** CHECKING LINE {lineA.Id} l={lineA.Length}. sum={sum}*** ");
                Console.WriteLine($"This: {lineA}, new sum={sum + lineA.Length}");
                counted.Clear();
                sum += lineA.Length;
                
                foreach (Line lineB in _lines)
                {
                    // Don't check against ourselves
                    if (lineA.Id == lineB.Id)
                    {
                        continue;
                    }

                    if (lineA.AlreadyChecked(lineB))
                    {
                        continue;
                    }
                    
                    Console.WriteLine($"Comparing against {lineB} - {counted.Count}");
                    
                    lineA.AddOverlappingCoordinates(lineB, counted);
                    Console.WriteLine($"Count: {counted.Count}");
                }
                
                foreach (Tuple<Point, Point> duplicateLine in counted)
                {
                    Console.WriteLine($"Will remove {duplicateLine}: {LineLength(duplicateLine)}");
                    sum -= LineLength(duplicateLine);
                }
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
