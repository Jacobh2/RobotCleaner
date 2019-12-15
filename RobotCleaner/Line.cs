using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RobotCleaner
{
    public class Line
    {
        public short Id;
        private readonly HashSet<short> _checkedIntersect = new HashSet<short>();
        private readonly HashSet<short> _checkedOverlapping = new HashSet<short>();

        public Point Start;
        public Point End;
        public int DeltaY;
        public int DeltaX;
        public int C;
        public int Length;
        public bool MovingX;
        private int _dx;
        private int _dy;

        public Line(int startX, int startY, int endX, int endY, int dx, int dy, short id)
        {
            Id = id;
            Start = new Point(startX, startY);
            End = new Point(endX, endY);
            // For a line DeltaY * x + DeltaX * y = C
            DeltaY = End.Y - Start.Y;
            DeltaX = End.X - Start.X;
            C = DeltaY * Start.X + DeltaX * Start.Y;
            Length = Math.Abs(End.Y - Start.Y) + Math.Abs(End.X - Start.X) + 1;
            MovingX = Start.X != End.X;
            _dx = dx;
            _dy = dy;
        }

        public int OverlappingFor(Line other)
        {
            if (ParallelDelta(other) != 0)
            {    
                Console.WriteLine("Not parallel");
                return 0;
            }
            // Parallel, calculate how much they overlap
            if (MovingX)
            {
                Console.WriteLine("Moving X");
                
                if (Start.Y != other.Start.Y) return 0;
                
                Console.WriteLine("Moving along X axis");
                // Along x axis, calculate how much overlap
                return Overlap(Start.X, End.X, other.Start.X, other.End.X);
            }
            Console.WriteLine("Moving Y");
            
            if (Start.X != other.Start.X) return 0;
            
            Console.WriteLine("Moving along Y axis");
            //Along Y axis, calculata how much the overlap
            return Overlap(Start.Y, End.Y, other.Start.Y, other.End.Y);

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMax(int first, int second)
        {
            return first > second ? first : second;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetMin(int first, int second)
        {
            return first < second ? first : second;
        }

        private int Overlap(int thisStart, int thisEnd, int otherStart, int otherEnd)
        {
            int thisMax = GetMax(thisStart, thisEnd);
            int thisMin = GetMin(thisStart, thisEnd);
            
            int otherMax = GetMax(otherStart, otherEnd);
            int otherMin = GetMin(otherStart, otherEnd);

            return GetMax(0, GetMin(thisMax, otherMax) - GetMax(thisMin, otherMin));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ParallelDelta(Line other)
        {
            return DeltaY * other.DeltaX - other.DeltaY * DeltaX;
        }
        
        public bool Intersect(Line other, out int x, out int y)
        {
            x = -1;
            y = -1;
            if (_checkedIntersect.Contains(other.Id) || other._checkedIntersect.Contains(Id))
                return false;

            _checkedIntersect.Add(other.Id);

            int delta = ParallelDelta(other);
            
            if (delta == 0)
                return false;
            
            x = (other.DeltaX * C - DeltaX * other.C) / delta;
            y = (DeltaY * other.C - other.DeltaY * C) / delta;
            return Inside(x,y) && other.Inside(x, y);
        }
        
        private bool Inside(int x, int y){
            bool inY;
            bool inX;
            if (Start.Y > End.Y)
            {
                inY = End.Y <= y && y <= Start.Y;
            }
            else
            {
                inY = Start.Y <= y && y <= End.Y;
            }
            
            if (Start.X > End.X)
            {
                inX = End.X <= x && x <= Start.X;
            }
            else
            {
                inX = Start.X <= x && x <= End.X;
            }
            return inX && inY;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetHash(long x, long y)
        {
            return (x+100_000)*1_000_000 + y + 100_000;
        }

        public void AddCoordinates(HashSet<long> counted)
        {
            int stepsX = GetMax(Start.X, End.X) - GetMin(Start.X, End.X);
            int stepsY = GetMax(Start.Y, End.Y) - GetMin(Start.Y, End.Y);
            
            for (int i = 1; i < stepsX; ++i)
            {
                counted.Add(GetHash(Start.X + i * _dx, Start.Y));
            }

            for (int i = 1; i < stepsY; ++i)
            {
                counted.Add(GetHash(Start.X, Start.Y + i * _dy));
            }
        }

        public void AddOverlappingCoordinates(Line other, HashSet<long> counted)
        {
            if (_checkedOverlapping.Contains(other.Id) || other._checkedOverlapping.Contains(Id)){
                Console.WriteLine($"Line {Id} has already been checked against {other.Id}");
                return;
            }
            
            Console.WriteLine($"Adding {other.Id} as checked against {Id}");
            _checkedOverlapping.Add(other.Id);
            
            if (ParallelDelta(other) != 0)
            {    
                return;
            }
            // Parallel, calculate how much they overlap
            if (MovingX)
            {
                Console.WriteLine("Moving X");
                
                if (Start.Y != other.Start.Y) return;
                
                Console.WriteLine("Moving along X axis");
                // Get the left/top line and the bottom/right line

                int thisLow = GetMin(Start.X, End.X);
                int otherLow = GetMin(other.Start.X, other.End.X);
                int thisHigh = GetMax(Start.X, End.X);
                int otherHigh = GetMax(other.Start.X, other.End.X);
                
                int startX = thisLow;
                int stepsX = otherHigh - thisLow + 1;
                if (thisLow < otherLow)
                {
                    startX = otherLow;
                    stepsX = thisHigh - otherLow + 1;
                }
                
                for (int i = 0; i < stepsX; ++i)
                {
                    Console.WriteLine($"    >> ({startX + i * _dx}, {Start.Y})");
                    counted.Add(GetHash(startX + i * _dx, Start.Y));
                }

                return;
            }
            Console.WriteLine("Moving Y");
            
            if (Start.X != other.Start.X) return;
            
            int thisLowY = GetMin(Start.Y, End.Y);
            int otherLowY = GetMin(other.Start.Y, other.End.Y);
            int thisHighY = GetMax(Start.Y, End.Y);
            int otherHighY = GetMax(other.Start.Y, other.End.Y);
                
            int startY = thisLowY;
            int stepsY = otherHighY - thisLowY + 1;
            if (thisLowY < otherLowY)
            {
                startY = otherLowY;
                stepsY = thisHighY - otherLowY + 1;
            }
                
            for (int i = 0; i < stepsY; ++i)
            {
                Console.WriteLine($"    >> ({Start.X}, {startY + i * _dy})");
                counted.Add(GetHash(Start.X, startY + i * _dy));
            }
        }
    }
}