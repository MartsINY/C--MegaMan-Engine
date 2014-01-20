using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaMan.Engine.Entities
{
    public class SceneEntityPoolDecorator : IEntityPool
    {
        private IEntityPool _basePool;

        private readonly List<IEntity> _additionalEntities;

        public SceneEntityPoolDecorator(IEntityPool basePool)
        {
            _basePool = basePool;
            _additionalEntities = new List<IEntity>();
        }

        public IEntity CreateEntity(string name)
        {
            var entity = _basePool.CreateEntity(name);
            _additionalEntities.Add(entity);
            return entity;
        }

        public IEntity CreateEntityWithId(string id, string name)
        {
            var entity = _basePool.CreateEntityWithId(id, name);
            _additionalEntities.Add(entity);
            return entity;
        }

        public IEntity GetEntityById(string id)
        {
            return _basePool.GetEntityById(id);
        }

        public int GetNumberAlive(string name)
        {
            return _basePool.GetNumberAlive(name);
        }

        public int GetTotalAlive()
        {
            return _basePool.GetTotalAlive();
        }

        public IEnumerable<IEntity> GetAll()
        {
            return _basePool.GetAll();
        }

        public void RemoveAll()
        {
            foreach (var entity in _additionalEntities)
                entity.Remove();

            _additionalEntities.Clear();
        }
    }
}
