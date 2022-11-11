using System;
using System.Collections.Generic;
using EntityComponent;
using UnityEngine;

namespace Entities
{
    public class Entity
    {
        public int EntityId;
        private Dictionary<Type, IComponent> m_components = new();
        public Entity(int entityId)
        {
            EntityId = entityId;
        }

        public void AddComponent(IComponent component)
        {
            var type = component.GetType();
            if (!m_components.ContainsKey(type))
            {
                m_components[type] = component;
            }
            else
            {
                Debug.LogError($"Entity({EntityId}) already has component({type.Name})");
            }
        }
    }
}