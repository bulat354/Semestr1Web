using MyServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.DataTypes
{
    public class SearchPageContent : PageContent
    {
        public SearchPageContent(bool isAuthorized, Session currentSession) : base(isAuthorized, currentSession)
        {
        }

        public Article[] SearchResults { get; set; }
        public string SearchString { get; set; }
        public string SearchType { get; set; }
    }
}
