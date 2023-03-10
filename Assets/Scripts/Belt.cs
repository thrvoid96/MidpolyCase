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
    [SerializeField] private ParticleSystem particleToPlay;
    [field: SerializeField] public Transform MoneyEnterance{ get; private set; }
    [field: SerializeField] public GameObject QuestionObject { get; private set; }
    [field: SerializeField] public BeltType BeltType { get; private set; }

    private List<GameObject> moneyObjects = new();
    private int currentIndex;
    private int betAmount;
    public string answerText { get; private set; }

    public List<GameObject> getMoneyObjects => moneyObjects;

    private void Awake()
    {
        triangle.material = defaultMat;
        answerText = beltText.text;
        
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

    public void AddBetOnBelt()
    {
        betAmount += 1;
        if (currentIndex == moneyObjects.Count)
        {
            Debug.Log("Belt capacity reached");
            return;
        }
        moneyObjects[currentIndex].SetActive(true);
        currentIndex++;
    }

    public int GetCurrentBetAmount()
    {
        return betAmount;
    }
    
    public void StartMoneyCollect()
    {
        for (int i = 0; i < moneyObjects.Count; i++)
        {
            moneyObjects[i].SetActive(false);
        }
        
        particleToPlay.Play();
    }
}
