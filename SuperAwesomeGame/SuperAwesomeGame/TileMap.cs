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

        public float WidthLimit
        {
            get { return (MapWidth - CellsAcross) * Tile.TileStepX; }
        }

        public float HeightLimit
        {
            get { return (MapHeight - CellsDown) * Tile.TileStepY; }
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // The cell on the top left of the viewport.
            var firstX = (int)(Manager.Camera.Location.X / Tile.TileStepX);
            var firstY = (int)(Manager.Camera.Location.Y / Tile.TileStepY);

            // The offset of that first cell.
            var offsetX = (int)(Manager.Camera.Location.X % Tile.TileStepX);
            var offsetY = (int)(Manager.Camera.Location.Y % Tile.TileStepY);

            var maxdepth = ((MapWidth + 1) * ((MapHeight + 1) * Tile.TileWidth)) / 10;

            for (var y = 0; y < CellsDown; y++)
            {
                var rowOffset = 0;
                // Move right if it's the odd row to fit tiles.
                if ((firstY + y) % 2 != 0)
                    rowOffset = Tile.OddRowXOffset;

                for (var x = 0; x < CellsAcross; x++)
                {
                    // The coordinates on the real map.
                    var mapx = (firstX + x);
                    var mapy = (firstY + y);
                    var depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);

                    foreach (var tileId in Rows[mapy].Columns[mapx].BaseTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Rectangle(
                                (x * Tile.TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * Tile.TileStepY) - offsetY + BaseOffsetY,
                                Tile.TileWidth, Tile.TileHeight),
                            Tile.GetSourceRectangle(tileId),
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
                            Tile.TileSetTexture,
                            new Rectangle(
                                (x * Tile.TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * Tile.TileStepY) - offsetY + BaseOffsetY - (heightRow * Tile.HeightTileOffset),
                                Tile.TileWidth, Tile.TileHeight),
                            Tile.GetSourceRectangle(tileId),
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
                            Tile.TileSetTexture,
                            new Rectangle(
                                (x * Tile.TileStepX) - offsetX + rowOffset + BaseOffsetX,
                                (y * Tile.TileStepY) - offsetY + BaseOffsetY - (heightRow * Tile.HeightTileOffset),
                                Tile.TileWidth, Tile.TileHeight),
                            Tile.GetSourceRectangle(tileId),
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
