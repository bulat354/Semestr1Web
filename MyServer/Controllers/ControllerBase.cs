using HtmlEngineLibrary;
using MyServer.DataTypes;
using MyServer.Managers;
using System.Net;

namespace MyServer.Controllers
{
    public abstract class ControllerBase
    {
        protected HttpListenerResponse _response;
        protected HttpListenerRequest _request;

        protected Session CurrentSession { get; private set; }
        protected bool IsAuthorized { get { return CurrentSession != null; } }
        protected string UrlBase { get; private set; }

        #region Initializing
        public void Init(HttpListenerRequest request, HttpListenerResponse response)
        {
            _request = request;
            _response = response;

            CurrentSession = GetCurrentSession();
            UrlBase = "http://" + _request.UserHostName;
        }

        private Session GetCurrentSession()
        {
            var cookie = _request.Cookies["SessionId"];

            if (cookie != null && SessionManager.Instance.CheckSession(cookie.Value))
            {
                return SessionManager.Instance.GetSession(cookie.Value);
            }

            return null;
        }
        #endregion

        #region Response Sending
        protected void Ok(object obj)
        {
            _response.SetStatusCode(HttpStatusCode.OK).SendJson(obj);
        }
        protected void Ok(string text)
        {
            _response.SetStatusCode(HttpStatusCode.OK).SendTextPlain(text);
        }
        protected void Ok(string text, string extension)
        {
            _response.SetStatusCode(HttpStatusCode.OK).SendFile(text, extension);
        }
        protected void Redirect(string url)
        {
            _response.Redirect(url);
        }
        protected void GenerateAndSend(string templatePath, object model)
        {
            var service = new HtmlEngineService();
            var buffer = service.GetHtmlInBytes(File.ReadAllText(templatePath), model);
            _response.SetStatusCode(HttpStatusCode.OK).SendBytes(buffer).SetContentType(Path.GetExtension(templatePath));
        }
        protected void NotFound()
        {
            _response.SetStatusCode(HttpStatusCode.NotFound);
        }
        protected void Unauthorized()
        {
            _response.SetStatusCode(HttpStatusCode.Unauthorized);
        }
        protected void BadRequest()
        {
            _response.SetStatusCode(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Other
        protected void AddCookie(string name, string value, int minutes = 0)
        {
            var headerName = "Set-Cookie";
            var headerValue = $"{name}={value}; Path=/;" + (minutes > 0 ? $" Max-Age={minutes * 60}" : "");
            _response.AddHeader(headerName, headerValue);
        }

        protected void RemoveCookie(string name)
        {
            var headerName = "Set-Cookie";
            var headerValue = $"{name}=; Path=/; Max-Age=0";
            _response.AddHeader(headerName, headerValue);
        }
        #endregion
    }
}
