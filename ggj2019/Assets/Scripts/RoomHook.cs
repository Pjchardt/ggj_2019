using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomHook : MonoBehaviour
{
    public string ObjectKey;
    public LayerMask ObjectsMask;

    RoomObject targetObject;
    Rigidbody rb;
    FixedJoint joint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (joint != null || targetObject != null) { return; }

        if (ObjectsMask != (ObjectsMask | (1 << other.gameObject.layer))) { return; }

        RoomObject temp = other.attachedRigidbody.GetComponent<RoomObject>();

        if (temp.ObjectKey == ObjectKey)
        {
            targetObject = temp;
            StartCoroutine(TrackObject());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (joint != null || targetObject == null) { return; }

        if (ObjectsMask != (ObjectsMask | (1 << other.gameObject.layer))) { return; }

        if (targetObject == other.attachedRigidbody.GetComponent<RoomObject>())
        {
            targetObject = null;
        }
    }

    IEnumerator TrackObject()
    {
        while(targetObject != null)
        {
            if (Vector3.Distance(transform.position, targetObject.transform.position) < .25f)
            {
                GameObject obj = targetObject.gameObject;
                targetObject.ConnectedToHook();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                joint = obj.AddComponent<FixedJoint>();
                joint.connectedBody = rb;
            }
            else
            {
                Vector3 dir = transform.position - targetObject.transform.position;
                targetObject.MoveTowardsHook(dir.normalized * .25f);
            }
            

            yield return new WaitForFixedUpdate();
        }
    }
}
