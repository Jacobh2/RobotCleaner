using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotCleaner
{
    public class Robot
    {
        public const int MAX_GRID_POSITION = 100000;
        public const int MIN_GRID_POSITION = -100000;
        public const int DIRECTION_POSITIVE = 1;
        public const int DIRECTION_NEGATIVE = -1;

        private readonly HashSet<string> UniquePositions = new HashSet<string>();
        private int CurrentLocationX;
        private int CurrentLocationY;


        public Robot(int startCoordinateX, int startCoordinateY)
        {
            CurrentLocationX = startCoordinateX;
            CurrentLocationY = startCoordinateY;
            UniquePositions.Add(startCoordinateX + "," + startCoordinateY);
        }

        public int UniquePositionCount => UniquePositions.Count;

        public Tuple<int, int> CurrentLocation => new Tuple<int, int>(CurrentLocationX, CurrentLocationY);

        public void Execute(string direction, int steps)
        {
            int x = CurrentLocationX;
            int y = CurrentLocationY;
            int dx = DIRECTION_POSITIVE;
            int dy = DIRECTION_POSITIVE;
            int stepsX = 0;
            int stepsY = 0;

            switch (direction)
            {
                case "E":
                    x = Math.Clamp(CurrentLocationX + steps, MIN_GRID_POSITION, MAX_GRID_POSITION);
                    stepsX = x - CurrentLocationX;
                    break;
                case "W":
                    x = Math.Clamp(CurrentLocationX - steps, MIN_GRID_POSITION, MAX_GRID_POSITION);
                    stepsX = CurrentLocationX - x;
                    dx = DIRECTION_NEGATIVE;
                    break;
                case "S":
                    y = Math.Clamp(CurrentLocationY + steps, MIN_GRID_POSITION, MAX_GRID_POSITION);
                    stepsY = y - CurrentLocationY;
                    break;
                case "N":
                    y = Math.Clamp(CurrentLocationY - steps, MIN_GRID_POSITION, MAX_GRID_POSITION);
                    stepsY = CurrentLocationY - y;
                    dy = DIRECTION_NEGATIVE;
                    break;
            }

            int intermediateLocationX;
            for (int i = 1; i < stepsX; ++i)
            {
                intermediateLocationX = CurrentLocationX + i * dx;
                UniquePositions.Add(intermediateLocationX + "," + CurrentLocationY);
            }

            int intermediateLocationY;
            for (int i = 1; i < stepsY; ++i)
            {
                intermediateLocationY = CurrentLocationY + i * dy;
                UniquePositions.Add(CurrentLocationX + "," + intermediateLocationY);
            }

            UniquePositions.Add(x + "," + y);
            CurrentLocationX = x;
            CurrentLocationY = y;
        }

    }
}
