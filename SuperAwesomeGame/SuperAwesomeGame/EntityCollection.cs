using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public EntityCollection GetEntitiesAtPos(float x, float y)
        {
            return new EntityCollection(_entities.FindAll(i => i.Area.Contains(x, y)));
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
