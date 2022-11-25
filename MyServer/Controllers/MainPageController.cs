using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;
using MyServer.Results;

namespace MyServer.Controllers
{
    [DefaultController]
    internal class MainPageController : ControllerBase
    {
        [DefaultHttpMethod]
        public IResult GetPage()
        {
            var model = CreateContent();
            model.Games = new[]
            {
                new GameInfo()
                {
                    Image = new Image(1, "war-of-god.jpg", "#000000"),
                    Description = "Description",
                    Title = "War of Gods",
                    Id = 1
                }
            };
            model.Articles = new[]
            {
                new Article()
                {
                    Image = new Image(1, "top10.jpg", "#000000"),
                    Title = "Top10",
                    Id = 1
                }
            };
            model.Content = GeneratePageContent(model, "index.html");
            return GeneratePage(model);
        }

        private MainPageContent CreateContent()
        {
            return new MainPageContent(IsAuthorized, CurrentSession, UrlBase, "index");
        }

        class MainPageContent : PageContent
        {
            public GameInfo[] Games;
            public Article[] Articles;

            public MainPageContent(bool isAuthorized, Session currentSession, string urlBase, params string[] cssNames) 
                : base(isAuthorized, currentSession, urlBase, cssNames) { }
        }
    }
}
