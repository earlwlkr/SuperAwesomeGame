using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame
{
    public abstract class GameEntity
    {
        public static float VerticalMoveSpeed = 30;
        private bool _isSlidingLeft;
        private bool _isSlidingRight;
        private float _newLeft;


        public virtual EntityState State { get; set; }

        public GameRectangle Area { get; set; }

        protected GameEntity()
        {
            Area = new GameRectangle();
            _isSlidingLeft = false;
            _isSlidingRight = false;
        }

        public virtual void SlideLeft(float newLeft)
        {
            _isSlidingLeft = true;
            _newLeft = newLeft;
        }

        public virtual void SlideRight(float newLeft)
        {
            _isSlidingRight = true;
            _newLeft = newLeft;
        }

        public virtual void Select(bool toggle)
        {
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime)
        {
            if (_isSlidingLeft)
            {
                if (Area.Left > _newLeft)
                {
                    Area.Left -= VerticalMoveSpeed;
                }
                else
                {
                    _isSlidingLeft = false;
                }
            }
            else if (_isSlidingRight)
            {
                if (Area.Left < _newLeft)
                {
                    Area.Left += VerticalMoveSpeed;
                }
                else
                {
                    _isSlidingRight = false;
                }
            }
        }
    }
}
