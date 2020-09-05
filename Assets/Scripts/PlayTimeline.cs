using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayTimeline : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public GameObject button;

    public void Play()
    {
        playableDirector.Play();
        button.SetActive(false);
    }

    public void PlayFromTimeline()
    {
        playableDirector.Play(timeline);
        button.SetActive(false);
    }
}
