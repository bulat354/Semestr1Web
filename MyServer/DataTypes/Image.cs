using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    [Table("Images")]
    public class Image
    {
        [Key, Identity]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public Image(int id, string name, string colorHex)
        {
            Id = id;
            Name = name;
            ColorHex = colorHex;
        }
    }
}
