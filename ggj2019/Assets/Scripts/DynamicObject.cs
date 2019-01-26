using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class DynamicObject : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody Rb;
    Outline outlineRef;
    
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        outlineRef = GetComponent<Outline>();
        outlineRef.enabled = false;
    }

    public void OnHoverEnter()
    {
        outlineRef.enabled = true;
    }

    public void OnHoverExit()
    {
        outlineRef.enabled = false;
    }
}
