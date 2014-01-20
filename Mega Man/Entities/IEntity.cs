﻿using MegaMan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaMan.Engine.Entities
{
    public interface IEntity
    {
        string Name { get; }
        IGameplayContainer Container { get; }
        ITiledScreen Screen { get; }
        IEntityPool Entities { get; }
        bool IsGravitySensitive { get; }
        bool Paused { get; set; }
        IEntity Parent { get; set; }
        Direction Direction { get; set; }

        event Action Removed;
        event Action Death;

        T GetComponent<T>() where T : Component;
        Component GetOrCreateComponent(string name);
        IEntity Spawn(string entityName);
        void Remove();
        void Die();
        void SendMessage(IGameMessage message);
        void Start(IGameplayContainer container);
        void Stop();
    }
}
