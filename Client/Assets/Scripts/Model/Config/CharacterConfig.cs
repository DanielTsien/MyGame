using MyGame.Proto;

namespace MyGame
{
    public class CharacterConfig
    {
        public int TID { get; set; }
        public string Name { get; set; }
        public CHARACTER_CLASS Class { get; set; }
        public string Resource { get; set; }
        public string Description { get; set; }

        //基本属性
        public int Speed { get; set; }
    }
}