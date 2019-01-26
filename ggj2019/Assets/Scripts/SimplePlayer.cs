using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public static SimplePlayer Instance;
    
    private void Awake()
    {
        Instance = this;
    }
}
