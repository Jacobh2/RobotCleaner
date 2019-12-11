using System;
using System.Linq;

namespace RobotCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the number of commands
            int numCommands = Convert.ToInt32(Console.ReadLine());

            // Parse the start coordinates
            int[] startCoordinates = Console.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

            // Create a robot with the given start coordinates
            Robot robot = new Robot(startCoordinates[0], startCoordinates[1]);

            //Read the required amount of commands and execute them
            for (int i = 0; i < numCommands; ++i)
            {
                // Read new direction
                string[] direction = Console.ReadLine().Split(' ');

                // Execute it on the robot
                robot.Execute(direction[0], Convert.ToInt32(direction[1]));
            }

            // Output the unique positions
            Console.WriteLine($"=> Cleaned: {robot.GetUniquePositions()}");

        }
    }
}
