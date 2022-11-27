using HtmlEngineLibrary;
using HtmlEngineLibrary.TemplateRendering;
using MyServer.DataTypes;
using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    public abstract class PageControllerBase : ControllerBase
    {
        protected void GenerateAndSend(string templatePath, PageContent content)
        {
            var service = new HtmlEngineService();
            content.Content = service.GetHtml(File.ReadAllText("templates/" + templatePath), content);
            base.GenerateAndSend("templates/base.html", content);
        }

        protected PageContent Init(PageContent content, string title, params string[] cssNames)
        {
            content.IsAuthorized = IsAuthorized;
            content.CurrentSession = CurrentSession;
            content.UrlBase = UrlBase;
            content.CssNames = cssNames;
            content.Title = title;

            return content;
        }

        protected class PageContent
        {
            public bool IsAuthorized { get; set; }
            public Session CurrentSession { get; set; }
            public string UrlBase { get; set; }
            public string Content { get; set; }
            public string[] CssNames { get; set; }
            public string Title { get; set; }
        }
    }
}
