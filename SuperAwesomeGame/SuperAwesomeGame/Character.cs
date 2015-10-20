﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame
{
    public class Character : GameEntity
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;

        private Vector2 _center;
        private const int Radius = 50;
        private float _multiplier;
        private int _characterType = 0;
        private readonly int[,] _listIndex = {{0, 1, 2}, {3, 4, 5}, {6, 7, 8}}; 
        private int _currentIndex;
        private TimeSpan _lastTotalGameTime = new TimeSpan(0);
        private bool _clicked = false;

        public Character(float left, float top, int type)
        {
            Area.Left = left - TileWidth / 2;
            Area.Top = top - TileHeight / 2;
            Area.Width = TileWidth;
            Area.Height = TileHeight;
            _center = new Vector2(left - TileWidth / 2, top - TileHeight / 2 - Radius);
            _characterType = type % 3;

            _currentIndex = 0;
        }

        public void ChangeCharacterType()
        {
            _characterType = (_characterType + 1) % 3;
        }

        public override void Select(bool toggle)
        {
            if (!toggle && _clicked)
            {
                State = State == EntityState.Selected ? EntityState.Default : EntityState.Selected;
            }

            if (toggle && !_clicked)
            {
                _clicked = true;
            }
            else if (!toggle)
            {
                _clicked = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            var interval = gameTime.TotalGameTime.Subtract(_lastTotalGameTime).Milliseconds;
            if (interval > 500)
            {
                _currentIndex = (_currentIndex + 1) % 3;
                _multiplier += 20f;
                Area.Left = _center.X + (float)Math.Sin((Math.PI / 180) * _multiplier) * Radius;
                Area.Top = _center.Y + (float)Math.Cos((Math.PI / 180) * _multiplier) * Radius;

                _lastTotalGameTime = gameTime.TotalGameTime;
            }
            
            base.Update(gameTime);
        }

        public Rectangle GetSourceRectangle(int tileIndex)
        {
            var tileY = tileIndex / (Manager.CharacterTextureSet.Width / TileWidth);
            var tileX = tileIndex % (Manager.CharacterTextureSet.Width / TileWidth);

            return new Rectangle(tileX * TileWidth, tileY * TileHeight, TileWidth, TileHeight);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Manager.CharacterTextureSet,
                new Rectangle((int)Area.Left, (int)Area.Top, TileWidth, TileHeight),
                GetSourceRectangle(_listIndex[_characterType, _currentIndex]),
                State == EntityState.Default ? Color.White : Color.Blue,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.1f);
        }
    }
}