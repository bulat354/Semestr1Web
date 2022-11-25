using MyORM;
using MyServer.Attributes;
using MyServer.DataTypes;
using MyServer.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [ApiController("data")]
    public class DataController : ControllerBase
    {
        private static MiniORM orm = new MiniORM("GavnoGamesDB");
    }
}
