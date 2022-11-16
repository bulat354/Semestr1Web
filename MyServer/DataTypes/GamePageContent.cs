using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    public class GamePageContent : PageContent
    {
        public Article Data { get; set; }

        public GamePageContent(bool isAuthorized, Session currentSession) : base(isAuthorized, currentSession)
        {
        }
    }
}
