using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLogManager : MonoBehaviour
{
    public static AudioLogManager Instance;

    [HideInInspector]
    public AudioSource CurrentSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (CurrentSource != null && !CurrentSource.isPlaying)
        {
            Destroy(CurrentSource);
            CurrentSource = null;
        }
    }

    //do we want to cross fade? or just not allow interuption
    public void PlayLog(AudioLog a)
    {
        AudioSource aS = gameObject.AddComponent<AudioSource>();
        aS.spatialBlend = 0f;
        aS.clip = a.Log;
        aS.Play();

        if (CurrentSource != null)
        {
            StartCoroutine(CrossFade(aS));
        }
        else
        {
            CurrentSource = aS;
        }
    }

    IEnumerator CrossFade (AudioSource newLog)
    {
        float timeElapsed = 0f;
        
        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime * .2f;
            CurrentSource.volume = Mathf.Lerp(1f, 0f, timeElapsed);
            newLog.volume = Mathf.Lerp(0f, 1f, timeElapsed);
            yield return new WaitForEndOfFrame();
        }

        Destroy(CurrentSource);
        CurrentSource = newLog;
        newLog.volume = 1f;
    }
}
