using System;
using System.Collections.Generic;
using System.Drawing;

namespace RobotCleaner
{
    public class LineLight : HashSet<Tuple<Point, Point>>
    {

        public void Add(Point a, Point b)
        {
            if (a.Equals(b))
            {
                //Check if we have this start point
                foreach (Tuple<Point, Point> line in this)
                {
                    if (line.Item1.Equals(a))
                    {
                        return;
                    }
                }
            }
            base.Add(new Tuple<Point, Point>(a, b));
        }
    }
}