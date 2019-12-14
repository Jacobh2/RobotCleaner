namespace RobotCleaner
{
    public class Square
    {
        private readonly int _topX;
        private readonly int _topY;
        private readonly int _bottomX;
        private readonly int _bottomY;

        public Square(int topx, int topy, int bottomx, int bottomy)
        {
            _topX = topx;
            _topY = topy;
            _bottomX = bottomx;
            _bottomY = bottomy;
        }

        public int Right => _bottomX;
        public int Left => _topX;
        public int Bottom => _bottomY;
        public int Top => _topY;

        public bool Contains(int x, int y)
        {
            return _topX <= x && x <= _bottomX && _topY <= y && y <= _bottomY;
        }
    }
}