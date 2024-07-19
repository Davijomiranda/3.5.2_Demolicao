using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoP;

    public GameObject[] panels;
    public Sprite[] frameDetonador;
    public Sprite[] frameTexto;
    public Sprite[] frameTitulo;
    public string[] stringTitles;
    public string[] stringDesc;
    public Sprite[] capasVideo;

    public Image imgDetonador;
    public Image imgTexto;
    public Image imgTitulo;
    public Image imgCapaVideo;

    public VideoClip[] Clips;
    public VideoClip ClipIdle;

    private int index = 0;
    private GameObject pnAtual;
    public GameObject panelText;
    public GameObject panelBlur;

    public Animator animPanelText;
    public Animator animPanelSmoke;
    public Animator animPanelWarning;
    public Animator animPanelTrans;

    private bool detonar = true;
    private float timerVideo;

    public LerpAnim btAnim1;
    public LerpAnim btAnim2;
    public LerpAnim btAnim3;
    public LerpAnim btAnim4;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textDesc;

    private void Start()
    {
        SetVideo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (detonar)
            {
                btAnim1.Button();
                detonar = false;
                Invoke("Detonar", 0);
                //Detonar();
            }
            else
            {
                animPanelSmoke.SetTrigger("Start");
            }
        }
        if (!detonar)
        {
            timerVideo -= Time.deltaTime;
            if (timerVideo < 0)
            {
                timerVideo = 0;
                animPanelTrans.SetTrigger("Start");
                Invoke(nameof(SetVideo), 0.2f);
                detonar = true;                
            }
        }
    }

    private void SetVideo()
    {
        videoP.clip = Clips[index];
        imgCapaVideo.sprite = capasVideo[index];
        imgCapaVideo.gameObject.SetActive(true);
        videoP.Play();
        textTitle.text = stringTitles[index];
        textDesc.text = stringDesc[index];
        videoP.isLooping = true;
        panelBlur.SetActive(true);
        animPanelText.SetTrigger("Start");
        Invoke("SetImages", 0.2f);
        videoP.Pause();
        /*foreach (GameObject pn in panels)
        {
            pn.transform.GetChild(1).gameObject.SetActive(false);
            pn.SetActive(false);
        }*/
        //pnAtual = panels[index];
        //pnAtual.SetActive(true);
        detonar = true;
        timerVideo = (float)videoP.clip.length;
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
        Invoke(nameof(PlayVideo), 2);
    }

    void PlayVideo()
    {
        animPanelWarning.SetBool("Active" , false);
        videoP.isLooping = false;
        timerVideo = (float)videoP.clip.length;
        videoP.Play();
        imgCapaVideo.gameObject.SetActive(false);
        if (index < panels.Length - 1)
            index++;
        else index = 0;
        detonar = false;
    }

    private void SetImages()
    {
        panelText.SetActive(false);
        imgDetonador.sprite = frameDetonador[index];
        imgTexto.sprite = frameTexto[index];
        imgTitulo.sprite = frameTitulo[index];
    }



}
