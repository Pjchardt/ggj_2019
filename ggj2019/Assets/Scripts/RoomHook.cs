using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomHook : MonoBehaviour
{
    public string ObjectKey;
    public LayerMask ObjectsMask;
    public Transform targetTransform;

    protected RoomObject targetObject;
    protected Rigidbody rb;
    protected FixedJoint joint;

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

    protected virtual void ObjectAttached()
    {
        GameObject obj = targetObject.gameObject;
        targetObject.ConnectedToHook();
        targetObject = null;
        obj.transform.position = targetTransform.position;
        obj.transform.rotation = targetTransform.rotation;
        joint = obj.AddComponent<FixedJoint>();
        joint.connectedBody = rb;
    }

    IEnumerator TrackObject()
    {
        while(targetObject != null)
        {
            if (Vector3.Distance(targetTransform.position, targetObject.transform.position) < .25f)
            {
                ObjectAttached();
            }
            else
            {
                Vector3 dir = targetTransform.position - targetObject.transform.position;
                targetObject.MoveTowardsHook(dir.normalized * .25f);
            }
            

            yield return new WaitForFixedUpdate();
        }
    }
}
