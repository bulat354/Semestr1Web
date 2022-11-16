using MyORM;
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
        private static MiniORM orm = new MiniORM();

        [Key, Identity]
        public int Id { get; set; }
        public string Type { get; set; }
        [Immutable]
        public int CreatorId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageColorHex { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Immutable]
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }

        #region With ORM only
        public static List<Article> GetAll()
        {
            return orm
                .Select<Article>()
                .Go<Article>()
                .ToList();
        }
        public static List<Article> GetAllByType(string type)
        {
            return orm
                .Select<Article>()
                .Where("Type = @type", ("@type", type))
                .Go<Article>()
                .ToList();
        }
        public static List<Article> GetByType(string type, int count)
        {
            return orm
                .Select<Article>()
                .Where("Type = @type", ("@type", type))
                .Take(count)
                .Go<Article>()
                .ToList();
        }
        public static List<Article> Search(string titlePart, string type)
        {
            return orm
                .Select<Article>()
                .Where("Type = @type AND Title LIKE @pattern", ("@type", type), ("@pattern", $"%{titlePart}%"))
                .Go<Article>()
                .ToList();
        }
        public static Article? GetArticleByIdAndType(int id, string type)
        {
            return orm
                .Select<Article>()
                .Where("Id = @id AND Type = @type", ("@id", id), ("@type", type))
                .Go<Article>()
                .SingleOrDefault();
        }
        public static bool Insert(Article article)
        {
            return orm
                .Insert(article)
                .Go() > 0;
        }
        public static bool Update(Article article)
        {
            return orm
                .Update(article)
                .Where("Id = @id", ("@id", article.Id))
                .Go() > 0;
        }
        #endregion
    }
}
