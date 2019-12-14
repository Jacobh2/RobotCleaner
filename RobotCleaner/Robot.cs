using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using QuadTrees;

namespace RobotCleaner
{
    public class Robot
    {
        public const int MaxGridPosition = 100_000;
        public const int MinGridPosition = -100_000;
        private const int NumberOfRegionsWide = 20;
        private const int GridSize = (MaxGridPosition - MinGridPosition) / NumberOfRegionsWide;

        private readonly List<Region> _regions = new List<Region>();
        private Region _currentRegion;
        private int _currentLocationX;
        private int _currentLocationY;

        public Robot(int startCoordinateX, int startCoordinateY)
        {
            _currentLocationX = startCoordinateX;
            _currentLocationY = startCoordinateY;

            int offsetX = 0;
            int offsetY = 0;
            for (int x = 0; x < NumberOfRegionsWide; ++x)
            {
                int upperLeftCornerX = MinGridPosition + GridSize * x;
                if (x >= 1)
                {
                    Console.WriteLine("LAST X");
                    offsetX = 1;
                }
                else
                {
                    Console.WriteLine("NOT LAST X");
                    offsetX = 0;
                }
                for (int y = 0; y < NumberOfRegionsWide; ++y)
                {
                    
                    if (y >= 1)
                    {
                        Console.WriteLine("LAST Y");
                        offsetY = 1;
                    }
                    else
                    {
                        Console.WriteLine("NOT LAST Y");
                        offsetY = 0;
                    }
                    
                    int upperLeftCornerY = MinGridPosition + GridSize * y;
                    Square square = new Square(upperLeftCornerX+ offsetX, upperLeftCornerY+offsetY, upperLeftCornerX + GridSize, upperLeftCornerY + GridSize);
                    Region region = new Region(square, MaxGridPosition);
                    if (square.Contains(startCoordinateX, startCoordinateY))
                    {
                        _currentRegion = region;
                        region.SetIsStartRegion(true, startCoordinateX, startCoordinateY);
                    }
                    Console.WriteLine($"Rec: {square} , {square.Left}, {square.Top}, {square.Right}, {square.Bottom} has start coord: {square.Contains(startCoordinateX,startCoordinateY)}");
                    _regions.Add(region);
                }
            }
            Console.WriteLine($"_currentRegion: {_currentRegion.Square}");
            
        }

        public int UniquePositionCount()
        {
            int total = 0;
            foreach (var region in _regions)
            {
                Console.WriteLine($"Region {region} unique: {region.GetUniqueCount}");
                total += region.GetUniqueCount;
            }

            return total;
        }

        public Tuple<int, int> CurrentLocation => new Tuple<int, int>(_currentLocationX, _currentLocationY);

        private Region GetRegion(int x, int y)
        {
            Console.WriteLine($"Looking for {x}, {y}");
            foreach (Region region in _regions)
            {
                if (region.Square.Contains(x, y))
                {
                    Console.WriteLine($"And found {region.Square}");
                    return region;
                }
            }
            Console.WriteLine($"And found NOTHING!!!");
            return null;
        }

        private bool SetNext(int newX, int newY)
        {
            //Can we go right? If which case, get that region
            Region nextRegion = GetRegion(newX, newY);
            if (nextRegion == null)
            {
                return false;
            }
            _currentRegion = nextRegion;
            return true;
        }

        public void Execute(string direction, int steps)
        {
            Console.WriteLine($"********* Want to execute {direction}{steps} *********");
            Console.WriteLine($"Current location {_currentLocationX}, {_currentLocationY}");
            // Given a direction and steps, split it!
            int stepsAllowed = 0;
            int newX = 0, newY = 0;
            switch (direction)
            {
                case "E":
                    // Calculate max in the current regions direction
                    stepsAllowed = Math.Clamp(_currentRegion.Square.Right - _currentLocationX, 0, steps);
                    newX = _currentRegion.Square.Right + 1;
                    newY = _currentLocationY;
                    break;
                case "W":
                    stepsAllowed = Math.Clamp(_currentLocationX - _currentRegion.Square.Left, 0, steps);
                    newX = _currentRegion.Square.Left - 1;
                    newY = _currentLocationY;
                    break;
                case "S":
                    stepsAllowed = Math.Clamp(_currentRegion.Square.Bottom - _currentLocationY, 0, steps);
                    newX = _currentLocationX;
                    newY = _currentRegion.Square.Bottom + 1;
                    break;
                case "N":
                    Console.WriteLine($"Current Y {_currentLocationY}, region top: {_currentRegion.Square.Top}");
                    stepsAllowed = Math.Clamp(_currentLocationY - _currentRegion.Square.Top, 0, steps);
                    newX = _currentLocationX;
                    newY = _currentRegion.Square.Top - 1;
                    break;
            }
            // If stepsAllowed is 0 here, that might mean that we need to go to the next region
            
            Console.WriteLine($"Allowed to go {stepsAllowed} steps in this region");
            if (stepsAllowed == 0)
            {
                
                Console.WriteLine($"Newx {newX}, newY {newY}");
                if (SetNext(newX, newY))
                {
                    //_currentLocationX = newX;
                    //_currentLocationY = newY;
                    Execute(direction, steps);
                    return;
                }

                return;
            }
            

            // add the action
            Tuple<int, int> newLocation = _currentRegion.Execute(direction, stepsAllowed, _currentLocationX, _currentLocationY);
            
            Console.WriteLine($"newLocation: {newLocation}");
            _currentLocationX = newLocation.Item1;
            _currentLocationY = newLocation.Item2;
            Console.WriteLine($"Affects the next rect: {steps - stepsAllowed > 0}");

            if (steps - stepsAllowed > 0)
            {
                Console.WriteLine($"We affect the next block too! current location {_currentLocationX}, {_currentLocationY}");
                if (!SetNext(_currentLocationX, _currentLocationY))
                {
                    return;
                }
                newLocation = _currentRegion.Execute(direction, steps - stepsAllowed, _currentLocationX, _currentLocationY);
                _currentLocationX = newLocation.Item1;
                _currentLocationY = newLocation.Item2;
                SetNext(_currentLocationX, _currentLocationY);
            }
        }
    }
}
