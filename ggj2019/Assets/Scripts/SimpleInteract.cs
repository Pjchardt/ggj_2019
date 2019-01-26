//Based off SteamVR_TestThrow script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class SimpleInteract : MonoBehaviour
{
    public LayerMask DynamicLayerMask;
    public SphereCollider OverlapCollider;
    public Rigidbody AttachPoint;

    DynamicObject hoverObject;
    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void FixedUpdate ()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (hoverObject != null)
            {
                //hoverObject.transform.position = AttachPoint.transform.position;
                joint = hoverObject.gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = AttachPoint;
            }
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Object.DestroyImmediate(joint);
            joint = null;
            
            // We should probably apply the offset between trackedObj.transform.position
            // and device.transform.pos to insert into the physics sim at the correct
            // location, however, we would then want to predict ahead the visual representation
            // by the same amount we are predicting our render poses.

            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null)
            {
                hoverObject.Rb.velocity = origin.TransformVector(device.velocity);
                hoverObject.Rb.angularVelocity = origin.TransformVector(device.angularVelocity);
            }
            else
            {
                hoverObject.Rb.velocity = device.velocity;
                hoverObject.Rb.angularVelocity = device.angularVelocity;
            }

            hoverObject.Rb.maxAngularVelocity = hoverObject.Rb.angularVelocity.magnitude;
            hoverObject.OnHoverExit();
        }
    }

    public void TriggerEnter(Collider c)
    {
        if (hoverObject != null || DynamicLayerMask != (DynamicLayerMask | (1 << c.gameObject.layer))) { return; }

        hoverObject = c.attachedRigidbody.GetComponent<DynamicObject>();
        hoverObject.OnHoverEnter();
    }

    public void TriggerExit(Collider c)
    {
        if (hoverObject == null || DynamicLayerMask != (DynamicLayerMask | (1 << c.gameObject.layer))) { return; }

        DynamicObject temp = c.attachedRigidbody.GetComponent<DynamicObject>();

        if (hoverObject == temp)
        {
            hoverObject.OnHoverExit();
            hoverObject = null;
        }
    }
}
