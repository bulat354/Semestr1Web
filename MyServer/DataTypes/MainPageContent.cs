using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    public class MainPageContent : PageContent
    {
        public Article[] Games { get; set; }
        public Article[] Articles { get; set; }

        public MainPageContent(bool isAuthorized, Session currentSession) : base(isAuthorized, currentSession)
        {
        }
    }
}
