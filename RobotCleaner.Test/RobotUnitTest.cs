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
            Robot robot = new Robot(Robot.MAX_POSITION, Robot.MAX_POSITION);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MAX_POSITION, Robot.MAX_POSITION);

            robot.Execute("S", 1);

            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void CannotMovePastMinumum()
        {
            Robot robot = new Robot(Robot.MIN_POSITION, Robot.MIN_POSITION);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MIN_POSITION, Robot.MIN_POSITION);

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

            Assert.Equal(2, robot.GetUniquePositions());
        }

        [Fact]
        public void SmallTest()
        {
            Robot robot = new Robot(10, 22);
            Tuple<int, int> finalLocation = new Tuple<int, int>(12, 21);

            robot.Execute("E", 2);
            robot.Execute("N", 1);

            Assert.Equal(4, robot.GetUniquePositions());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void BigTest()
        {
            Robot robot = new Robot(10, 22);
            Tuple<int, int> finalLocation = new Tuple<int, int>(-47650, -16789);

            Random rnd = new Random(1337);
            string[] directions = new string[] { "E", "W", "S", "N" };

            for (int i = 0; i < 100; ++i)
            {
                string direction = directions.OrderBy(x => rnd.Next()).Take(1).First();
                robot.Execute(direction, rnd.Next(0, 100000));
            }

            Assert.Equal(2584093, robot.GetUniquePositions());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }

        [Fact]
        public void LongSteps()
        {
            Robot robot = new Robot(10, 22);
            Tuple<int, int> finalLocation = new Tuple<int, int>(90978, -1785);

            Random rnd = new Random(1337);
            string[] directions = new string[] { "E", "W", "S", "N" };

            for (int i = 0; i < 10; ++i)
            {
                string direction = directions.OrderBy(x => rnd.Next()).Take(1).First();
                robot.Execute(direction, rnd.Next(90000, 100000));
            }

            Assert.Equal(484730, robot.GetUniquePositions());
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

            Assert.Equal(400001, robot.GetUniquePositions());
            Assert.Equal(finalLocation, robot.CurrentLocation);
        }
    }
}
