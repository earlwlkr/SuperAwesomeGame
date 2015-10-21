using Microsoft.Xna.Framework;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame
{
    public class Camera
    {
        public Vector2 Location = Vector2.Zero;
        private float _scale = 1f;
        private float _rotation = 0f;
        private TileMap _map;

        public Camera(TileMap map)
        {
            _map = map;
        }

        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = MathHelper.Clamp(value, 0.6f, 1.4f);
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (value > 1)
                {
                    value = 1f;
                }
                else if (value < 0)
                {
                    value = 0f;
                }
                _rotation = value;
            }
        }

        public void MoveX(float offset)
        {
            Location.X = MathHelper.Clamp(Manager.Camera.Location.X + offset, 
                0, _map.WidthLimit);
        }

        public void MoveY(float offset)
        {
            Location.Y = MathHelper.Clamp(Manager.Camera.Location.Y + offset,
                0, _map.HeightLimit);
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) * 
                   Matrix.CreateScale(Scale);
        }
    }
}
