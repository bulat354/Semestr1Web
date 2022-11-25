using MyORM;
using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    [Table("Games")]
    public class GameInfo
    {
        [Key, Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ImageId { get; set; }
        public DateTime Date { get; set; }

        [Hide]
        public Image Image { get; set; }
    }
}
