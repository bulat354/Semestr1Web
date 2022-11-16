using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    public class PageContent
    {
        public bool IsAuthorized { get; set; }
        public Session CurrentSession { get; set; }

        public PageContent(bool isAuthorized, Session currentSession)
        {
            IsAuthorized = isAuthorized;
            CurrentSession = currentSession;
        }
    }
}
