using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame.Controls
{
    public class Checkbox : Control
    {
        public bool Checked { get; set; }

        public Texture2D TextureChecked { get; private set; }

        public Texture2D TextureUnchecked { get; private set; }


        public Checkbox(SoundEffect soundEffect, 
            SpriteFont font, string content, float left, float top, Texture2D textureChecked, Texture2D textureUnchecked, bool value)
            : base(soundEffect, font, content)
        {
            Area.Left = left;
            Area.Top = top;

            TextureChecked = textureChecked;
            TextureUnchecked = textureUnchecked;

            Area.Width = TextureChecked.Width;
            Area.Height = TextureChecked.Height;

            Checked = value;
        }

        public bool Value
        {
            get { return Checked; }
        }

        public override void Select(bool toggle)
        {
            base.Select(toggle);

            if (toggle && State != EntityState.Selected)
            {
                State = EntityState.Selected;
                Checked = !Checked;
                SoundEffect.Play();
            }
            else if (!toggle)
            {
                State = EntityState.Default;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var center = Area.GetCenter();
            
            var fontOrigin = Font.MeasureString(Content) / 2;
            var fontPos = new Vector2(Area.Left - fontOrigin.X - 30, center.Y);

            var texture = Checked ? TextureChecked : TextureUnchecked;
            spriteBatch.Draw(texture, new Vector2(Area.Left, Area.Top), Color.White);
            spriteBatch.DrawString(Font, Content, fontPos, Color.GhostWhite, 0f, fontOrigin, 1.0f, SpriteEffects.None, 0f);
        }

        public event EventHandler OnClick;
    }
}
