﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotCleaner
{
    public class Robot
    {
        public const int MaxGridPosition = 100_000;
        public const int MinGridPosition = -100_000;
        private const int NumberOfRegionsWide = 2;
        private const int GridSize = (MaxGridPosition - MinGridPosition) / NumberOfRegionsWide;

        private readonly List<Region> _regions = new List<Region>();
        private Region _currentRegion;
        private int _currentLocationX;
        private int _currentLocationY;

        public Robot(int startCoordinateX, int startCoordinateY)
        {
            _currentLocationX = startCoordinateX;
            _currentLocationY = startCoordinateY;

            int offsetX;
            int offsetY;
            for (int x = 0; x < NumberOfRegionsWide; ++x)
            {
                int upperLeftCornerX = MinGridPosition + GridSize * x;
                if (x >= 1)
                {
                    offsetX = 1;
                }
                else
                {
                    offsetX = 0;
                }
                for (int y = 0; y < NumberOfRegionsWide; ++y)
                {
                    
                    if (y >= 1)
                    {
                        offsetY = 1;
                    }
                    else
                    {
                        offsetY = 0;
                    }
                    
                    int upperLeftCornerY = MinGridPosition + GridSize * y;
                    Square square = new Square(upperLeftCornerX+ offsetX, upperLeftCornerY+offsetY, upperLeftCornerX + GridSize, upperLeftCornerY + GridSize);
                    Console.WriteLine($"SQUARE: {square}");
                    Region region = new Region(square, MaxGridPosition);
                    if (square.Contains(startCoordinateX, startCoordinateY))
                    {
                        Console.WriteLine($"SQUARE: {square} is current");
                        _currentRegion = region;
                        region.SetIsStartRegion(true, startCoordinateX, startCoordinateY);
                    }
                    _regions.Add(region);
                }
            }
        }

        public long UniquePositionCount()
        {
            return _regions.Sum(region => region.GetUniqueCount);
        }

        public Tuple<int, int> CurrentLocation => new Tuple<int, int>(_currentLocationX, _currentLocationY);

        private Region GetRegion(int x, int y)
        {
            return _regions.FirstOrDefault(region => region.Square.Contains(x, y));
        }

        private bool SetNext(int newX, int newY)
        {
            Region nextRegion = GetRegion(newX, newY);
            if (nextRegion == null)
            {
                return false;
            }
            _currentRegion = nextRegion;
            return true;
        }

        private int CalculateAllowedSteps(string direction, int steps, out int newX, out int newY)
        {
            switch (direction)
            {
                case "E":
                    newX = _currentRegion.Square.Right + 1;
                    newY = _currentLocationY;
                    return Math.Clamp(_currentRegion.Square.Right - _currentLocationX, 0, steps);
                case "W":
                    newX = _currentRegion.Square.Left - 1;
                    newY = _currentLocationY;
                    return Math.Clamp(_currentLocationX - _currentRegion.Square.Left, 0, steps);
                case "S":
                    newX = _currentLocationX;
                    newY = _currentRegion.Square.Bottom + 1;
                    return Math.Clamp(_currentRegion.Square.Bottom - _currentLocationY, 0, steps);
                case "N":
                    newX = _currentLocationX;
                    newY = _currentRegion.Square.Top - 1;
                    return Math.Clamp(_currentLocationY - _currentRegion.Square.Top, 0, steps);
                default:
                    newX = 0;
                    newY = 0;
                    return 0;
            }
        }

        public void Execute(string direction, int steps)
        {
            int stepsAllowed;
            int newX;
            int newY;

            while(steps > 0)
            {
                Console.WriteLine($"Execute, region is current: {_currentRegion.Square}");
                stepsAllowed = CalculateAllowedSteps(direction, steps, out newX, out newY);

                if (stepsAllowed == 0)
                {
                    if (SetNext(newX, newY))
                    {
                        continue;
                    }

                    return;
                }
                _currentRegion.Execute(direction, stepsAllowed, _currentLocationX, _currentLocationY, out _currentLocationX, out _currentLocationY);
                
                steps -= stepsAllowed;
                
                if (!SetNext(_currentLocationX, _currentLocationY))
                {
                    return;
                }
            }
            
        }
    }
}
