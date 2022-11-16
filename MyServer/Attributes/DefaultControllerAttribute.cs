using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultControllerAttribute : ApiControllerAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DefaultHttpMethodAttribute : HttpGetAttribute
    {

    }
}
