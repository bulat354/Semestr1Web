using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServer.Attributes;
using MyServer.Managers;
using MyServer.Results;

namespace MyServer.Controllers
{
    [ApiController("accounts")]
    public class AccountController : ControllerBase
    {
        private bool TryGetCurrentAccountId(out int result)
        {
            var manager = SessionManager.Instance;
            var cookie = _request.Cookies["SessionId"];
            if (cookie != null)
            {
                var guid = cookie.Value;
                if (manager.CheckSession(guid))
                {
                    var session = manager.GetSession(guid);
                    result = session.AccountId;
                    return true;
                }
            }

            result = -1;
            return false;
        }
    }
}