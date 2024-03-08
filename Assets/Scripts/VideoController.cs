using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoP;

    public GameObject[] panels;

    public VideoClip[] Clips;
    public VideoClip ClipIdle;

    private int index = 0;
    private GameObject pnAtual;

    private bool detonar = true;
    private float timerVideo;

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
                Detonar();
            }
        }
        if (!detonar)
        {
            timerVideo -= Time.deltaTime;
            if (timerVideo < 0)
            {
                timerVideo = 0;
                SetVideo();
                detonar = true;
            }
        }
    }

    private void SetVideo()
    {
        videoP.clip = ClipIdle;
        videoP.isLooping = true;
        videoP.Play();
        foreach (GameObject pn in panels)
        {
            pn.transform.GetChild(1).gameObject.SetActive(false);
            pn.SetActive(false);
        }
        pnAtual = panels[index];
        pnAtual.SetActive(true);
        timerVideo = (float)videoP.clip.length + 0.2f;
        detonar = true;
    }

    public void Detonar()
    {
        videoP.clip = Clips[index];
        videoP.isLooping = false;
        videoP.Play();
        if (index < panels.Length -1)
            index++;
        else index = 0;
        pnAtual.transform.GetChild(1).gameObject.SetActive(true);
        detonar = false;
    }
}
