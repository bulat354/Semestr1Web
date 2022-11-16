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
            var service = new HtmlEngineService();
            var template = File.ReadAllText("templates/index.html");
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

            service.GenerateAndSaveInDirectory("generated", "index.html", template, model);
            return new FileResult("generated/index.html");
        }
    }
}
