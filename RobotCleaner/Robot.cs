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
        /// HashSet that holds all unique positions
        /// </summary>
        private readonly HashSet<Tuple<int, int>> UniquePositions = new HashSet<Tuple<int, int>>();

        public Tuple<int, int> CurrentLocation { get; private set; }


        public Robot(int startCoordinateX, int startCoordinateY)
        {
            //Add the start position
            CurrentLocation = new Tuple<int, int>(startCoordinateX, startCoordinateY);
            UniquePositions.Add(CurrentLocation);
        }

        public int GetUniquePositions()
        {
            return UniquePositions.Count;
        }

        public void Execute(string direction, int steps)
        {
            int x = CurrentLocation.Item1;
            int y = CurrentLocation.Item2;
            int stepsX = 0;
            int stepsY = 0;
            int dx = 1;
            int dy = 1;

            switch (direction)
            {
                case "E":
                    x += steps;
                    stepsX = steps;
                    dx = 1;
                    break;
                case "W":
                    x -= steps;
                    stepsX = steps;
                    dx = -1;
                    break;
                case "S":
                    y += steps;
                    stepsY = steps;
                    dy = 1;
                    break;
                case "N":
                    y -= steps;
                    stepsY = steps;
                    dy = -1;
                    break;
            }

            // We check the position to make sure we are within the
            // allowed space given by the exercise (-100k <= pos <= 100k)
            x = Math.Clamp(x, MIN_POSITION, MAX_POSITION);
            y = Math.Clamp(y, MIN_POSITION, MAX_POSITION);

            Tuple<int, int> intermediateLocation;
            for (int i = 1; i < stepsX; ++i)
            {
                intermediateLocation = new Tuple<int, int>(CurrentLocation.Item1 + i * dx, CurrentLocation.Item2);
                UniquePositions.Add(intermediateLocation);
            }
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocation = new Tuple<int, int>(CurrentLocation.Item1, CurrentLocation.Item2 + i * dy);
                UniquePositions.Add(intermediateLocation);
            }

            CurrentLocation = new Tuple<int, int>(x, y);
            UniquePositions.Add(CurrentLocation);
        }

    }
}
