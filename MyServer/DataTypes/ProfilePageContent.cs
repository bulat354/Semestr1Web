using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    public class ProfilePageContent : PageContent
    {
        public Account Data { get; set; }

        public ProfilePageContent(bool isAuthorized, Session currentSession) : base(isAuthorized, currentSession)
        {
        }
    }
}
