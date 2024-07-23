using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManagerTesteBackoffice : MonoBehaviour
{
    [Header("Panels")] public GameObject panelImage;
    public GameObject panelVideo;
    public GameObject panelText;
    public GameObject panelMain;
    private GameObject _currentPanel;
    private GetterFromJson _getterFromJson;
    [Header("Image")] public Image image;
    public TMP_InputField inputImageId;
    public TMP_InputField inputImageLanguage;
    [Header("Video")] public VideoPlayer videoPlayer;
    public TMP_InputField inputVideoId;
    public TMP_InputField inputVideoLanguage;
    [Header("Texts")] public TMP_Text text;
    public TMP_InputField inputIdText;
    public TMP_InputField inputLanguageText;

    private void Start()
    {
        SetPanel(panelMain);
        _getterFromJson = FindObjectOfType<GetterFromJson>();
    }

    public void SetPanelImage()
    {
        SetPanel(panelImage);
    }

    public void SetPanelVideo()
    {
        SetPanel(panelVideo);
    }

    public void SetPanelText()
    {
        SetPanel(panelText);
    }

    public void SetPanelMain()
    {
        SetPanel(panelMain);
    }

    private void SetPanel(GameObject obj)
    {
        if (_currentPanel != null)
        {
            _currentPanel.SetActive(false);
        }

        _currentPanel = obj;
        obj.SetActive(true);
    }

    #region SetImage

    public void SetImage()
    {
        var id = inputImageId.text;
        var language = inputImageLanguage.text;

        _getterFromJson.GetImageFromBackOffice(_getterFromJson.GetPathImageFromJson(id, language),
            sprite => { image.sprite = sprite; });
    }

    #endregion

    #region SetVideo

    public void SetVideo()
    {
        var id = inputVideoId.text;
        var language = inputVideoLanguage.text;

        _getterFromJson.GetVideoFromBackOffice(_getterFromJson.GetPathVideoFromJson(id, language),videoPlayer,
            videoclip =>
            {
                // videoPlayer.clip = videoclip;
                // videoPlayer.Play();
            });
    }

    #endregion

    #region SetText

    public void SetText()
    {
        var id = inputIdText.text;
        var language = inputLanguageText.text;

        text.text = _getterFromJson.GetStringFromJson(id);
    }

    #endregion
}