using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame.Controls
{
    public class Button : Control
    {
        public Texture2D TextureIdle { get; private set; }

        public Texture2D TextureSelected { get; private set; }


        public Button(SoundEffect soundEffect, 
            SpriteFont font, string content, float left, float top, Texture2D idle, Texture2D selected)
            : base(soundEffect, font, content)
        {
            Area.Left = left;
            Area.Top = top;

            TextureIdle = idle;
            TextureSelected = selected;

            Area.Width = idle.Width;
            Area.Height = idle.Height;
        }

        public override void Select(bool toggle)
        {
            base.Select(toggle);

            if (!toggle && State == EntityState.Selected && OnClick != null)
            {
                OnClick(this, new EventArgs());
            }

            if (toggle && State != EntityState.Selected)
            {
                State = EntityState.Selected;
                SoundEffect.Play();
            }
            else if (!toggle)
            {
                State = EntityState.Default;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var fontPos = Area.GetCenter();
            // Place origin at the center of the string.
            var fontOrigin = Font.MeasureString(Content) / 2;

            if (State == EntityState.Default)
            {
                spriteBatch.Draw(TextureIdle, new Vector2(Area.Left, Area.Top), Color.White);
                spriteBatch.DrawString(Font, Content, fontPos, Color.White, 0f, fontOrigin, 1.0f, SpriteEffects.None, 0f);
            }
            else
            {
                var offsetHeight = Math.Abs(TextureIdle.Height - TextureSelected.Height);
                spriteBatch.Draw(TextureSelected, new Vector2(Area.Left, Area.Top + offsetHeight), Color.White);
                spriteBatch.DrawString(Font, Content, new Vector2(fontPos.X, fontPos.Y + offsetHeight), Color.White, 0f, fontOrigin, 1.0f, SpriteEffects.None, 0f);
            }
        }

        public event EventHandler OnClick;
    }
}
