using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.TemplateEngines
{
    public interface ITemplateData
    {
        Dictionary<string, string> ToDictionary();
    }
}
