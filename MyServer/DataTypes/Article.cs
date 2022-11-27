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
        [Key, Identity]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ImageId { get; set; }

        [Hide]
        public Image Image { get; set; }
    }

    public class Articles
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        private static Article? GetSingle(string predicate, params (string, object)[] values)
        {
            return orm.Select<Article>()
                .Where(predicate, values)
                .Go<Article>()
                .SingleOrDefault();
        }

        public static Article? GetArticleBy(int id)
        {
            return GetSingle("Id = @id", ("@id", id));
        }

        public static IEnumerable<Article> GetAll()
        {
            return orm.Select<Article>()
                .OrderByDescending("Date")
                .Go<Article>();
        }

        public static IEnumerable<Article> GetAll(int count)
        {
            return orm.Select<Article>()
                .Take(count)
                .OrderByDescending("Date")
                .Go<Article>();
        }

        public static IEnumerable<Article> GetAll(int count, int offset, string search)
        {
            return orm.Select<Article>()
                .Take(count)
                .Skip(offset)
                .Where($"Title LIKE '%{search}%'")
                .OrderByDescending("Date")
                .Go<Article>();
        }

        public static int GetCount(string search)
        {
            return (int)orm.Scalar()
                .Count<Article>()
                .Where($"Title LIKE '%{search}%'")
                .Go();
        }

        public static Article LoadImage(Article article)
        {
            if (article != null)
                article.Image = Images.GetImageBy(article.ImageId);

            return article;
        }
    }
}
