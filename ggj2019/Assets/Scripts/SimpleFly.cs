﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SimpleFly : MonoBehaviour
{
    public float Speed = 1f;

    SteamVR_TrackedObject trackedObj;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            SimplePlayer.Instance.transform.position += trackedObj.transform.forward * Speed;
        }
    }
    
}