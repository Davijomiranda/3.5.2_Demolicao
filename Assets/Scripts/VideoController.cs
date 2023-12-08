using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { 
            if (!videoP.isPlaying) { 
                videoP.Play();
            }
        }
    }
}
