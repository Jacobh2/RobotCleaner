using System;
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

            // Go one step down
            robot.Execute("S", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CanMoveNorth()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(0, -1);

            // Go one step up
            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CanMoveEast()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(1, 0);

            // Go one step right
            robot.Execute("E", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CanMoveWest()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(-1, 0);

            // Go one step left
            robot.Execute("W", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CanMoveOverPlacesTwice()
        {
            Robot robot = new Robot(0, 0);
            Tuple<int, int> finalLocation = new Tuple<int, int>(0, 0);

            // Go one step down
            robot.Execute("S", 1);
            // and then one step up again
            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CannotMovePastMaximum()
        {
            Robot robot = new Robot(Robot.MAX_POSITION, Robot.MAX_POSITION);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MAX_POSITION, Robot.MAX_POSITION);

            // Go one step down, this should not succeed
            robot.Execute("S", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void CannotMovePastMinumum()
        {
            Robot robot = new Robot(Robot.MIN_POSITION, Robot.MIN_POSITION);
            Tuple<int, int> finalLocation = new Tuple<int, int>(Robot.MIN_POSITION, Robot.MIN_POSITION);

            // Go one step down, this should not succeed
            robot.Execute("N", 1);

            Assert.Equal(finalLocation, robot.CurrentPosition);
        }

        [Fact]
        public void DoesNotCountSamePlaceTwice()
        {
            Robot robot = new Robot(0, 0);

            // Go right and left 2 times, but we only want to count it once
            robot.Execute("E", 1);
            robot.Execute("W", 1);
            robot.Execute("E", 1);
            robot.Execute("W", 1);

            Assert.Equal(2, robot.GetUniquePositions());
        }

        [Fact]
        public void ProvidedTestcase()
        {
            Robot robot = new Robot(10, 22);
            Tuple<int, int> finalLocation = new Tuple<int, int>(12, 21);

            // Go right and left 2 times, but we only want to count it once
            robot.Execute("E", 2);
            robot.Execute("N", 1);

            Assert.Equal(4, robot.GetUniquePositions());
            Assert.Equal(finalLocation, robot.CurrentPosition);
        }
    }
}
