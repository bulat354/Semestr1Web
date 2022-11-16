﻿using MyServer.Attributes;
using MyServer.Controllers;
using MyServer.Results;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public object? Invoke(HttpListenerRequest request, HttpListenerResponse response)
        {
            NameValueCollection strParams;
            try
            {
                strParams = httpAttribute.Parse(request);
            }
            catch
            {
                return ErrorResult.BadRequest("Cannot parse giving input.");
            }
            object?[] objParams = new object?[parameters.Length];

            try
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var str = strParams[param.Name];
                    objParams[i] = Convert.ChangeType(strParams[param.Name], param.ParameterType);
                }
            }
            catch
            {
                return ErrorResult.NotFound("Cannot find method with the same parameter types");
            }

            var controller = Activator.CreateInstance(controllerType) as ControllerBase;
            controller.Init(request, response);

            return method.Invoke(controller, objParams);
        }
    }
}