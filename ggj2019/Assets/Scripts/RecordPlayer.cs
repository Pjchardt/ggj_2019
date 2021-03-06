﻿using System.Collections;
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
        connectedRecord.GetComponent<Rigidbody>().isKinematic = true;
        connectedRecord.transform.position = targetTransform.position;
        connectedRecord.transform.rotation = targetTransform.rotation;

        //AudioSource aS = aL.PlayClip();
        StartCoroutine(SpinWhilePlaying());
    }

    IEnumerator SpinWhilePlaying()
    {
        while (true/*aS != null && aS.isPlaying*/)
        {
            connectedRecord.transform.Rotate(0f, Time.deltaTime* 45f, 0f);
            yield return new WaitForEndOfFrame();
        }
    }
}
