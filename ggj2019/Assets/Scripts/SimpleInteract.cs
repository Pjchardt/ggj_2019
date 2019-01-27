//Based off SteamVR_TestThrow script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
[RequireComponent(typeof(TargetingLineRendering))]
public class SimpleInteract : MonoBehaviour
{
    public LayerMask DynamicLayerMask;
    public SphereCollider OverlapCollider;
    public Rigidbody AttachPoint;

    bool isTargeting;
    float timeTriggerDown;
    DynamicObject hoverObject;
    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    TargetingLineRendering targeting;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        targeting = GetComponent<TargetingLineRendering>();
    }

    void FixedUpdate()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        CheckGrabbing(device);
        CheckTargeting(device);
    }

    void CheckGrabbing(SteamVR_Controller.Device device)
    {
        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (hoverObject != null)
            {
                hoverObject.OnGrab(this);
                joint = hoverObject.gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = AttachPoint;
            }
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {

            Object.DestroyImmediate(joint);
            joint = null;

            if (hoverObject == null) { return; }

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
            hoverObject.OnDrop();
        }
    }

    void CheckTargeting(SteamVR_Controller.Device device)
    {
        if (joint != null) { return; }

        if (!isTargeting)
        {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                timeTriggerDown = Time.timeSinceLevelLoad;
            }
            else if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (timeTriggerDown + 1f > Time.timeSinceLevelLoad)
                {
                    isTargeting = true;
                }
            }
        }

        if (isTargeting)
        {
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (hoverObject != null)
                {
                    AudioManager.Instance.PlayWhistle(transform.position);
                    hoverObject.OnWhistle(transform);
                    hoverObject = null;
                }

                isTargeting = false;
                return;
            }

            RaycastHit hit = new RaycastHit();
            if (targeting.UpdateLine(new Ray(transform.position, transform.forward), ref hit))
            {
                DynamicObject d = hit.collider.attachedRigidbody.GetComponent<DynamicObject>();
                if (hoverObject != null && hoverObject != d) { hoverObject.OnHoverExit(); }
                else if (hoverObject == null)
                {
                    hoverObject = d;
                    hoverObject.OnHoverEnter();
                }
            }
            else if (hoverObject != null)
            {
                hoverObject.OnHoverExit();
                hoverObject = null;
            }
        }

        targeting.SetLineEnabled(isTargeting);
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

    public void ForceDrop ()
    {
        Object.DestroyImmediate(joint);
        joint = null;
    }
}
