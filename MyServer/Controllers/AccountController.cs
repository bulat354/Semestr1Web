using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;

namespace MyServer.Controllers
{
    [ApiController("accounts")]
    public class AccountController : ControllerBase
    {
        [HttpGet("login")]
        public bool Login(string email, string password, string remember = "off")
        {
            var account = Accounts.GetAccountBy(email, password);
            if (account == null)
            {
                return false;
            }

            var toremember = remember == "on";
            var session = SessionManager.Instance.CreateSession(account.Id,
                toremember ? SessionManager.GetSlidingPolicy(60) : SessionManager.GetSessionPolicy());
            AddCookie("SessionId", session.Id, toremember ? 60 : 0);
            return true;
        }

        [HttpPost("signup")]
        public bool SignUp(string name, string gender, string email, string password, string acceptpassword)
        {
            if (password != acceptpassword)
                return false;

            var account = new Account()
            {
                Name = name,
                Gender = gender,
                Email = email,
                Password = password
            };

            if (!account.IsCorrect())
                return false;

            try
            {
                Accounts.InsertAccount(account);
            }
            catch
            {
                return false;
            }

            return true;
        }

        [HttpPost("update")]
        public bool Update(string name, string gender, string email, string password, string acceptpassword)
        {
            if (!IsAuthorized)
            {
                Unauthorized();
                return false;
            }

            if (password != acceptpassword)
                return false;

            var account = new Account()
            {
                Name = name,
                Gender = gender,
                Email = email,
                Password = password
            };

            if (!account.IsCorrect())
                return false;

            try
            {
                Accounts.Update(account, CurrentSession.AccountId);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}