using MyServer.Managers;
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
    }
}
