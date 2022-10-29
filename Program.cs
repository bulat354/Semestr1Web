using MyServer.TemplateEngines;
using System.Data.SqlClient;
using MyServer.DataBaseConnection;

namespace MyServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Debug.StartDebugging(new HttpServer());
        }
    }
}