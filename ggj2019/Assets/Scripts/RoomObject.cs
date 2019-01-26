using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public enum ObjectState { Wander, Return, Wistle }
    ObjectState currentState;

    public Room RoomIn;
    public string ObjectKey;

    Vector3 startPosition;
    Transform whistleTarget;
    DynamicObject dynamicRef;

    private void Awake()
    {
        dynamicRef = GetComponent<DynamicObject>();
        startPosition = transform.position;
    }

    private void Start()
    {
        StartWander();
    }

    private void FixedUpdate()
    {
        dynamicRef.Rb.velocity = (dynamicRef.Rb.velocity.magnitude > 1) ? dynamicRef.Rb.velocity.normalized * 1f : dynamicRef.Rb.velocity;

        switch (currentState)
        {
            case ObjectState.Wander:
                if (Vector3.Distance(transform.position, RoomIn.transform.position) > RoomIn.RoomRadius)
                {
                    ReturnToStart();
                }
                break;
            case ObjectState.Return:
                if (Vector3.Distance(transform.position, startPosition) < .5)
                {
                    StartWander();
                }
                else
                {
                    Vector3 dir = startPosition - transform.position;
                    dynamicRef.Rb.velocity = dir.normalized * .25f;
                }
                break;
            case ObjectState.Wistle:
                if (Vector3.Distance(transform.position, whistleTarget.position) < .15)
                {
                    StopWistle();
                }
                else
                {
                    Vector3 targetDir = whistleTarget.position - transform.position;
                    dynamicRef.Rb.velocity = targetDir.normalized * 2f;
                }
                break;
        }
    }

    void StartWander()
    {
        currentState = ObjectState.Wander;
        StartCoroutine(Wander());
    }

    void ReturnToStart()
    {
        currentState = ObjectState.Return;
    }

    public void DoWistle(Transform target)
    {
        currentState = ObjectState.Wistle;
        whistleTarget = target;
    }

    public void StopWistle()
    {
        whistleTarget = null;
        StartWander();
    }

    public void MoveTowardsHook(Vector3 dir)
    {
        dynamicRef.Rb.AddForce(dir);
    }

    public void ConnectedToHook()
    {
        RoomIn.RoomObjectHooked();
        Destroy(dynamicRef);
        Destroy(this);
    }

    IEnumerator Wander()
    {
        while (currentState == ObjectState.Wander)
        {
            dynamicRef.Rb.AddForce(Random.insideUnitSphere);
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
        
    }
}
