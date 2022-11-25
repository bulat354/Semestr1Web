﻿using MyORM;
using MyORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    [Table("Articles")]
    public class Article
    {
        [Key, Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime FirstDate { get; set; }
        public int ImageId { get; set; }

        [Hide]
        public Image Image { get; set; }
    }
}
