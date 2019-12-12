using System;
using System.Linq;

namespace RobotCleaner
{
    class Program
    {
        static void Main(string[] args)
        {

            int numCommands = Convert.ToInt32(Console.ReadLine());
            int[] startCoordinates = Console.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

            Robot robot = new Robot(startCoordinates[0], startCoordinates[1]);

            for (int i = 0; i < numCommands; ++i)
            {
                string[] direction = Console.ReadLine().Split(' ');
                robot.Execute(direction[0], Convert.ToInt32(direction[1]));
            }

            Console.WriteLine($"=> Cleaned: {robot.UniquePositionCount}");
        }
    }
}
