using HtmlEngineLibrary;
using HtmlEngineLibrary.TemplateRendering;
using MyServer.DataTypes;
using MyServer.Managers;
using MyServer.Results;
using System.Net;

namespace MyServer.Controllers
{
    public abstract class ControllerBase
    {
        protected HttpListenerResponse _response;
        protected HttpListenerRequest _request;

        public void Init(HttpListenerRequest request, HttpListenerResponse response)
        {
            _request = request;
            _response = response;
        }

        protected Session CurrentSession
        {
            get
            {
                if (IsAuthorized)
                {
                    var cookie = _request.Cookies["SessionId"];
                    return SessionManager.Instance.GetSession(cookie.Value);
                }

                return null;
            }
        }
        protected bool IsAuthorized
        {
            get
            {
                var cookie = _request.Cookies["SessionId"];

                return cookie != null && SessionManager.Instance.CheckSession(cookie.Value);
            }
        }
        protected string UrlBase
        {
            get
            {
                return "http://" + _request.UserHostName;
            }
        }

        protected FileResult GeneratePage(PageContent model)
        {
            new HtmlEngineService().GenerateAndSaveInDirectory("generated", "page.html", File.ReadAllText("templates/base.html"), model);
            return new FileResult("generated/page.html");
        }

        protected string GeneratePageContent(object model, string fileName)
        {
            return new HtmlEngineService().GetHtml(File.ReadAllText("templates/" + fileName), model);
        }
    }
}
