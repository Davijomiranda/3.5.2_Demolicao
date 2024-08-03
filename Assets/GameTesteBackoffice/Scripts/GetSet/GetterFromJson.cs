using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GetterFromJson : MonoBehaviour
{
    public AppConfig _appConfig;
    private int controlNumberVideo = 0;
    public string urlBackOffice;
    public List<TextsTranslate> textsTranslates;
    public List<TextsCodeBack> textsCodeBack;

    private void Awake()
    {
        _appConfig = GetComponent<AppConfig>();
    }

    private void Start()
    {
        GetStringFromJson("text_content1");
    }

    #region Image

    public string GetPathImageFromJson(string id, string language)
    {
        language = language == string.Empty ? "pt" : language;
        string str = String.Empty;
        AppConfig.Data s;
        s = _appConfig.rootInspector.app.contents.Find(x => x.key == id);
        switch (language)
        {
            case "pt":
                str = s.languages.pt;
                break;
            case "es":
                str = s.languages.es;
                break;
            case "en":
                str = s.languages.en;
                break;
            default:
                str = s.languages.pt;
                break;
        }

        print($"Image Content: {str}");
        return str;
    }

    public void GetImageFromBackOffice(string path, System.Action<Sprite> callback)
    {
        StartCoroutine(LoadTextureFromFile(path, callback));
    }

    private IEnumerator LoadTextureFromFile(string path, System.Action<Sprite> callback)
    {
        using var uwr = UnityWebRequestTexture.GetTexture($"file://{_appConfig.path}/{path}");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao carregar imagem: " + uwr.error);
            callback(null); // Passa null no callback se ocorrer um erro
        }
        else
        {
            // Obtem a textura da resposta
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            // Cria um Sprite a partir da textura carregada
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            callback(sprite); // Passa o sprite carregado no callback
        }
    }

    #endregion

    #region Video

    public string GetPathVideoFromJson(string id, string language)
    {
        language = language == string.Empty ? "pt" : language;
        string str = String.Empty;
        AppConfig.Data s;
        s = _appConfig.rootInspector.app.contents.Find(x => x.key == id);
        switch (language)
        {
            case "pt":
                str = s.languages.pt;
                break;
            case "es":
                str = s.languages.es;
                break;
            case "en":
                str = s.languages.en;
                break;
            default:
                str = s.languages.pt;
                break;
        }

        print($"Video Content: {str}");
        return str;
    }

    public void GetVideoFromBackOffice(string path, VideoPlayer videoPlayer, Action<VideoClip> callback = null)
    {
        StartCoroutine(LoadVideoFromFile(path, videoPlayer, callback));
    }

    private IEnumerator LoadVideoFromFile(string path, [NotNull] VideoPlayer videoPlayer, Action<VideoClip> callback)
    {
        if (videoPlayer == null) throw new ArgumentNullException(nameof(videoPlayer));
        string Spath = $"file://{_appConfig.path}/{path}";
        videoPlayer.Stop();
        using var uwr = UnityWebRequest.Get($"file://{_appConfig.path}/{path}");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao carregar vídeo: " + uwr.error);
            callback(null); // Passa null no callback se ocorrer um erro
        }
        else
        {
            // Carrega o vídeo usando VideoClip
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, "tempVideo.mp4");
            videoPlayer.url = Spath;
            videoPlayer.Prepare();
            yield return new WaitUntil(() => videoPlayer.isPrepared);
            // videoPlayer.Play();
            callback?.Invoke(videoPlayer.clip); // Passa o VideoClip no callback
        }
    }

    #endregion

    #region Texts

    public void TranslateTexts()
    {
        if (textsTranslates.Count == 0) return;
        foreach (var item in textsTranslates)
        {
            item.text.text = GetStringFromJson(item.id);
        }
    }

    public string GetStringFromJson(string id)
    {
        var language = _appConfig.braceletModel.language == string.Empty ? "pt" : _appConfig.braceletModel.language;
        string str;
        AppConfig.Data s;
        s = _appConfig.rootInspector.app.tokens.Find(x => x.key == id);
        switch (language)
        {
            case "pt":
                str = s.languages.pt;
                str = HttpUtility.HtmlDecode(str);
                return str.Replace("<p>", "").Replace("/>", "").Replace("</p>", "").Replace("<br","");
            case "es":
                str = s.languages.es;
                  str = HttpUtility.HtmlDecode(str);
                return str.Replace("<p>", "").Replace("/>", "").Replace("</p>", "").Replace("<br","");      
            case "en":
                str = s.languages.en;
                  str = HttpUtility.HtmlDecode(str);
                return str.Replace("<p>", "").Replace("/>", "").Replace("</p>", "").Replace("<br","");     
            default:
                str = s.languages.pt;
                  str = HttpUtility.HtmlDecode(str);
                return str.Replace("<p>", "").Replace("/>", "").Replace("</p>", "").Replace("<br","");     
        }
    }

    public string TranslateCodeBack(string code)
    {
        foreach (var texts in textsCodeBack)
        {
            if (!code.Contains(texts.code)) continue;
            var str = code;
            return str.Replace(texts.code, texts.letter);
        }
        return string.Empty;
    }
    #endregion

    #region Breacelet

    public BraceletModel LoadJsonBracelet()
    {
        var path = $"{_appConfig.pathDownloadedBraceletJson}{_appConfig.nameArchiveBracelet}{_appConfig.idBracelet}.json";
        if (File.Exists(path))
        {
            try
            {
                var json = File.ReadAllText(path);
                var teste = Regex.Unescape(json);
                var bracelet = JsonUtility.FromJson<BraceletModel>(json);
                return bracelet;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Erro ao carregar o JSON: " + ex.Message);
                return null;
            }
        }
        else
        {
            Debug.LogError("Arquivo não encontrado: " + path);
            return null;
        }
    }

    public void SaveJsonBracelet(BraceletModel bracelet, Action onSuccess, Action<string> onFailure)
    {
        var path = $"{_appConfig.pathDownloadedBraceletJson}{_appConfig.nameArchiveBracelet}{_appConfig.idBracelet}.json";
        try
        {
            string json = JsonUtility.ToJson(bracelet);
            string id = $"{_appConfig.braceletModel.id}";
            string avatar = JsonUtility.ToJson(_appConfig.braceletModel.avatar);
            File.WriteAllText(path, json);
            Debug.Log("JSON salvo com sucesso em: " + path);

            // Enviar a requisição POST para o servidor
            var form = new WWWForm();
            form.AddField("id",id);
            form.AddField("avatar",avatar);
            string postUrl = urlBackOffice;

            // Use StartCoroutine para chamar uma função que usa UnityWebRequest
            StartCoroutine(PostDataToServer(postUrl, form, onSuccess, onFailure));
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Erro ao salvar o JSON: " + ex.Message);
            onFailure?.Invoke(ex.Message);
        }
    }

    // Função Coroutine para enviar os dados para o servidor
    private IEnumerator PostDataToServer(string url, WWWForm form, Action onSuccess, Action<string> onFailure)
    {
        using var www = UnityWebRequest.Post($"{url}", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            onFailure?.Invoke(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            onSuccess?.Invoke();
        }
    }
    // Função para remover os caracteres de escape de um JSON string
    private string UnescapeJson(string jsonString)
    {
        // Remove os caracteres de escape
        return jsonString.Replace('\"', '"');
    }

    #endregion
}

[Serializable]
public class TextsTranslate
{
    public TMP_Text text;
    public string id;
}

[Serializable]
public class TextsCodeBack
{
    public string code;
    public string letter;
}