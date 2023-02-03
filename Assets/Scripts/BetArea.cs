using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BetArea : MonoBehaviour
{
    [SerializeField] private TextMeshPro leftBeltText, rightBeltText, multiplierText;
    [SerializeField] private Transform leftBeltCashParent, rightBeltCashParent;
    [SerializeField] private GameObject leftQuestionObject, rightQuestionObject;
    [SerializeField] private MeshRenderer leftTriangle, rightTriangle;
    [SerializeField] private Material rightMat, wrongMat, defaultMat;
    [SerializeField] private float questionMoveSpeed;
    [field: SerializeField] public int multiplier { get; private set; }
    [SerializeField] private string questionToAsk, leftBeltAnswer, rightBeltAnswer;
    [SerializeField] private BeltType correctBelt;

    private int[] betsOnBelt = new int[2];
    private int[] rewardMoney = new int[2];

    private List<List<GameObject>> moneyObjects = new List<List<GameObject>>();


#if UNITY_EDITOR
    private void OnValidate()
    {
        leftBeltText.text = leftBeltAnswer;
        rightBeltText.text = rightBeltAnswer;
        multiplierText.text = "BETTING ODDS <size=150%> x" + multiplier;
        if (correctBelt == BeltType.BeltLeft)
        {
            leftTriangle.material = rightMat;
            rightTriangle.material = wrongMat;
            return;
        }
        
        leftTriangle.material = wrongMat;
        rightTriangle.material = rightMat;
    }
#endif
    
    private void Awake()
    {
        leftTriangle.material = defaultMat;
        rightTriangle.material = defaultMat;
        leftBeltText.text = leftBeltAnswer;
        rightBeltText.text = rightBeltAnswer;
        multiplierText.text = "BETTING ODDS <size=150%> x" + multiplier;
        List<GameObject> tempList = new List<GameObject>();

        foreach (Transform child in leftBeltCashParent)
        {
            tempList.Add(child.gameObject);
        }
        
        moneyObjects.Add(tempList);
        tempList.Clear();
        
        foreach (Transform child in rightBeltCashParent)
        {
            tempList.Add(child.gameObject);
        }
        moneyObjects.Add(tempList);
    }

    public void BetOnBelt(BeltType beltType, int amount)
    {
        betsOnBelt[(int) beltType] += amount;
    }

    public void UnlockNextMoney(BeltType beltType)
    {
        if (rewardMoney[(int)beltType]< moneyObjects[(int)beltType].Count)
        {
            moneyObjects[(int)beltType][rewardMoney[(int)beltType]].SetActive(true);
            rewardMoney[(int) beltType] += 1;
        }
    }

    public void EnterArea()
    {
        BetPanel.Instance.AskQuestion(questionToAsk);

        leftQuestionObject.transform.DOLocalMoveZ(45f, 1f/questionMoveSpeed).SetDelay(1.5f).SetEase(Ease.Linear);
        rightQuestionObject.transform.DOLocalMoveZ(45f, 1f/questionMoveSpeed).SetDelay(1.5f).SetEase(Ease.Linear);
    }

    public void ExitArea()
    {
        BetPanel.Instance.QuestionComplete();
        GiveReward();
    }

    private void GiveReward()
    {
        var totalBetOnCorrect = betsOnBelt[(int) correctBelt];
        
        if (totalBetOnCorrect <= 0)
        {
            // Do bad ui, no rewards
            return;
        }

        for (int i = 0; i < totalBetOnCorrect; i++)
        {
            
        }
    }
}
