using HtmlEngineLibrary;
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

        protected IResult GenerateFile(string templatePath, object model)
        {
            var service = new HtmlEngineService();
            var template = File.ReadAllText("templates/" + templatePath);

            service.GenerateAndSaveInDirectory("generated", templatePath, template, model);
            return new FileResult("generated/" + templatePath);
        }
    }
}
