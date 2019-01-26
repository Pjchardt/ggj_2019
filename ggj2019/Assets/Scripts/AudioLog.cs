using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog : MonoBehaviour
{
    public AudioClip Log;

    private void Awake()
    {
        //testing
        PlayClip();
    }

    public void PlayClip()
    {
        AudioLogManager.Instance.PlayLog(this); //check if failed and keep trying?
    }

    public void StopClip()
    {

    }
}
