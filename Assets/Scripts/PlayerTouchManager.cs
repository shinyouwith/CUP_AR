using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerTouchManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Start()
    {
        GameObject videoPlane = GameObject.Find("SheepVideo");
        videoPlayer = videoPlane.GetComponent<VideoPlayer>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            toggleVideoPlayerState();
        }
    }

    private void toggleVideoPlayerState() {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else {
            videoPlayer.Play();
        }
    }
}
