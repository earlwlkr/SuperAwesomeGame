using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeGame
{
    public class Manager
    {
        private static Texture2D _mapTextureSet;

        private static List<Texture2D> _buttonTextures;
        private static List<Texture2D> _sliderTextures;
        private static List<Texture2D> _checkboxTextures;
        private static SoundEffect _clickSoundEffect;
        private static SpriteFont _font;

        public static ContentManager Content { get; set; }

        public static Texture2D MapTextureSet
        {
            get
            {
                if (Content == null) return null;
                return _mapTextureSet ?? (_mapTextureSet = Content.Load<Texture2D>(@"Sprite2D\part4_tileset"));
            }
        }

        public static List<Texture2D> ButtonTextures
        {
            get
            {
                if (Content == null) return null;
                return _buttonTextures ?? (_buttonTextures = new List<Texture2D>
                {
                    Content.Load<Texture2D>(@"Sprite2D\yellow_button"),
                    Content.Load<Texture2D>(@"Sprite2D\yellow_button_pressed")
                });
            }
        }

        public static List<Texture2D> CheckboxTextures
        {
            get
            {
                if (Content == null) return null;
                return _checkboxTextures ?? (_checkboxTextures = new List<Texture2D>
                {
                    Content.Load<Texture2D>(@"Sprite2D\checkbox_checked"),
                    Content.Load<Texture2D>(@"Sprite2D\checkbox")
                });
            }
        }

        public static List<Texture2D> SliderTextures
        {
            get
            {
                if (Content == null) return null;
                return _sliderTextures ?? (_sliderTextures = new List<Texture2D>
                {
                    Content.Load<Texture2D>(@"Sprite2D\slider"),
                    Content.Load<Texture2D>(@"Sprite2D\slider_marker")
                });
            }
        }

        public static SoundEffect ClickSoundEffect
        {
            get
            {
                if (Content == null) return null;
                return _clickSoundEffect ?? (_clickSoundEffect = Content.Load<SoundEffect>(@"SFX\rollover"));
            }
        }

        public static SpriteFont Font
        {
            get
            {
                if (Content == null) return null;
                return _font ?? (_font = Content.Load<SpriteFont>(@"Fonts\KenVector_Future"));
            }
        }

        public static Camera Camera { get; set; }
    }
}
