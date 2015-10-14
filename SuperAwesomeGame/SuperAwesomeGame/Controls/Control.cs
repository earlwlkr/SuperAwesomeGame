using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeGame.Controls
{
    public class Control : GameEntity
    {
        public SpriteFont Font { get; private set; }

        public SoundEffect SoundEffect { get; private set; }

        public string Content { get; private set; }

        public Control(SoundEffect soundEffect, 
            SpriteFont font, string content)
        {
            SoundEffect = soundEffect;
            Font = font;
            Content = content;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
