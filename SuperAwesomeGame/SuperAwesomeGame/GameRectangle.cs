using Microsoft.Xna.Framework;

namespace SuperAwesomeGame
{
    public class GameRectangle
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Vector2 BasePoint
        {
            get
            {
                return new Vector2(Left, Top);
            }
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public GameRectangle()
        { }

        public GameRectangle(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public bool Contains(Vector2 point)
        {
            return (point.X >= Left && point.X <= Left + Width)
                   && (point.Y >= Top && point.Y <= Top + Height);
        }

        public bool Contains(float x, float y)
        {
            return (x >= Left && x <= Left + Width)
                   && (y >= Top && y <= Top + Height);
        }

        public Vector2 GetCenter()
        {
            return new Vector2(Left + Width / 2, Top + Height / 2);
        }
    }
}
