using System;
using System.Collections.Generic;

namespace RobotCleaner
{
    public class Robot
    {
        /// <summary>
        /// The maximum allowed position (for both X and Y)
        /// </summary>
        public const int MAX_POSITION = 100000;

        /// <summary>
        /// The minimum allowed position (for both X and Y)
        /// </summary>
        public const int MIN_POSITION = -100000;

        /// <summary>
        /// Linked list of steps taken by the robot
        /// </summary>
        private readonly LinkedList<Tuple<int, int>> Path = new LinkedList<Tuple<int, int>>();


        public Robot(int startCoordinateX, int startCoordinateY)
        {
            //Add the start position
            var startLocation = new Tuple<int, int>(startCoordinateX, startCoordinateY);
            Path.AddLast(startLocation);
        }

        public Tuple<int, int> CurrentPosition => Path.Last.Value;

        public int GetUniquePositions()
        {

            HashSet<Tuple<int, int>> uniqueLocations = new HashSet<Tuple<int, int>>();

            Tuple<int, int> lastLocation = Path.First.Value;
            //The start position is unique
            uniqueLocations.Add(lastLocation);

            int steps;
            int multiplier;
            Tuple<int, int> intermediateLocation;

            foreach (Tuple<int, int> location in Path)
            {
                if (location.Equals(lastLocation))
                {
                    continue;
                }

                //Check if it was the X value that was changed
                if (location.Item1 != lastLocation.Item1)
                {
                    multiplier = lastLocation.Item1 > location.Item1 ? 1 : -1;
                    steps = Math.Abs(lastLocation.Item1 - location.Item1);
                    for (int i = 0; i < steps; ++i)
                    {
                        intermediateLocation = new Tuple<int, int>(location.Item1 + i * multiplier, location.Item2);
                        uniqueLocations.Add(intermediateLocation);
                    }
                }
                else
                {
                    multiplier = lastLocation.Item1 > location.Item1 ? 1 : -1;
                    steps = Math.Abs(lastLocation.Item2 - location.Item2);
                    for (int i = 0; i < steps; ++i)
                    {
                        intermediateLocation = new Tuple<int, int>(location.Item1, location.Item2 + i * multiplier);
                        uniqueLocations.Add(intermediateLocation);
                    }
                }
                lastLocation = location;
            }

            return uniqueLocations.Count;
        }

        private int CheckPosition(int pos)
        {
            if (pos > MAX_POSITION)
            {
                return MAX_POSITION;
            }
            else if (pos < MIN_POSITION)
            {
                return MIN_POSITION;
            }
            return pos;
        }


        public void Execute(string direction, int steps)
        {
            Tuple<int, int> currentLocation = Path.Last.Value;
            int x = currentLocation.Item1;
            int y = currentLocation.Item2;

            switch (direction)
            {
                case "E":
                    x += steps;
                    break;
                case "W":
                    x -= steps;
                    break;
                case "S":
                    y += steps;
                    break;
                case "N":
                    y -= steps;
                    break;
            }

            x = CheckPosition(x);
            y = CheckPosition(y);

            Path.AddLast(new Tuple<int, int>(x, y));
        }

    }
}
