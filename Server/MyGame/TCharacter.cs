//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyGame
{
    using System;
    using System.Collections.Generic;
    
    public partial class TCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Class { get; set; }
        public int Level { get; set; }
        public int ConfigId { get; set; }
        public int MapId { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }
    
        public virtual TPlayer TPlayer { get; set; }
    }
}
