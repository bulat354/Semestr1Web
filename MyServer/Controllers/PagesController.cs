using HtmlEngineLibrary;
using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Managers;
using MyServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [ApiController("html")]
    public class PagesController : ControllerBase
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");
    }
}
