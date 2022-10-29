using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyServer.Attributes;
using System.Net;

namespace MyServer.Controllers
{
    public class FileMethodInfo
    {
        public readonly string Name;

        private readonly object controller;
        private readonly MethodInfo method;
        private readonly HttpGetAttribute httpAttribute;
        private readonly ParameterInfo[] parameters;

        public FileMethodInfo(object? controller, Type type, MethodInfo method)
        {
            if (Attribute.IsDefined(type, typeof(FileControllerAttribute)))
            {
                this.controller = controller;

                var controllerName = type.GetCustomAttribute<FileControllerAttribute>().FileName.ToLower();
                if (controllerName == null)
                    throw new ArgumentNullException();

                if (Attribute.IsDefined(method, typeof(HttpGetAttribute)))
                {
                    this.method = method;
                    httpAttribute = method.GetCustomAttribute<HttpGetAttribute>();
                    parameters = method.GetParameters();

                    Name = $"{controllerName}.{string.Join('.', parameters.Select(x => x.Name))}";
                }
                else
                    throw new ArgumentException("Method that was given doesn't have attribute HttpGet");
            }
            else
                throw new ArgumentException("Controller that was given doesn't have attribute FileController");
        }

        public static IEnumerable<FileMethodInfo> GetMethods(Type controller)
        {
            var obj = Activator.CreateInstance(controller);
            return controller.GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(HttpGetAttribute)) && m.ReturnType == typeof(Dictionary<string, string>))
                .Select(m => new FileMethodInfo(obj, controller, m));
        }

        public Dictionary<string, string> Invoke(HttpListenerRequest request)
        {
            var strParams = httpAttribute.Parse(request);

            object?[] objParams = new object?[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                var str = strParams[param.Name];
                if (str == null)
                    return null;
                objParams[i] = Convert.ChangeType(strParams[param.Name], param.ParameterType);
            }

            return (Dictionary<string, string>)method.Invoke(controller, objParams);
        }
    }
}
