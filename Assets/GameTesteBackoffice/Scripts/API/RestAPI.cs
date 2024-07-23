using UnityEngine;
using System.IO;
using JetBrains.Annotations;

public class RestAPI : MonoBehaviour
{
    public AppConfig appConfig;
    public class Config
    {
        public string ID;
        public string Name;
        public string ContentsURL;
        public string LastUpdated;
    }

    public class App
    {
        public string Tokens;
    }

    [CanBeNull] public string path;

    private void Awake()
    {
        GetLocalData();
    }

    void GetLocalData()
    {
        var jsonPath = Path.Combine(path, "appconfig.json");
        var jsonText = File.ReadAllText(jsonPath);
        Debug.Log(jsonText);
        appConfig.SetJson(jsonText);
        appConfig.path = path;
    }
}

