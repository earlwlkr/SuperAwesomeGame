using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame
{
    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    public class TileMap : GameEntity
    {
        public List<MapRow> Rows = new List<MapRow>();

        public const float MapWidth = 120;
        public const float MapHeight = 120;

        public const int CellsAcross = 80;
        public const int CellsDown = 80;

        private const int BaseOffsetX = -32;
        private const int BaseOffsetY = -64;
        private const float HeightRowDepthMod = 0.00001f;

        public const int TileWidth = 64;
        public const int TileHeight = 64;
        public const int TileStepX = TileWidth;
        public const int TileStepY = TileHeight / 4;
        public const int OddRowXOffset = TileWidth / 2;
        public const int HeightTileOffset = TileWidth / 2;

        public float WidthLimit
        {
            get { return (MapWidth - CellsAcross) * TileStepX; }
        }

        public float HeightLimit
        {
            get { return (MapHeight - CellsDown) * TileStepY; }
        }

        public TileMap()
        {
            for (var y = 0; y < MapHeight; y++)
            {
                var row = new MapRow();
                for (var x = 0; x < MapWidth; x++)
                {
                    row.Columns.Add(new MapCell(0));
                }
                Rows.Add(row);
            }

            Rows[16].Columns[4].AddHeightTile(54);

            Rows[17].Columns[3].AddHeightTile(54);

            Rows[15].Columns[3].AddHeightTile(54);
            Rows[16].Columns[3].AddHeightTile(53);

            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(54);
            Rows[15].Columns[4].AddHeightTile(51);

            Rows[18].Columns[3].AddHeightTile(51);
            Rows[19].Columns[3].AddHeightTile(50);
            Rows[18].Columns[4].AddHeightTile(55);

            Rows[14].Columns[4].AddHeightTile(54);

            Rows[14].Columns[5].AddHeightTile(62);
            Rows[14].Columns[5].AddHeightTile(61);
            Rows[14].Columns[5].AddHeightTile(63);

            Rows[17].Columns[4].AddTopperTile(114);
            Rows[16].Columns[5].AddTopperTile(115);
            Rows[14].Columns[4].AddTopperTile(125);
            Rows[15].Columns[5].AddTopperTile(91);
            Rows[16].Columns[6].AddTopperTile(94);
        }

        public override void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                Manager.Camera.MoveX(-2);
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                Manager.Camera.MoveX(2);
            }

            if (ks.IsKeyDown(Keys.Up))
            {
                Manager.Camera.MoveY(-2);
            }

            if (ks.IsKeyDown(Keys.Down))
            {
                Manager.Camera.MoveY(2);
            }

            base.Update(gameTime);
        }

        public Rectangle GetSourceRectangle(int tileIndex)
        {
            var tileY = tileIndex / (Manager.MapTextureSet.Width / TileWidth);
            var tileX = tileIndex % (Manager.MapTextureSet.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // The cell on the top left of the viewport.
            var firstX = (int)(Manager.Camera.Location.X / TileStepX);
            var firstY = (int)(Manager.Camera.Location.Y / TileStepY);

            // The offset of that first cell.
            var offsetX = (int)(Manager.Camera.Location.X % TileStepX);
            var offsetY = (int)(Manager.Camera.Location.Y % TileStepY);

            var maxdepth = ((MapWidth + 1) * ((MapHeight + 1) * TileWidth)) / 10;

            for (var y = 0; y < CellsDown; y++)
            {
                var rowOffset = 0;
                // Move right if it's the odd row to fit tiles.
                if ((firstY + y) % 2 != 0)
                    rowOffset = OddRowXOffset;

                for (var x = 0; x < CellsAcross; x++)
                {
                    // The coordinates on the real map.
                    var mapx = (firstX + x);
                    var mapy = (firstY + y);
                    var depthOffset = 0.7f - ((mapx + (mapy * TileWidth)) / maxdepth);

                    foreach (var tileId in Rows[mapy].Columns[mapx].BaseTiles)
                    {
                        spriteBatch.Draw(
                            Manager.MapTextureSet,
                            new Rectangle(
                                (x * TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * TileStepY) - offsetY + BaseOffsetY,
                                TileWidth, TileHeight),
                            GetSourceRectangle(tileId),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0.1f);
                    }

                    var heightRow = 0;

                    foreach (var tileId in Rows[mapy].Columns[mapx].HeightTiles)
                    {
                        spriteBatch.Draw(
                            Manager.MapTextureSet,
                            new Rectangle(
                                (x * TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * TileStepY) - offsetY + BaseOffsetY - (heightRow * HeightTileOffset),
                                TileWidth, TileHeight),
                            GetSourceRectangle(tileId),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            depthOffset - ((float)heightRow * HeightRowDepthMod));
                        heightRow++;
                    }

                    foreach (var tileId in Rows[mapy].Columns[mapx].TopperTiles)
                    {
                        spriteBatch.Draw(
                            Manager.MapTextureSet,
                            new Rectangle(
                                (x * TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * TileStepY) - offsetY + BaseOffsetY - (heightRow * HeightTileOffset),
                                TileWidth, TileHeight),
                            GetSourceRectangle(tileId),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            depthOffset - ((float)heightRow * HeightRowDepthMod));
                    }
                }
            }
        }
    }
}
