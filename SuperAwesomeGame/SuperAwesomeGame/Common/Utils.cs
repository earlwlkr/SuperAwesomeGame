using Microsoft.Xna.Framework;

namespace SuperAwesomeGame.Common
{
    public static class Utils
    {
        public static Vector2 WorldToScreen(Vector2 pos)
        {
            return Manager.Camera == null ? pos : Vector2.Transform(pos, Manager.Camera.GetTransform());
        }

        public static Vector2 ScreenToWorld(Vector2 pos)
        {
            return Manager.Camera == null ? pos : Vector2.Transform(pos, Matrix.Invert(Manager.Camera.GetTransform()));
        }
    }
}
