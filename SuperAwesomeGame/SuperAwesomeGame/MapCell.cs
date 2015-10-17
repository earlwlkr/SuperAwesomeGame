using System.Collections.Generic;

namespace SuperAwesomeGame
{
    public class MapCell
    {
        public List<int> BaseTiles = new List<int>();
        public List<int> HeightTiles = new List<int>();
        public List<int> TopperTiles = new List<int>();

        public int TileID
        {
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
            set
            {
                if (BaseTiles.Count > 0)
                {
                    BaseTiles[0] = value;
                }
                else
                {
                    AddBaseTile(value);
                }
            }
        }

        public void AddBaseTile(int tileId)
        {
            BaseTiles.Add(tileId);
        }

        public void AddHeightTile(int tileId)
        {
            HeightTiles.Add(tileId);
        }

        public void AddTopperTile(int tileId)
        {
            TopperTiles.Add(tileId);
        }

        public MapCell(int tileId)
        {
            TileID = tileId;
        }
    }
}
