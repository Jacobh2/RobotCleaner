using System;
using System.Linq;
using Xunit;

namespace RobotCleaner.Test
{
    public class RobotUnitTest
    {
        [Fact]
        public void CanMoveSouth()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(0, 1);

            robot.Execute("S", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CanMoveNorth()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(0, -1);

            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CanMoveEast()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(1, 0);

            robot.Execute("E", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CanMoveWest()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(-1, 0);

            robot.Execute("W", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CanMoveOverPlacesTwice()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(0, 0);

            robot.Execute("S", 1);
            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CannotMovePastMaximum()
        {
            Robot robot = new Robot(Robot.MaxGridPosition, Robot.MaxGridPosition);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MaxGridPosition, Robot.MaxGridPosition);

            robot.Execute("S", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CannotMovePastMinumum()
        {
            Robot robot = new Robot(Robot.MinGridPosition, Robot.MinGridPosition);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MinGridPosition, Robot.MinGridPosition);

            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void DoesNotCountSamePlaceTwice()
        {
            Robot robot = new Robot(0, 0);

            robot.Execute("E", 1);
            robot.Execute("W", 1);
            robot.Execute("E", 1);
            robot.Execute("W", 1);

            Assert.Equal(2, robot.UniquePositionCount());
        }

        [Fact]
        public void TwoExecutions()
        {
            Robot robot = new Robot(10, 22);
            Tuple<int, int> finalLocation = new Tuple<int, int>(12, 21);

            robot.Execute("E", 2);
            robot.Execute("N", 1);

            Assert.Equal(4, robot.UniquePositionCount());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }
        
        [Fact]
        public void TestLongSteps()
        {
            Robot robot = new Robot(-100_000, -100_000);
            Tuple<int, int> finalLocation = new Tuple<int, int>(30_000,-5_000);

            robot.Execute("E", 100_000);
            robot.Execute("E", 100_000);
            robot.Execute("S", 75_000);
            
            robot.Execute("W", 50_000);
            robot.Execute("N", 50_000);
            robot.Execute("E", 60_000);
            
            robot.Execute("N", 50_000);
            robot.Execute("W", 80_000);
            robot.Execute("S", 95_000);
            
            robot.Execute("E", 10_000);

            Assert.Equal(530000, robot.UniquePositionCount());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }
        
        [Fact]
        public void GoFromEdgeToEdge()
        {
            Robot robot = new Robot(-100000, -100000);
            Tuple<int, int> finalLocation = new Tuple<int, int>(100000, 100000);

            robot.Execute("E", 100000);
            robot.Execute("E", 100000);
            robot.Execute("E", 100);
            robot.Execute("S", 100000);
            robot.Execute("S", 100000);
            robot.Execute("S", 100);

            Assert.Equal(400001, robot.UniquePositionCount());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }
        
        [Fact]
        public void Use10000Steps()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(80861, 100000);

            Random rnd = new Random(1337);
            string[] directions = new string[] { "E", "W", "S", "N" };
            int[] directionIndex = Enumerable.Range(0, 10000)
                .Select(i => rnd.Next(0, 3)).ToArray();

            int[] steps = Enumerable.Range(0, 10000).Select(i => rnd.Next(0, 100000)).ToArray();

            for (int i = 0; i < 10000; ++i)
            {
                robot.Execute(directions[directionIndex[i]], steps[i]);
            }

            Assert.Equal(400001, robot.UniquePositionCount());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }
    }
}
