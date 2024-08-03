using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.Serialization;

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
    /// <summary>
    /// folder path to download the appconfig.json program YdreamsSynchronizer
    /// </summary>
    [CanBeNull] public string pathFolderYdreamsSynchronizerDownload;

    private void Awake()
    {
        GetLocalData();
    }

    void GetLocalData()
    {
        var jsonPath = Path.Combine(pathFolderYdreamsSynchronizerDownload, "appconfig.json");
        var jsonText = File.ReadAllText(jsonPath);
        Debug.Log(jsonText);
        appConfig.SetJson(jsonText);
        appConfig.path = pathFolderYdreamsSynchronizerDownload;
    }
}

