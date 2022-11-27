using MyORM;
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
        public string Alt { get; set; }
    }

    public class Images
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        private static Image? GetSingle(string predicate, params (string, object)[] values)
        {
            return orm.Select<Image>()
                .Where(predicate, values)
                .Go<Image>()
                .SingleOrDefault();
        }

        public static Image? GetImageBy(int id)
        {
            return GetSingle("Id = @id", ("@id", id));
        }
    }
}
