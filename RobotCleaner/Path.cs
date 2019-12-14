using System;
using System.Collections.Generic;
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
    }
}