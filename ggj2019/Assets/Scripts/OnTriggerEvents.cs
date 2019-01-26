using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEvent : UnityEvent<Collider>
{
}

public class OnTriggerEvents : MonoBehaviour
{
    public TriggerEvent TriggerEnter;
    public TriggerEvent TriggerExit;

    public void OnTriggerEnter(Collider other)
    {
        TriggerEnter.Invoke(other);
    }


    private void OnTriggerExit(Collider other)
    {
        TriggerExit.Invoke(other);
    }

}
