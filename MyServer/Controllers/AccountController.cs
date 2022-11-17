using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;
using MyServer.Results;

namespace MyServer.Controllers
{
    [ApiController("accounts")]
    public class AccountController : ControllerBase
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");

        [HttpGet("signin")]
        public IResult SignIn()
        {
            if (IsAuthorized)
                return ErrorResult.NotFound();

            return GenerateFile("html/signin.html", new PageContent(IsAuthorized, CurrentSession));
        }

        [HttpPost("login")]
        public IResult Login(string email, string password, string remember = "off")
        {
            if (!IsAuthorized)
            {
                var accounts = orm
                    .Select<Account>()
                    .Where("Email = @email AND Password = @pass", ("@email", email), ("@pass", password))
                    .Go<Account>()
                    .ToList();

                if (accounts.Count > 0)
                {
                    var rem = remember == "on";
                    var acc = accounts[0];

                    var session = SessionManager.Instance.CreateSession(acc.Id, email, rem ? SessionManager.GetSlidingPolicy(1) : null);
                    _response.SetCookie(new Cookie("SessionId", session.Id, "/"));

                    _response.Redirect("../");
                    return new RedirectResult();
                }

                return (ObjectResult<string>)"Неверный логин или пароль";
            }

            return ErrorResult.NotFound();
        }

        [HttpGet("signup")]
        public IResult SignUp()
        {
            if (IsAuthorized)
                return ErrorResult.NotFound();

            return GenerateFile("html/signup.html", new PageContent(IsAuthorized, CurrentSession));
        }

        [HttpPost("add")]
        public IResult AddAccount
            (
            string surname, string name, string birthdate, string gender, string level, 
            string nickname, string email, string tel, string newpass, string acceptpass
            )
        {
            if (IsAuthorized)
                return ErrorResult.NotFound();

            var account = new Account()
            {
                Surname = surname,
                Name = name,
                Nickname = nickname,
                Email = email,
                Tel = tel,
                Password = newpass,
                Gender = gender,
                Level = level,
                BirthDate = DateTime.Parse(birthdate)
            };

            if (newpass == acceptpass && account.IsCorrect())
            {
                try
                {
                    orm.Insert(account)
                        .Go();
                    _response.Redirect("../accounts/signin");
                    return new RedirectResult();
                }
                catch
                {
                    return ErrorResult.BadRequest();
                }
            }
            return ErrorResult.BadRequest();
        }
    }
}