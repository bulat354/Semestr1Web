using System.Data.SqlClient;
using System.Text.Json;
using System.Linq.Expressions;
using System.IO;
using System.Collections;
using MyORM;
using MyServer.DataTypes;

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