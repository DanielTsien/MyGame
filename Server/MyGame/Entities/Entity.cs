using MyGame.Proto;
using MyGame.Utils;

namespace MyGame.Entities
{
    internal class Entity
    {
        public int EntityId => EntityData.Id;


        public NEntity EntityData
        {
            get
            {
                return EntityData;
            }
            set 
            { 
                EntityData = value;
                SetEntityData(value);
            }
        }

        public Vector3Int Position 
        {
            get 
            {
                return Position;
            }
            set 
            {
                Position = value;
                EntityData.Position = value;
            }
        }

        public Vector3Int Direction
        {
            get
            {
                return Direction;
            }
            set
            {
                Direction = value;
                EntityData.Direction = value;
            }
        }

        public int Speed
        {
            get
            {
                return Speed;
            }
            set
            {
                Speed = value;
                EntityData.Speed = value;
            }
        }

        public Entity(NEntity entityData)
        {
            EntityData = entityData;
        }

        public void SetEntityData(NEntity entity)
        {
            Position = entity.Position;
            Direction = entity.Direction;
            Speed = entity.Speed;
        }
    }
}
