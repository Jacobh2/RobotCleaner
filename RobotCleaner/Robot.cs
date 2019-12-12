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

            x = Math.Clamp(x, MIN_POSITION, MAX_POSITION);
            y = Math.Clamp(y, MIN_POSITION, MAX_POSITION);

            //Only loop any intermediate locations if we are not at any wall
            // and are heading in that direction
            if (CurrentLocation.Item1 == MAX_POSITION && dx == 1 || CurrentLocation.Item1 == MIN_POSITION && dx == -1)
            {
                stepsX = 0;
            }

            if (CurrentLocation.Item2 == MAX_POSITION && dy == 1 || CurrentLocation.Item2 == MIN_POSITION && dy == -1)
            {
                stepsY = 0;
            }

            Tuple<int, int> intermediateLocation;

            int intermediateLocationX;
            for (int i = 1; i < stepsX; ++i)
            {
                intermediateLocationX = Math.Clamp(CurrentLocation.Item1 + i * dx, MIN_POSITION, MAX_POSITION);
                intermediateLocation = new Tuple<int, int>(intermediateLocationX, CurrentLocation.Item2);
                UniquePositions.Add(intermediateLocation);
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {

                intermediateLocationY = Math.Clamp(CurrentLocation.Item2 + i * dy, MIN_POSITION, MAX_POSITION);
                intermediateLocation = new Tuple<int, int>(CurrentLocation.Item1, intermediateLocationY);
                UniquePositions.Add(intermediateLocation);
            }

            CurrentLocation = new Tuple<int, int>(x, y);
            UniquePositions.Add(CurrentLocation);
        }

    }
}
