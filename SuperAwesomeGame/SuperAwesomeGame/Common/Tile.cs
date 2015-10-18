using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeGame.Common
{
    public static class Tile
    {
        public static Texture2D TileSetTexture;
        public static int TileWidth = 64;
        public static int TileHeight = 64;
        public static int TileStepX = TileWidth;
        public static int TileStepY = TileHeight / 4;
        public static int OddRowXOffset = TileWidth / 2;
        public static int HeightTileOffset = TileWidth / 2;

        public static Rectangle GetSourceRectangle(int tileIndex)
        {
            var tileY = tileIndex / (TileSetTexture.Width / TileWidth);
            var tileX = tileIndex % (TileSetTexture.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }
    }
}
