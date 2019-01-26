using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLogManager : MonoBehaviour
{
    public static AudioLogManager Instance;

    [HideInInspector]
    public AudioLog CurrentLogPlaying;

    public AudioSource Source;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CurrentLogPlaying != null && !Source.isPlaying)
        {
            CurrentLogPlaying = null;
        }
    }

    //do we want to cross fade? or just not allow interuption
    public bool PlayLog(AudioLog a)
    {
        if (CurrentLogPlaying != null) { return false;}

        CurrentLogPlaying = a;
        Source.clip = a.Log;
        Source.Play();
        return true;
    }

    public bool StopLog(AudioLog a)
    {
        if (CurrentLogPlaying == null || CurrentLogPlaying != a) { return false; }

        Source.Stop();
        CurrentLogPlaying = null;
        return true;
    }
}
