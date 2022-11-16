using HtmlEngineLibrary;
using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [DefaultController]
    public class MainPageController : ControllerBase
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        [DefaultHttpMethod]
        public IResult GetPage()
        {
            var model = new MainPageContent(IsAuthorized, CurrentSession);

            model.Games = orm
                .Select<Article>()
                .Where("Type = @type", ("@type", "game"))
                .Take(2)
                .OrderBy("LastDate")
                .Go<Article>()
                .ToArray();
            model.Articles = orm
                .Select<Article>()
                .Where("Type = @type", ("@type", "article"))
                .Take(2)
                .OrderBy("LastDate")
                .Go<Article>()
                .ToArray();

            return GenerateFile("index.html", model);
        }
    }
}
