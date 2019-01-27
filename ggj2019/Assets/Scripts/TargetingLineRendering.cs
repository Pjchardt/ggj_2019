using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingLineRendering : MonoBehaviour
{
    public int NumLineVerts = 100;
    public int numRayCasts = 4;
    public float MaxLineDistance;
    public LayerMask DynamicLayerMask;
    public LineRenderer l;

    bool isActive;
    Vector3 targetPosition;
    Vector3 currentPosition;

    private void Awake()
    {
        l.positionCount = NumLineVerts;
        l.enabled = false;
    }

    public void SetLineEnabled(bool isEnabled)
    {
        l.enabled = isEnabled;
    }

    public bool UpdateLine(Ray r, ref RaycastHit hit)
    {
        bool foundTarget = false;
        targetPosition = r.origin + r.direction * MaxLineDistance;
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, .2f);

        Vector3 controllerToObject = currentPosition - transform.position;
        Vector3 vecToTarget = targetPosition - currentPosition;
        Vector3 controlPoint = (transform.position + controllerToObject * .5f) + (vecToTarget);

        Vector3 raycastPosition;
        Vector3 raycastDirection;
        for (int i = 0; i < numRayCasts; i++)
        {
            raycastPosition = GetPoint(l.transform.position, controlPoint, currentPosition, i / (float)(numRayCasts - 1));
            raycastDirection = currentPosition - raycastPosition;
            if (DoRaycast(new Ray(raycastPosition, raycastDirection), out hit))
            {
                targetPosition = hit.collider.attachedRigidbody.transform.position;
                foundTarget = true;
                break;
            }
        }

        if (foundTarget)
        {
            currentPosition = targetPosition;
            controlPoint = (transform.position + (currentPosition - transform.position) * .5f);
        }

        LineRendering(controlPoint);

        return foundTarget;
    }

    bool DoRaycast(Ray r, out RaycastHit hit)
    {
        return Physics.SphereCast(r, .1f, out hit, MaxLineDistance, DynamicLayerMask);
    }

    void LineRendering(Vector3 controlPoint)
    {
        l.SetPosition(0, l.transform.position);
        for (int i = 1; i < l.positionCount - 1; i++)
        {
            l.SetPosition(i, GetPoint(l.transform.position, controlPoint, currentPosition, i / (float)(l.positionCount - 1)));
        }
        l.SetPosition(l.positionCount - 1, currentPosition);
    }

    public Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * p0 +
            2f * oneMinusT * t * p1 +
            t * t * p2;
    }
}
