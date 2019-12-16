using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RobotCleaner
{
    public class Line
    {
        public short Id;
        private readonly HashSet<short> _checked = new HashSet<short>();

        private Point _start;
        private Point _end;
        private readonly long _deltaY;
        private readonly long _deltaX;
        private readonly long _c;
        public readonly int Length;
        private readonly bool _movingX;
        private readonly int _dx;
        private readonly int _dy;
        private readonly int _minX;
        private readonly int _maxX;
        private readonly int _minY;
        private readonly int _maxY;

        public Line(int startX, int startY, int endX, int endY, int dx, int dy, short id)
        {
            Id = id;
            _start = new Point(startX, startY);
            _end = new Point(endX, endY);
            // For a line DeltaY * x + DeltaX * y = C
            _minX = GetMin(startX, endX);
            _maxX = GetMax(startX, endX);
            _minY = GetMin(startY, endY);
            _maxY = GetMax(startY, endY);
            _deltaX = endX - startX;
            _deltaY = endY - startY;
            _c = _deltaY * startX + _deltaX * startY;
            
            Length = (_maxX - _minX) + (_maxY - _minY) + 1;
            if (Length < 0)
            {
                throw new Exception("Length is negative");
            }
            _movingX = startX != endX;
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
        private long ParallelDelta(Line other)
        {
            return _deltaY * other._deltaX - other._deltaY * _deltaX;
        }

        public int AmountOverlapXAxis(Line other)
        {
            if (_maxX < other._minX || other._maxX < _minX){
                return 0;
            }
            if (_minX == other._minX)
            {
                return GetMin(_maxX, other._maxX) - _minX;
            }
            if (_maxX == other._maxX)
            {
                return _maxX - GetMax(_minX, other._minX);
            }
            if (_minX < other._minX)
            {
                return _maxX - other._minX;
            }
            return other._maxX - _minX;
        }
        
        public int AmountOverlapYAxis(Line other)
        {
            if (_maxY < other._minY || other._maxY < _minY){
                return 0;
            }
            if (_minY == other._minY)
            {
                return GetMin(_maxY, other._maxY) - _minY;
            }
            if (_maxY == other._maxY)
            {
                return _maxY - GetMax(_minY, other._minY);
            }
            if (_minY < other._minY)
            {
                return _maxY - other._minY;
            }
            return other._maxY - _minY;
        }
        
        private void AddXCoordinates(Line other, LineLight overlapping)
        {
            int stepsX = AmountOverlapXAxis(other);
            if (stepsX == 0)
            {
                return;
            }
            int startX =_minX < other._minX ? other._minX : _minX;
            overlapping.Add(new Point(startX, _start.Y), new Point(startX + stepsX, _start.Y));
        }

        private void AddYCoordinates(Line other, LineLight overlapping)
        {
            int stepsY = AmountOverlapYAxis(other);
            if (stepsY == 0)
            {
                return;
            }
            int startY =_minY < other._minY ? other._minY : _minY;
            overlapping.Add(new Point(_start.X, startY), new Point(_start.X, startY + stepsY));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Inside(int x, int y){
            return _minY <= y && y <= _maxY && _minX <= x && x <= _maxX;
        }

        public void AddOverlappingCoordinates(Line other, LineLight overlapping)
        {
            /* 1. Are they parallel?
             *     -> YES:
             *         1.How much do they overlap
             *     -> NO:
             *         1. Remove 1 where they intersect
             * 
             */
            _checked.Add(other.Id);
            
            long delta = ParallelDelta(other);
            
            if (delta != 0)
            {
                int x = (int) ((other._deltaX * _c - _deltaX * other._c) / delta);
                int y = (int) ((_deltaY * other._c - other._deltaY * _c) / delta);
                //Console.WriteLine($"Lines not parallel! ({x},{y})");
                if (Inside(x, y) && other.Inside(x, y))
                {
                    //Console.WriteLine($"They are touching");
                    overlapping.Add(new Point(x, y), new Point(x, y));
                }
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

        public override string ToString()
        {
            return $"L({_start},{_end})";
        }

        public bool AlreadyChecked(Line other)
        {
            return _checked.Contains(other.Id) || other._checked.Contains(Id);
        }

        public void Clear()
        {
            _checked.Clear();
        }
    }
}