using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly bool _isEdgeX;
        private readonly bool _isEdgeY;
        
        private readonly LinkedList<Path> _paths = new LinkedList<Path>();
        private bool _isStartRegion;
        private Tuple<int, int> _startPosition;

        public Region(Square square, int maxGridPosition)
        {
            Square = square;
            _isEdgeX = square.Right > maxGridPosition;
            _isEdgeY = square.Bottom > maxGridPosition;
            _maxGridPosition = maxGridPosition;
        }

        public long GetUniqueCount => _paths.Sum(x => x.UniqueLocationsCount);

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
            //We need to check if this is a new path or not.
            if (_paths.Last != null && locationX == _paths.Last.Value.EndLocationX && locationY == _paths.Last.Value.EndLocationY)
            {
                // Same path!
                Console.WriteLine("This is a continuation on the previous path!");
                _paths.Last.Value.AddAction(direction, steps);
            }
            else
            {
                // Only add this path if it already does not exist
                Path newPath = new Path(locationX, locationY, direction, steps);
                Console.WriteLine($"New path {newPath} already exist: {_paths.Contains(newPath)}");
                if (!_paths.Contains(newPath))
                {
                    Console.WriteLine("This is a new path");
                    //New path!
                    _paths.AddLast(newPath);    
                }
                else
                {
                    Console.WriteLine($"THIS PATH ALREADY EXIST!!");
                }
                
            }
            Console.WriteLine($"_paths.Last for {Square.Top} END:{_paths.Last != null}");
            
            CalculateUniqueCount(out newLocationX, out newLocationY);
        }

        private void CalculateUniqueCount(out int newLocationX, out int newLocationY)
        {
            // We will only re-calculate the last path
            Path lastPath = _paths.Last.Value;
            lastPath.UniqueLocations.Clear();
            
            if (_isStartRegion)
            {
                Console.WriteLine("Adding startposition!");
                lastPath.UniqueLocations.Add(GetHash(_startPosition.Item1, _startPosition.Item2));
            }

            newLocationX = lastPath.StartLocationX;
            newLocationY = lastPath.StartLocationY;
            
            foreach (Tuple<string, int> action in lastPath.Actions)
            {    
                Console.WriteLine($"Adding action {action} from ({newLocationX},{newLocationY})");
                CalculateSteps(newLocationX, newLocationY, action.Item1, action.Item2, lastPath.UniqueLocations, out newLocationX, out newLocationY);
                lastPath.EndLocationX = newLocationX;
                lastPath.EndLocationY = newLocationY;
                Console.WriteLine($"New location: ({newLocationX},{newLocationY})");
            }

            lastPath.UniqueLocationsCount = lastPath.UniqueLocations.LongCount();
        }

        private void CalculateSteps(int currentLocationX, int currentLocationY, string direction, int steps, HashSet<long> uniqueLocations, out int newX, out int newY)
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
                uniqueLocations.Add(GetHash(intermediateLocationX, currentLocationY));
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocationY = currentLocationY + i * dy;
                uniqueLocations.Add(GetHash(currentLocationX, intermediateLocationY));
            }

            uniqueLocations.Add(GetHash(x, y));
            newX = x;
            newY = y;
        }
    }
}