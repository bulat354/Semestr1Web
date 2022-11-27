using MyORM;
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
        [Key, Identity]
        public int Id { get; set; }
        public string Content { get; set; }
        public int AccountId { get; set; }
        public int ArticleId { get; set; }
        public DateTime Date { get; set; }

        [Hide]
        public Account Creator { get; set; }
    }

    public class Comments
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        public static IEnumerable<Comment> GetAll(int articleId)
        {
            return orm.Select<Comment>()
                .Where("ArticleId = @id", ("@id", articleId))
                .OrderByDescending("Date")
                .Go<Comment>();
        }

        public static Comment GetBy(int id)
        {
            return orm.Select<Comment>()
                .Where("Id = @id", ("@id", id))
                .Go<Comment>()
                .Single();
        }

        public static void Update(Comment comment, int id)
        {
            orm.Update(comment)
                .Where("Id = @id", ("@id", id))
                .Go();
        }

        public static IEnumerable<Comment> Insert(Comment comment)
        {
            return orm.InsertOutput(comment).Go<Comment>();
        }

        public static Comment LoadAccount(Comment comment)
        {
            comment.Creator = Accounts.GetAccountBy(comment.AccountId);

            return comment;
        }
    }
}
