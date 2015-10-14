using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeGame
{
    public class Sprite2D : GameEntity
    {
        public int CurrentIndex { get; set; }

        private List<Texture2D> _textures;

        public List<Texture2D> Textures
        {
            get { return _textures; }
            set
            {
                _textures = value;
                Area.Width = _textures[0].Width;
                Area.Height = _textures[0].Height;
                CurrentIndex = 0;
            }
        }

        public Sprite2D(float left, float top, List<Texture2D> resources)
        {
            Area.Left = left;
            Area.Top = top;
            Textures = resources;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentIndex = (CurrentIndex + 1) % _textures.Count;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textures[CurrentIndex], new Vector2(Area.Left, Area.Top), Color.White);
        }
    }
}
