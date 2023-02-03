using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private TextMeshPro beltText;
    [SerializeField] private Material rightMat, wrongMat, defaultMat;
    [SerializeField] private MeshRenderer triangle;
    [SerializeField] private Transform beltCashParent;
    [field: SerializeField] public GameObject QuestionObject { get; private set; }
    public Transform moneyEnterance;
    public BeltType beltType;

    private List<GameObject> moneyObjects = new();
    private int currentIndex;
    private int betAmount;
    
    private void Awake()
    {
        triangle.material = defaultMat;
        
        foreach (Transform child in beltCashParent)
        {
            moneyObjects.Add(child.gameObject);
        }
    }

    public void SetText(string text)
    {
        beltText.text = text;
    }

    public void SetMat(bool isTrue)
    {
        if (!isTrue)
        {
            triangle.material = wrongMat;
            return;
        }

        triangle.material = rightMat;
    }

    public void AddBetOnBelt(int amount)
    {
        betAmount += amount;
        if (currentIndex == moneyObjects.Count)
        {
            Debug.LogError("Belt capacity reached");
            return;
        }
        moneyObjects[currentIndex].SetActive(true);
        currentIndex++;
    }

    public int GetCurrentBetAmount()
    {
        return betAmount;
    }
}
