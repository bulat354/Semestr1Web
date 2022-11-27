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
        public decimal Price { get; set; }
        public int ImageId { get; set; }
        public DateTime Date { get; set; }

        [Hide]
        public Image Image { get; set; }
    }

    public class GameInfos
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        private static GameInfo? GetSingle(string predicate, params (string, object)[] values)
        {
            return orm.Select<GameInfo>()
                .Where(predicate, values)
                .Go<GameInfo>()
                .SingleOrDefault();
        }

        public static GameInfo? GetGameBy(int id)
        {
            return GetSingle("Id = @id", ("@id", id));
        }

        public static IEnumerable<GameInfo> GetAll()
        {
            return orm.Select<GameInfo>()
                .OrderByDescending("Date")
                .Go<GameInfo>();
        }

        public static IEnumerable<GameInfo> GetAll(int count)
        {
            return orm.Select<GameInfo>()
                .Take(count)
                .OrderByDescending("Date")
                .Go<GameInfo>();
        }

        public static IEnumerable<GameInfo> GetAll(string text)
        {
            if (text == null || text.Length == 0)
                return GetAll();

            return orm.Select<GameInfo>()
                .Where($"Title LIKE '%{text}%'")
                .OrderByDescending("Date")
                .Go<GameInfo>();
        }

        public static GameInfo LoadImage(GameInfo game)
        {
            if (game != null)
                game.Image = Images.GetImageBy(game.ImageId);

            return game;
        }
    }
}
