using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RobotCleaner
{
    public class Path
    {
        public readonly int StartLocationX;
        public readonly int StartLocationY;
        public int EndLocationX;
        public int EndLocationY;
        
        public readonly IList<Tuple<string, int>> Actions = new List<Tuple<string, int>>();
        
        public HashSet<long> UniqueLocations = new HashSet<long>();
        public long UniqueLocationsCount = 0;

        public Path(int x, int y, string direction, int steps)
        {
            StartLocationX = x;
            StartLocationY = y;
            AddAction(direction, steps);
        }

        public void AddAction(string direction, int steps)
        {
            Actions.Add(new Tuple<string, int>(direction, steps));
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                Console.WriteLine("obj is null");
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Path p = obj as Path;

            // Return true if the fields match:
            if (StartLocationX != p.StartLocationX || StartLocationY != p.StartLocationY)
            {
                Console.WriteLine("Start loc is not same");
                return false;
            }
            
            bool ret = Actions.SequenceEqual(p.Actions);
            Console.WriteLine($"ACtions are same {ret}");
            return ret;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartLocationX, StartLocationY, EndLocationX, EndLocationY, Actions);
        }
    }
}