using System;
using System.Drawing;
using System.Linq;

namespace RobotCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeAlgorithm();
            /*
            int numCommands = Convert.ToInt32(Console.ReadLine());
            int[] startCoordinates = Console.ReadLine().Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

            Robot robot = new Robot(startCoordinates[0], startCoordinates[1]);

            for (int i = 0; i < numCommands; ++i)
            {
                string[] direction = Console.ReadLine().Split(' ');
                robot.Execute(direction[0], Convert.ToInt32(direction[1]));
            }

            Console.WriteLine($"=> Cleaned: {robot.UniquePositionCount()}");
            */
        }

        private static void TimeAlgorithm()
        {
            /*
             Using 20 
             Running 1000 steps
             Took 35.695913 seconds
            
             Using 20 
             Running 100 steps
             Took 0.585131 seconds
             
             Using 2
             Running 100 steps
             Took 1.572883 seconds
             
             Using 10
             Running 100 steps
             Took 0.657997 seconds


            */
            int numberOfSteps = 10000;
            Robot robot = new Robot(0, 0);
            //Tuple<int, int> finalLocation = new Tuple<int, int>(80861, 100000);
            Tuple<int, int> finalLocation = new Tuple<int, int>(70714, 100000);

            Random rnd = new Random(1337);
            string[] directions = new string[] { "E", "W", "S", "N" };
            int[] directionIndex = Enumerable.Range(0, numberOfSteps)
                .Select(i => rnd.Next(0, 3)).ToArray();

            int[] steps = Enumerable.Range(0, numberOfSteps).Select(i => rnd.Next(0, 100000)).ToArray();
            
            Console.ReadLine();
            DateTime start = DateTime.UtcNow;
            Console.WriteLine($"Running {numberOfSteps} steps");
            for (int i = 0; i < numberOfSteps; ++i)
            {
                robot.Execute(directions[directionIndex[i]], steps[i]);
            }
            Console.WriteLine($"Took {DateTime.UtcNow.Subtract(start).TotalSeconds} seconds");
        }
    }
}
