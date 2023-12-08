using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControllerPira : MonoBehaviour
{
    public VideoPlayer videoP;

    public VideoClip clipIdle;
    public VideoClip clipAction;

    public GameObject panelFire;

    private bool idle = true;
    private bool fire = false;

    private float i_timerFire = 0;

    void Start()
    {

    }

    public void activeVideo()
    {
        if (idle)
        {
            videoP.clip = clipAction;
            videoP.isLooping = false;
            videoP.Play();
            Invoke(nameof(setIdle), 0.1f);
        }
    }

    private void activeFire()
    {
        if(!fire)
        {
            panelFire.SetActive(true); 
            fire = true;
            i_timerFire = 3;
        }
    }

    private void setIdle()
    {
        idle = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            activeVideo();
            activeFire();
        }

        if (!idle)
        {
            if (!videoP.isPlaying)
            {
                videoP.clip = clipIdle;
                videoP.isLooping = true;
                videoP.Play();
                idle = true;
                panelFire.SetActive(false);
                fire = false;
            }
        }

        if (fire)
        {
            if (i_timerFire > 0)
            {
                i_timerFire -= Time.deltaTime;
            }
            else
            {
                panelFire.SetActive(false);
                fire = false;
            }
        }
    }
}
