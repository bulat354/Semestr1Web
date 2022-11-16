using HtmlEngineLibrary;
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
        [DefaultHttpMethod]
        public IResult GetPage()
        {
            var service = new HtmlEngineService();
            var template = File.ReadAllText("templates/index.html");
            var model = new MainPageContent(IsAuthorized, CurrentSession);

            model.Games = new Article[]
            {
                new Article()
                {
                    CreatorId = 1,
                    Description = "",
                    FirstDate = DateTime.Now,
                    Id = 1,
                    ImageColorHex = "#000000",
                    ImageUrl = "../img/programmers.jpg",
                    LastDate = DateTime.Now,
                    Title = "first",
                    Type = "game"
                }
            };
            model.Articles = new Article[]
            {
                new Article()
                {
                    CreatorId = 1,
                    Description = "",
                    FirstDate = DateTime.Now,
                    Id = 2,
                    ImageColorHex = "#000000",
                    ImageUrl = "../img/programmers.jpg",
                    LastDate = DateTime.Now,
                    Title = "second",
                    Type = "game"
                }
            };

            service.GenerateAndSaveInDirectory("generated", "index.html", template, model);
            return new FileResult("generated/index.html");
        }
    }
}
