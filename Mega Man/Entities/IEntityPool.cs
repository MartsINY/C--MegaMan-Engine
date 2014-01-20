using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaMan.Engine.Entities
{
    public interface IEntityPool
    {
        IEntity CreateEntity(string name);
        IEntity CreateEntityWithId(string id, string name);
        IEntity GetEntityById(string id);
        Int32 GetNumberAlive(string name);
        Int32 GetTotalAlive();
        IEnumerable<IEntity> GetAll();
        void RemoveAll();
    }
}
