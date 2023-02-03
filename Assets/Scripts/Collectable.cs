using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour, ICollectable
{
    [field: SerializeField] protected MoneyType moneyType;
    [SerializeField] private Transform modelsParent;

    protected List<GameObject> allModels = new ();
    protected virtual void Awake()
    {
        foreach (Transform child in modelsParent)
        {
            allModels.Add(child.gameObject);
        }
    }

    public virtual void Collect()
    {
        
    }
    
    public void ChangeType(MoneyType type)
    {
        for (int i = 0; i < allModels.Count; i++)
        {
            allModels[i].SetActive(false);
        }
        
        allModels[(int)type].SetActive(true);
        moneyType = type;
    }

    public MoneyType GetType()
    {
        return moneyType;
    }

    public Transform GetTransform()
    {
        return transform;
    }

   
}
