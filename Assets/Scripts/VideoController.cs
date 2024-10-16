using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoP;

    public GameObject[] panels;
    public Sprite[] frameDetonador;
    public Sprite[] frameTexto;
    public Sprite[] frameTitulo;
    public Sprite[] frameDetalhe1;
    public Sprite[] frameDetalhe2;
    public Sprite[] frameDetalhe3;
    public string[] stringTitles; //titulos dos paineis
    public string[] stringTitlesDetalhes; //titulos dos paineis
    public string[] stringDesc; //descricões
    public Sprite[] capasVideo; //imagens videos

    public Image imgDetonador;
    public Image imgTexto;
    public Image imgTitulo;
    public Image imgCapaVideo;
    public Image imgDetalhe1;
    public Image imgDetalhe2;
    public Image imgDetalhe3;

    public VideoClip[] Clips; //videos
    public VideoClip ClipIdle;

    private int index = 0;
    private GameObject pnAtual;
    public GameObject panelText;
    public GameObject panelBlur;

    public Animator animPanelText;
    public Animator animPanelSmoke;
    public Animator animPanelWarning;
    public Animator animPanelTrans;
    public Animator animDetonador;

    private bool detonar = true;
    private bool countVideo = false;
    private bool activeButton = true;
    private float timerVideo;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textTitle_detalhes;
    public TextMeshProUGUI textDesc;
    public TextMeshProUGUI textDetonador;
    public TextMeshProUGUI textDetonador2;

    public GetterFromJson getter;
    public const string keyVideoIdle = "idle";
    public const string keyVideoSambodromo = "videoclip_content1";
    public const string keyVideoGrota = "videoclip_content2";
    public const string keyVideoPerimetral = "videoclip_content3";
    public const string keyVideoTransolimpica = "videoclip_content4";
    public const string keyImagemIdle = "image_idle";
    public const string keyImagemSambodromo = "imagecover_content1";
    public const string keyImagemGrota = "imagecover_content2";
    public const string keyImagemPerimetral = "imagecover_content3";
    public const string keyImagemTransolimpica = "imagecover_content4";
    public const string keyTitleSambodromo = "title_content1";
    public const string keyTitleGrota = "title_content2";
    public const string keyTitlePerimetral = "title_content3";
    public const string keyTitleTransolimpica = "title_content4";
    public const string keyDescricaoSambodromo = "description_content1";
    public const string keyDescricaoGrota = "description_content2";
    public const string keyDescricaoPerimetral = "description_content3";
    public const string keyDescricaoTransolimpica = "description_content4";
    public const string keyTitleDetonador = "text_content1";
    public List<string> titles;
    public List<string> videos;
    public List<string> images;
    public List<string> descriptions;
    public GameObject[] mapa;

    public bool BackOffice;

    private bool fumaca = true;

    private void Start()
    {
        if (BackOffice)
        {
            BackEnd("pt");
        }
        SetVideo();
    }

    private void BackEnd(string language)
    {
        //adding videos
        videos = new List<string>
        {
            keyVideoSambodromo,
            keyVideoGrota,
            keyVideoPerimetral,
            keyVideoTransolimpica
        };
        //adding images
        images = new List<string>
        {
            keyImagemSambodromo,
            keyImagemGrota,
            keyImagemPerimetral,
            keyImagemTransolimpica
        };
        //adding desc
        descriptions = new List<string>
        {
            keyDescricaoSambodromo,
            keyDescricaoGrota,
            keyDescricaoPerimetral,
            keyDescricaoTransolimpica
        };
        //adding title
        titles = new List<string>
        {
            keyTitleSambodromo,
            keyTitleGrota,
            keyTitlePerimetral,
            keyTitleTransolimpica
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && activeButton)
        {
            if (detonar)
            {
                detonar = false;
                fumaca = false;
                animDetonador.SetTrigger("start");
                Invoke("Detonar", 1f);
                Invoke("ReactiveFumaca", 2);
                //Detonar();
            }
            else
            {
                if (fumaca)
                {
                    fumaca = false;
                    animPanelSmoke.SetTrigger("Start");
                    Invoke("ReactiveFumaca", 2);
                }
            }
        }
        if (countVideo)
        {
            timerVideo -= Time.deltaTime;
            if (timerVideo < 0)
            {
                countVideo = false;
                activeButton = false;
                timerVideo = 0;
                animPanelTrans.SetTrigger("Start");
                Invoke(nameof(SetVideo), 0.5f);
                Invoke(nameof(ReactiveDetonador), 0.8f);
            }
        }
    }

    private void ReactiveFumaca()
    {
        fumaca = true;
    }

    void ReactiveDetonador()
    {
        animDetonador.SetTrigger("start");
        activeButton = true;
    }

    private void SetVideo()
    {
        if (BackOffice)
        {
            // videoP.clip = Clips[index];
            getter.GetVideoFromBackOffice(getter.GetPathVideoFromJson(videos[index], getter._appConfig.braceletModel.language), videoP,
                callback =>
                {
                    // timerVideo = (float)callback.length;
                    getter.GetImageFromBackOffice(getter.GetPathImageFromJson(images[index], getter._appConfig.braceletModel.language), callback =>
                    {
                        imgCapaVideo.sprite = callback;
                        imgCapaVideo.gameObject.SetActive(true);
                        videoP.Play();
                        textTitle.text = getter.GetStringFromJson(titles[index]);
                        textDesc.text = getter.GetStringFromJson(descriptions[index]);
                        videoP.isLooping = true;
                        panelBlur.SetActive(true);
                        animPanelText.SetTrigger("Start");
                        Invoke("SetImages", 0.2f);
                        videoP.Pause();
                        detonar = true;
                        timerVideo = (float)videoP.length;
                        SetMapa(index);
                        
                    });
                });
        }
        else
        {
            videoP.clip = Clips[index];
            imgCapaVideo.sprite = capasVideo[index];
            imgCapaVideo.gameObject.SetActive(true);
            imgTexto.sprite = frameTexto[index];
            imgDetalhe1.sprite = frameDetalhe1[index];
            imgDetalhe2.sprite = frameDetalhe2[index];
            imgDetalhe3.sprite = frameDetalhe3[index];
            videoP.Play();
            textTitle.text = stringTitles[index];
            textTitle_detalhes.text = stringTitlesDetalhes[index];
            textDesc.text = stringDesc[index];
            videoP.isLooping = true;
            panelBlur.SetActive(true);
            animPanelText.SetTrigger("Start");
            Invoke("SetImages", 0.2f);
            videoP.Pause();
            SetMapa(index);
            detonar = true;
            timerVideo = (float)videoP.length;
        }
    }

    public void Detonar()
    {
        //videoP.clip = Clips[index];
        panelText.SetActive(true);
        panelBlur.SetActive(false);
        animPanelText.SetTrigger("Start");
        animPanelWarning.SetBool("Active" , true);
        Invoke(nameof(PlaySmoke), 2);
    }

    void PlaySmoke()
    {
        animPanelSmoke.SetTrigger("Start");
        Invoke(nameof(PlayVideo), 1);
    }

    void PlayVideo()
    {
        animPanelWarning.SetBool("Active" , false);
        videoP.isLooping = false;
        // timerVideo = (float)videoP.clip.length;
        videoP.Play();
        imgCapaVideo.gameObject.SetActive(false);
        if (index < panels.Length - 1)
            index++;
        else index = 0;
        detonar = false;
        countVideo = true;
    }
    public void SetMapa(int i)
    {
        foreach (var item in mapa)
        {
            item.SetActive(false);
        }
        mapa[i].SetActive(true);
    }

    private void SetImages()
    {
        panelText.SetActive(false);
        //imgDetonador.sprite = frameDetonador[index];
        imgTexto.sprite = frameTexto[index];
        imgTitulo.sprite = frameTitulo[index];
    }



}
