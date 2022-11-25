using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    [Table("Comments")]
    public class Comment
    {
        public string Content { get; set; }
        public int AccountId { get; set; }
        public int ArticleId { get; set; }
        public DateTime Date { get; set; }
    }
}
