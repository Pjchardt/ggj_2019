using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLog : MonoBehaviour
{
    public AudioClip Log;

    public void PlayClip()
    {
        AudioLogManager.Instance.PlayLog(this); 
    }

    public void StopClip()
    {

    }
}
