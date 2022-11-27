using System.Net;
using System.Text;
using MyServer.Managers;

namespace MyServer
{
    public class HttpServer : IDisposable
    {
        public static string configFileName { get; private set; } = "serverconfig.json";

        private HttpListener _listener;
        private Thread _listenerThread;
        private CancellationTokenSource _src = new CancellationTokenSource();

        public bool IsWorking;

        public HttpServer()
        {
            _listener = new HttpListener();
            Debug.ListenerCreatedMsg();

            Start();
        }

        public void Start()
        {
            if (IsWorking)
            {
                Debug.AlreadyStartedMsg();
                return;
            }

            StartQuietly();
        }

        private void StartQuietly()
        {
            IsWorking = true;
            _listenerThread = new Thread(() => Task.Run(Listen, _src.Token));
            _listenerThread.Start();

            ControllerManager.Init();
        }

        private async void Listen()
        {
            lock (_listener)
            {
                var configs = Configs.Load(Path.GetFullPath(configFileName));
                _listener.Prefixes.Add($"http://localhost:{configs.Port}/");
                _listener.Start();
            }
            Debug.ListenerStartedMsg();

            while (_listener.IsListening)
            {
                Debug.ListenerIsListeningMsg();
                HttpListenerContext context;
                try
                {
                    context = await _listener.GetContextAsync();
                }
                catch
                {
                    return;
                }
                
                var request = context.Request;
                var response = context.Response;

                try
                {
                    Debug.RequestReceivedMsg(request.Url.ToString());
                    
                    if (!ControllerManager.DefaultMethodHandler(request, response))
                    {
                        if (!FileManager.MethodHandler(request, response))
                        {
                            ControllerManager.MethodHandler(request, response);
                        }
                    }
                }
                catch
                {
                    response.SetStatusCode(HttpStatusCode.InternalServerError);
                }

                response.ContentEncoding = Encoding.UTF8;

                Debug.ResponseSendedMsg(response.StatusCode);

                response.Close();
            }
        }

        public void Stop()
        {
            if (!IsWorking)
            {
                Debug.AlreadyStoppedMsg();
                return;
            }

            StopQuietly();
            Debug.ListenerStoppedMsg();
        }

        private void StopQuietly()
        {
            IsWorking = false;
            _listener.Stop();
            _listenerThread.Join();
        }

        public void Restart()
        {
            Debug.RestartMsg();
            if (IsWorking)
            {
                StopQuietly();
                StartQuietly();
            }
            else
            {
                StartQuietly();
            }
        }

        public void Close()
        {
            Stop();
            _listener.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
