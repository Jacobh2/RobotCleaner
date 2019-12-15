using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RobotCleaner
{
    public class Line
    {
        public short Id;
        private readonly HashSet<short> _checkedIntersect = new HashSet<short>();
        private readonly HashSet<short> _checkedOverlapping = new HashSet<short>();

        private Point _start;
        private Point _end;
        private readonly int _deltaY;
        private readonly int _deltaX;
        private readonly int _c;
        public readonly int Length;
        private readonly bool _movingX;
        private readonly int _dx;
        private readonly int _dy;

        public Line(int startX, int startY, int endX, int endY, int dx, int dy, short id)
        {
            Id = id;
            _start = new Point(startX, startY);
            _end = new Point(endX, endY);
            // For a line DeltaY * x + DeltaX * y = C
            _deltaY = _end.Y - _start.Y;
            _deltaX = _end.X - _start.X;
            _c = _deltaY * _start.X + _deltaX * _start.Y;
            Length = Math.Abs(_end.Y - _start.Y) + Math.Abs(_end.X - _start.X) + 1;
            _movingX = _start.X != _end.X;
            _dx = dx;
            _dy = dy;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ParallelDelta(Line other)
        {
            return _deltaY * other._deltaX - other._deltaY * _deltaX;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long GetHash(long x, long y)
        {
            return (x+100_000)*1_000_000 + y + 100_000;
        }

        private void AddXCoordinates(Line other, HashSet<long> overlapping)
        {
            int thisLow = GetMin(_start.X, _end.X);
            int thisHigh = GetMax(_start.X, _end.X);
            int otherLow = GetMin(other._start.X, other._end.X);
            int otherHigh = GetMax(other._start.X, other._end.X);
                
            int startX = thisLow;
            int stepsX = otherHigh - thisLow + 1;
            if (thisLow < otherLow)
            {
                startX = otherLow;
                stepsX = thisHigh - otherLow + 1;
            }
                
            for (int i = 0; i < stepsX; ++i)
            {
                overlapping.Add(GetHash(startX + i * _dx, _start.Y));
            }
        }

        private void AddYCoordinates(Line other, HashSet<long> overlapping)
        {
            int thisLow = GetMin(_start.Y, _end.Y);
            int otherLow = GetMin(other._start.Y, other._end.Y);
            int thisHigh = GetMax(_start.Y, _end.Y);
            int otherHigh = GetMax(other._start.Y, other._end.Y);
                
            int startY = thisLow;
            int stepsY = otherHigh - thisLow + 1;
            if (thisLow < otherLow)
            {
                startY = otherLow;
                stepsY = thisHigh - otherLow + 1;
            }
                
            for (int i = 0; i < stepsY; ++i)
            {
                overlapping.Add(GetHash(_start.X, startY + i * _dy));
            }
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
            
            x = (other._deltaX * _c - _deltaX * other._c) / delta;
            y = (_deltaY * other._c - other._deltaY * _c) / delta;
            return Inside(x,y) && other.Inside(x, y);
        }
        
        private bool Inside(int x, int y){
            int minY = GetMin(_start.Y, _end.Y);
            int maxY = GetMax(_start.Y, _end.Y);
            bool inY = minY <= y && y <= maxY;
            
            int minX = GetMin(_start.X, _end.X);
            int maxX = GetMax(_start.X, _end.X);
            bool inX = minX <= x && x <= maxX;
            return inX && inY;
        }
        
        

        public void AddOverlappingCoordinates(Line other, HashSet<long> overlapping)
        {
            if (_checkedOverlapping.Contains(other.Id) || other._checkedOverlapping.Contains(Id)){
                return;
            }
            
            _checkedOverlapping.Add(other.Id);
            
            if (ParallelDelta(other) != 0)
            {    
                return;
            }
            
            if (_movingX)
            {
                if (_start.Y != other._start.Y) return;
                AddXCoordinates(other, overlapping);                
                return;
            }
            
            if (_start.X != other._start.X) return;

            AddYCoordinates(other, overlapping);
        }
    }
}