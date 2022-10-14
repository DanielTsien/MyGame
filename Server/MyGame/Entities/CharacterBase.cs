using MyGame.Data;
using MyGame.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Entities
{
    internal class CharacterBase : Entity

    {
        public int Id { get; set; }
        public string Name => Info.Name;
        public CharacterInfo Info;
        public CharacterDefine Define;

        public CharacterBase(NEntity entityData) : base(entityData)
        {

        }
    }
}
