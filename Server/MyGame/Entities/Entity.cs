using MyGame.Proto;
using MyGame.Utils;

namespace MyGame.Entities
{
    internal class Entity
    {
        public int EntityId => EntityData.Id;


        private NEntity m_entityData;
        public NEntity EntityData
        {
            get
            {
                return m_entityData;
            }
            set 
            {
                m_entityData = value;
                SetEntityData(value);
            }
        }

        private Vector3Int m_position;
        public Vector3Int Position 
        {
            get 
            {
                return m_position;
            }
            set 
            {
                m_position = value;
                EntityData.Position = value;
            }
        }

        private Vector3Int m_dirction;
        public Vector3Int Direction
        {
            get
            {
                return m_dirction;
            }
            set
            {
                m_dirction = value;
                EntityData.Direction = value;
            }
        }

        private int m_speed;
        public int Speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_speed = value;
                EntityData.Speed = value;
            }
        }

        public Entity(Vector3Int pos, Vector3Int dir)
        {
            m_entityData = new NEntity();
            m_entityData.Position = pos;
            m_entityData.Direction = dir;
            SetEntityData(m_entityData);
        }


        public Entity(NEntity entityData)
        {
            m_entityData = entityData;
        }

        public void SetEntityData(NEntity entity)
        {
            Position = entity.Position;
            Direction = entity.Direction;
            Speed = entity.Speed;
        }
    }
}
