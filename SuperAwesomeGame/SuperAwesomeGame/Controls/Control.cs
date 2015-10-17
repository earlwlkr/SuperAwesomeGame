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

        public Control(string content)
        {
            SoundEffect = Manager.ClickSoundEffect;
            Font = Manager.Font;
            Content = content;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
