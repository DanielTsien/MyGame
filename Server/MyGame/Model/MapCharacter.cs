using MyGame.Entities;
using MyGame.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Model
{
    internal class MapCharacter
    {
        public NetConnection Sender;
        public Character Character;

        public MapCharacter(NetConnection sender, Character character)
        {
            Sender = sender;
            Character = character;
        }
    }
}
