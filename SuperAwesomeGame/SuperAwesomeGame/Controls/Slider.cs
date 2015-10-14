using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame.Controls
{
    public class Slider : Control
    {
        private float _markerPosition;

        public Texture2D TextureSlider { get; private set; }

        public Texture2D TextureMarker { get; private set; }


        public Slider(SoundEffect soundEffect, 
            SpriteFont font, string content, float left, float top, Texture2D slider, Texture2D marker, float value = 0f)
            : base(soundEffect, font, content)
        {
            Area.Left = left;
            Area.Top = top;

            TextureSlider = slider;
            TextureMarker = marker;

            Area.Width = slider.Width;
            Area.Height = slider.Height + marker.Height;

            if (value > 100)
            {
                value = 100;
            }
            else if (value < 0)
            {
                value = 0;
            }

            _markerPosition = Area.Left - TextureMarker.Width / 2 + value * Area.Width / 100;
            
            Area.Top -= (slider.Height + marker.Height);
        }

        public float Value
        {
            get
            {
                return ((_markerPosition + TextureMarker.Width / 2 - Area.Left) / Area.Width) * 100;
            }
        }

        public override void Select(bool toggle)
        {
            base.Select(toggle);
            
            if (toggle)
            {
                if (State == EntityState.Selected)
                {
                    var mouseX = (float)Mouse.GetState().X;
                    if (mouseX < Area.Left)
                    {
                        mouseX = Area.Left;
                    }
                    else if (mouseX > Area.Left + Area.Width)
                    {
                        mouseX = Area.Left + Area.Width;
                    }

                    _markerPosition = mouseX - TextureMarker.Width / 2;
                }
                else
                {
                    State = EntityState.Selected;
                }
            }
            else
            {
                if (State == EntityState.Selected)
                {
                    State = EntityState.Default;
                    SoundEffect.Play();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var center = Area.GetCenter();
            var fontPos = new Vector2(center.X, center.Y + TextureMarker.Height);
            // Place origin at the center of the string.
            var fontOrigin = Font.MeasureString(Content) / 2;

            spriteBatch.Draw(TextureSlider, new Vector2(Area.Left, Area.Top + TextureMarker.Height), Color.White);
            spriteBatch.Draw(TextureMarker, new Vector2(_markerPosition, Area.Top), Color.White);
            spriteBatch.DrawString(Font, Content, fontPos, Color.GhostWhite, 0f, fontOrigin, 1.0f, SpriteEffects.None, 0f);
        }

        public event EventHandler OnClick;
    }
}
