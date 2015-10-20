using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperAwesomeGame.Common;

namespace SuperAwesomeGame
{
    public class EntityCollection
    {
        private readonly List<GameEntity> _entities;

        public EntityCollection()
        {
            _entities = new List<GameEntity>();
        }

        public EntityCollection(List<GameEntity> entities)
        {
            _entities = entities;
        }

        public void Add(GameEntity entity)
        {
            _entities.Add(entity);
        }

        public void Remove(GameEntity entity)
        {
            _entities.Remove(entity);
        }

        public bool Contains(GameEntity entity)
        {
            return _entities.Contains(entity);
        }

        public bool IsEmpty()
        {
            return _entities.Count == 0;
        }

        public EntityCollection GetEntitiesAtPos(float x, float y)
        {
            Vector2 screenPos = Utils.ScreenToWorld(new Vector2(x, y));
            return new EntityCollection(_entities.FindAll(i => i.Area.Contains(screenPos)));
        }

        public void Select(bool toggle)
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                _entities[i].Select(toggle);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(gameTime, spriteBatch);
            }
        }
    }
}
