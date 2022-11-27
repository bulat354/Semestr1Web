using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyServer
{
    [Serializable]
    public class Configs
    {
        private static readonly Configs _defaultConfigs = new Configs(8080, "./source", "/index.html");
        public static Configs Instanse { get; private set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }
        [JsonPropertyName("localRoot")]
        public string LocalRoot { get; set; }
        [JsonPropertyName("defaultFile")]
        public string DefaultFile { get; set; }

        [JsonConstructor]
        public Configs(int port, string localRoot, string defaultFile)
        {
            Port = port;
            LocalRoot = localRoot;
            DefaultFile = defaultFile;
        }

        public static Configs Load(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                Instanse = JsonSerializer.Deserialize<Configs>(json);
                Debug.ConfigsLoadedMsg();
                return Instanse;
            }
            else
            {
                var json = JsonSerializer.Serialize(_defaultConfigs);
                File.WriteAllText(jsonPath, json);
                Debug.ConfigFileNotFoundMsg(jsonPath);
                return _defaultConfigs;
            }
        }
    }
}
