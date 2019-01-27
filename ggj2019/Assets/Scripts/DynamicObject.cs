using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class DynamicObject : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody Rb;
    Transform whistleTarget;
    Outline outlineRef;
    SimpleInteract interactConnected;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        outlineRef = GetComponent<Outline>();
        outlineRef.enabled = false;
    }

    private void OnDestroy()
    {
        if (interactConnected != null) { interactConnected.ForceDrop(); }
    }

    public void OnHoverEnter()
    {
        outlineRef.enabled = true;
    }

    public void OnHoverExit()
    {
        outlineRef.enabled = false;
    }

    public void OnGrab(SimpleInteract s)
    {
        whistleTarget = null;
        interactConnected = s;
    }

    public void OnDrop()
    {
        interactConnected = null;
        outlineRef.enabled = false;
    }

    public void OnWhistle(Transform t)
    {
        outlineRef.enabled = false;
        //turn on a particle effect trail
        whistleTarget = t;
        StartCoroutine(FollowWhistle());
    }

    IEnumerator FollowWhistle()
    {
        yield return new WaitForSeconds(1f);

        while (whistleTarget != null)
        {
            if (Vector3.Distance(transform.position, whistleTarget.position) < .2)
            {
                Rb.velocity *= .99f;
            }
            else
            {
                Vector3 targetDir = whistleTarget.position - transform.position;
                Rb.AddForce(targetDir.normalized * 2f);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
