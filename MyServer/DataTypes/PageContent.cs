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
        public string UrlBase { get; set; }
        public string Content { get; set; }
        public string[] CssNames { get; set; }

        public PageContent(bool isAuthorized, Session currentSession, string urlBase, params string[] cssNames)
        {
            IsAuthorized = isAuthorized;
            CurrentSession = currentSession;
            UrlBase = urlBase;
            CssNames = cssNames;
        }
    }
}
