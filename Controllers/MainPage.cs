using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.Attributes;
using MyServer.TemplateEngines;

namespace MyServer.Controllers
{
    [FileController("/html/game-page.html")]
    public class GamePage
    {
        [HttpGet]
        public Dictionary<string, string> GetPage()
        {
            return new Dictionary<string, string>()
            {
                { "Title", "War of Gods" }
            };
        }
    }
}
