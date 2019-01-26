using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayer : RoomHook
{
    public AudioClip audioLog;

    AudioSource aS;
    
    protected override void ObjectAttached()
    {
        GameObject obj = targetObject.gameObject;
        targetObject.ConnectedToHook();
        targetObject = null;
        obj.transform.position = targetTransform.position;
        obj.transform.rotation = targetTransform.rotation;
        obj.GetComponent<Rigidbody>().isKinematic = true;

        aS = gameObject.AddComponent<AudioSource>();
        aS.spatialBlend = 1f;
        aS.clip = audioLog;
        aS.Play();

        StartCoroutine(SpinWhilePlaying());
    }

    IEnumerator SpinWhilePlaying()
    {
        while (aS.isPlaying)
        {
            joint.gameObject.transform.Rotate(0f, Time.deltaTime, 0f);
            yield return new WaitForEndOfFrame();
        }
    }
}
