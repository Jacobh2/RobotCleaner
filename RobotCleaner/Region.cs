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
        
        public HashSet<long> UniqueLocations = new HashSet<long>();
        public long UniqueLocationsCount;
        private bool _addStartPosition = false;

        public Region(Square square, int maxGridPosition)
        {
            Square = square;
            _isEdgeX = square.Right > maxGridPosition;
            _isEdgeY = square.Bottom > maxGridPosition;
            _maxGridPosition = maxGridPosition;
        }

        public long GetUniqueCount => UniqueLocationsCount + (_addStartPosition ? 1 : 0);

        public void SetIsStartRegion(int x, int y)
        {
            _isStartRegion = true;
            _startPosition = new Tuple<int, int>(x, y);
            _addStartPosition = true;
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
            
            if (_paths.Last != null && locationX == _paths.Last.Value.EndLocationX && locationY == _paths.Last.Value.EndLocationY)
            {
                //Console.WriteLine($">> Continuing path from {locationX}, {locationY}");
                _paths.Last.Value.AddAction(direction, steps);
            }
            else
            {
                //Console.WriteLine($">> Might want to add {direction}{steps} from ({locationX}, {locationY})");
                Path newPath = new Path(locationX, locationY, direction, steps);
                if (!_paths.Contains(newPath))
                {
                    //Console.WriteLine($">> New path from {locationX}, {locationY}");
                    //New path!
                    _paths.AddLast(newPath);    
                }
                else
                {
                    newLocationX = locationX;
                    newLocationY = locationY;
                    return;
                }

            }
            CalculateUniqueCount(out newLocationX, out newLocationY);
        }

        private void CalculateUniqueCount(out int newLocationX, out int newLocationY)
        {
            UniqueLocations.Clear();
            newLocationX = 0;
            newLocationY = 0;

            foreach (Path path in _paths)
            {
                newLocationX = path.StartLocationX;
                newLocationY = path.StartLocationY;
            
                foreach (Tuple<string, int> action in path.Actions)
                {    
                    //Console.WriteLine($"Adding action {action} from ({newLocationX},{newLocationY})");
                    CalculateSteps(newLocationX, newLocationY, action.Item1, action.Item2, UniqueLocations, out newLocationX, out newLocationY);
                    path.EndLocationX = newLocationX;
                    path.EndLocationY = newLocationY;
                    //Console.WriteLine($"New location: ({newLocationX},{newLocationY}), UniqueLocations:{UniqueLocations.Count}");
                }

            }
            
            UniqueLocationsCount = UniqueLocations.LongCount();
            UniqueLocations.Clear();
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
                AddUniqueLocation(uniqueLocations, intermediateLocationX, currentLocationY);
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocationY = currentLocationY + i * dy;
                AddUniqueLocation(uniqueLocations, currentLocationX, intermediateLocationY);
            }

            AddUniqueLocation(uniqueLocations, x, y);
            newX = x;
            newY = y;
        }

        private void AddUniqueLocation(HashSet<long> set, int x, int y)
        {
            //Console.WriteLine($"Maybe adding {x}, {y}");
            if (_isStartRegion && _startPosition.Item1 == x && _startPosition.Item2 == y)
            {
                _addStartPosition = false;
                return;
            }
            set.Add(GetHash(x, y));
        }
    }
}