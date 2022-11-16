using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Results
{
    public class NextResult : IResult
    {
        public string GetContentType() => null;

        public byte[] GetResult() => null;

        public HttpStatusCode GetStatusCode() => HttpStatusCode.NotImplemented;
    }
}
