using System.Net;

namespace MyServer.Managers
{
    public static class FileManager
    {
        public static bool MethodHandler(HttpListenerRequest request, HttpListenerResponse response)
        {
            var path = GetPath(request.RawUrl);

            if (File.Exists(path))
            {
                response.SendFile(path).SetStatusCode(HttpStatusCode.OK);
            }
            else
            {
                response.SetStatusCode(HttpStatusCode.NotFound);
                return false;
            }

            return true;
        }

        public static string GetFileText(string path)
        {
            return File.ReadAllText(path);
        }

        public static string GetPath(string? rawUrl)
        {
            if (rawUrl == null || rawUrl.Length < 2)
                return Configs.Instanse.LocalRoot + Configs.Instanse.DefaultFile;

            return Configs.Instanse.LocalRoot + rawUrl;
        }
    }
}
