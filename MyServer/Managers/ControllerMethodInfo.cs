using MyServer.Attributes;
using MyServer.Controllers;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Web;

namespace MyServer.Managers
{
    internal class ControllerMethodInfo
    {
        public readonly string Name;
        public readonly bool IsVoid;

        private readonly Type controllerType;
        private readonly MethodInfo method;
        private readonly HttpMethodAttribute httpAttribute;
        private readonly ParameterInfo[] parameters;

        public ControllerMethodInfo(Type type, MethodInfo method)
        {
            if (Attribute.IsDefined(type, typeof(ApiControllerAttribute)))
            {
                controllerType = type;

                var controllerName = type.GetCustomAttribute<ApiControllerAttribute>().Name?.ToLower();
                if (controllerName == null || controllerName.Length == 0)
                    controllerName = type.Name.ToLower();

                if (Attribute.IsDefined(method, typeof(HttpMethodAttribute)))
                {
                    this.method = method;
                    httpAttribute = method.GetCustomAttribute<HttpMethodAttribute>();
                    parameters = method.GetParameters();

                    var methodName = httpAttribute.Name.ToLower();
                    if (methodName.StartsWith('.'))
                        methodName = method.Name.ToLower() + methodName;
                    else if (methodName == null)
                        methodName = method.Name.ToLower() + ".get";

                    Name = $"{controllerName}.{methodName}";
                    IsVoid = method.ReturnType == typeof(void);
                }
                else
                    throw new ArgumentException("Method that was given doesn't have attribute HttpMethod");
            }
            else
                throw new ArgumentException("Controller that was given doesn't have attribute HttpController");
        }

        public static IEnumerable<ControllerMethodInfo> GetMethods(Type controller)
        {
            return controller.GetMethods()
                .Where(m => Attribute.IsDefined(m, typeof(HttpMethodAttribute)))
                .Select(m => new ControllerMethodInfo(controller, m));
        }

        public void Invoke(HttpListenerRequest request, HttpListenerResponse response)
        {
            NameValueCollection strParams;
            try
            {
                strParams = httpAttribute.Parse(request);
            }
            catch
            {
                response.SetStatusCode(HttpStatusCode.BadRequest); 
                return;
            }
            object?[] objParams = new object?[parameters.Length];

            try
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var str = WebUtility.UrlDecode(strParams[param.Name]);
                    objParams[i] = Convert.ChangeType(str, param.ParameterType);
                }
            }
            catch
            {
                response.SetStatusCode(HttpStatusCode.BadRequest);
                return;
            }

            var controller = Activator.CreateInstance(controllerType) as ControllerBase;
            controller.Init(request, response);

            var result = method.Invoke(controller, objParams);

            if (!IsVoid && result != null)
            {
                response.SendJson(result).SetStatusCode(HttpStatusCode.OK);
            }
        }
    }
}
