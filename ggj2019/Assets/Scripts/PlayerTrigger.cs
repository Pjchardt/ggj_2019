using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent eventToCall;

    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) { return; }

        eventToCall.Invoke();
    }
}
