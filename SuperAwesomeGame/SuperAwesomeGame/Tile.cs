using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeGame
{
    public static class Tile
    {
        public static Texture2D TileSetTexture;
        public static int TileWidth = 64;
        public static int TileHeight = 64;
        public static int TileStepX = 64;
        public static int TileStepY = 16;
        public static int OddRowXOffset = 32;
        public static int HeightTileOffset = 32;

        public static Rectangle GetSourceRectangle(int tileIndex)
        {
            var tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            var tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
