using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;

namespace MyServer.Controllers
{
    [DefaultController]
    internal class MainPageController : PageControllerBase
    {
        [DefaultHttpMethod]
        public void GetPage()
        {
            var content = new MainPageContent()
            {
                Games = GameInfos.GetAll(2).Select(GameInfos.LoadImage).ToArray(),
                Articles = Articles.GetAll(2).Select(Articles.LoadImage).ToArray()
            };
            GenerateAndSend("index.html", Init(content, "Добро пожаловать", "index"));
        }

        class MainPageContent : PageContent
        {
            public GameInfo[] Games;
            public Article[] Articles;
        }
    }
}
