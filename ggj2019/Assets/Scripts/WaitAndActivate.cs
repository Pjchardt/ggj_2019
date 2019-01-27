using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ActivateData
{
    public float WaitTime;
    public GameObject Obj;
}

public class WaitAndActivate : MonoBehaviour
{
    public ActivateData[] objects;

    public void StartActivating()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            StartCoroutine(WaitToActivate(objects[i]));
        }
        
    }

    IEnumerator WaitToActivate(ActivateData a)
    {
        yield return new WaitForSeconds(a.WaitTime);

        a.Obj.SetActive(true);
    }
}
