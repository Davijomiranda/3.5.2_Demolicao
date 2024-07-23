using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AppConfig : MonoBehaviour
{
    public static RootObject root;
    public string path;
    public RootObject rootInspector;
    public int idBracelet;
    public string pathJsonBracelet;
    public string nameArchiveBracelet;
    public BraceletModel braceletModel;

    public void SetIdBracelet(int i)
    {
        idBracelet = i;
    }

    public void SetJson(string json)
    {
        root = JsonUtility.FromJson<RootObject>(json);

        var jsonObject = JSON.Parse(json);
        var tokens = jsonObject["app"]["tokens"];
        var content = jsonObject["app"]["contents"];
        var adcontent = jsonObject["app"]["advcontents"];
        var variables = jsonObject["app"]["variables"];
        var configfiles = jsonObject["app"]["configfiles"];

        foreach (var key in tokens.Keys)
        {
            var value = tokens[key];
            //Debug.Log($"Token Key: {key}, Value: {value}");
            root.app.SetupToken(key, value.ToString());
        }

        foreach (var key in content.Keys)
        {
            var value = content[key];
            //Debug.Log($"Content Key: {key}, Value: {value}");
            root.app.SetupContents(key, value.ToString());
        }

        foreach (var adKey in adcontent.Keys)
        {
            var adValue = adcontent[adKey];
            //Debug.Log($"AdvContent Key: {adKey}, Value: {adValue}");
            root.app.SetupAdvContents(adKey,adValue);
        }

        foreach (var key in variables.Keys)
        {
            var value = variables[key];
            //Debug.Log($"Variable Key: {key}, Value: {value}");
            root.app.SetupVariables(key, value);
        }

        foreach (var key in configfiles.Keys)
        {
            var value = configfiles[key];
            //Debug.Log($"ConfigFile Key: {key}, Path: {value}");
            root.app.SetupConfigFiles(key, value);
        }

        rootInspector = root;
    }


    [System.Serializable]
    public class Config
    {
        public string id;
        public string title;
        public string contents_url;
        public string lastSyncDate;
        public string lastContentsUpdate;
    }

    [System.Serializable]
    public class Data
    {
        public string key;
        public Languages languages;
        public void SetupLanguage(string content)
        {
            var jsonContent = JSON.Parse(content);
            languages = new Languages
            {
                pt = jsonContent["pt"],
                en = jsonContent["en"],
                es = jsonContent["es"]
            };
        }
    }

    [System.Serializable]
    public class Languages
    {
        public string pt;
        public string en;
        public string es;
    }

    [System.Serializable]
    public class Content
    {
        public string key;

        public Languages title;

        public Languages text;
        public Languages text2;

        public Languages file1;
        public Languages file2;
    }

    [System.Serializable]
    public class Variable
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class ConfigFile
    {
        public string key;
        public string path;
    }

    [System.Serializable]
    public class App
    {
        public List<Data> tokens;
        public List<Data> contents;
        public List<Content> advcontents;
        public List<Variable> variables;
        public List<ConfigFile> configfiles;
        public void SetupToken(string key, string content)
        {
            var data = new Data
            {
                key = key
            };
            data.SetupLanguage(content);
            tokens.Add(data);
        }
        public void SetupContents(string key, string content)
        {
            var data = new Data
            {
                key = key
            };
            data.SetupLanguage(content);
            contents.Add(data);
        }
        public void SetupAdvContents(string key,JSONNode adcontentNode)
        {
            var content = new Content
            {
                key = key,
                title = new Languages
                {
                    pt = adcontentNode["title"]["pt"],
                    en = adcontentNode["title"]["en"],
                    es = adcontentNode["title"]["es"]
                },
                text = new Languages
                {
                    pt = adcontentNode["text"]["pt"],
                    en = adcontentNode["text"]["en"],
                    es = adcontentNode["text"]["es"]
                },
                text2 = new Languages
                {
                    pt = adcontentNode["text2"]["pt"],
                    en = adcontentNode["text2"]["en"],
                    es = adcontentNode["text2"]["es"]
                },
                file1 = new Languages
                {
                    pt = adcontentNode["file1"]["pt"],
                    en = adcontentNode["file1"]["en"],
                    es = adcontentNode["file1"]["es"]
                },
                file2 = new Languages
                {
                    pt = adcontentNode["file2"]["pt"],
                    en = adcontentNode["file2"]["en"],
                    es = adcontentNode["file2"]["es"]
                }
            };
            advcontents.Add(content);
        }
        public void SetupVariables(string key, string value)
        {
            var variable = new Variable
            {
                key = key,
                value = value
            };
            variables.Add(variable);
        }
        public void SetupConfigFiles(string key, string path)
        {
            var configfile = new ConfigFile
            {
                key = key,
                path = path
            };
            configfiles.Add(configfile);
        }
    }

    [System.Serializable]
    public class RootObject
    {
        public Config config;
        public App app;
    }
}
