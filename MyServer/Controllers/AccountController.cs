using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;
using MyServer.Results;

namespace MyServer.Controllers
{
    [ApiController("accounts")]
    public class AccountController : ControllerBase
    {
        [HttpGet("signin")]
        public IResult SignIn()
        {
            if (IsAuthorized)
                return ErrorResult.Unauthorized();

            return GenerateFile("html/signin.html", new PageContent(IsAuthorized, CurrentSession));
        }
    }
}