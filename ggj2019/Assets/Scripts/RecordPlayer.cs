using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPlayer : RoomHook
{
    AudioLog aL;
    GameObject connectedRecord;

    protected override void Awake()
    {
        base.Awake();
        aL = GetComponent<AudioLog>();
    }

    protected override void ObjectAttached()
    {
        connectedRecord = targetObject.gameObject;
        targetObject.ConnectedToHook();
        targetObject = null;
        connectedRecord.transform.position = targetTransform.position;
        connectedRecord.transform.rotation = targetTransform.rotation;
        connectedRecord.GetComponent<Rigidbody>().isKinematic = true;

        aL.PlayClip();
        //StartCoroutine(SpinWhilePlaying());
    }

    /*IEnumerator SpinWhilePlaying()
    {
        while (aS.isPlaying)
        {
            connectedRecord.transform.Rotate(0f, Time.deltaTime* 45f, 0f);
            yield return new WaitForEndOfFrame();
        }
    }*/
}
