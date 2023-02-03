using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour, ICollectable
{
    protected virtual void Awake()
    {

    }

    public virtual void Collect()
    {
        
    }
    
    public Transform GetTransform()
    {
        return transform;
    }
}
