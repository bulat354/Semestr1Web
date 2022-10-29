using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServer.TemplateEngines;
using MyServer.Attributes;

namespace MyServer.DataBaseConnection
{
    [ApiController("html")]
    public class GameInfo : ITemplateData
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Title", Title);
            dict.Add("Description", Description);
            dict.Add("ImageUrl", "../img/programmers.jpg");
            return dict;
        }

        [HttpGet("game")]
        public HttpResponse GetGame(string type, int id)
        {
            return TemplateEngine.MethodHandler("/html/game-page.html", new GameInfo() { Title = "OMAGAD", Description = "ETO RABOTAET!!!!!!" });
        }
    }
}
