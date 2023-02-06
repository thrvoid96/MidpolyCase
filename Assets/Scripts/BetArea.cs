using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BetArea : MonoBehaviour
{
    [SerializeField] private TextMeshPro multiplierText;
    
    [SerializeField] private float questionMoveSpeed;
    [SerializeField] private List<Belt> belts;
    [field: SerializeField] public int Multiplier { get; private set; }
    [SerializeField] private string questionToAsk, leftBeltAnswer, rightBeltAnswer;
    [SerializeField] private BeltType correctBelt;
    

#if UNITY_EDITOR
    private void OnValidate()
    {
        belts[0].SetText(leftBeltAnswer);
        belts[1].SetText(rightBeltAnswer);
        multiplierText.text = "BETTING ODDS <size=150%> x" + Multiplier;
        belts[0].SetMat(correctBelt == belts[0].beltType);
        belts[1].SetMat(correctBelt == belts[1].beltType);
    }
#endif
    
    private void Awake()
    {
        multiplierText.text = "BETTING ODDS <size=150%> x" + Multiplier;
        
    }
    
    public void EnterArea()
    {
        BetPanel.Instance.AskQuestion(questionToAsk);

        belts[0].QuestionObject.transform.DOLocalMoveZ(45f, 1f/questionMoveSpeed).SetDelay(1.5f).SetEase(Ease.Linear);
        belts[1].QuestionObject.transform.DOLocalMoveZ(45f, 1f/questionMoveSpeed).SetDelay(1.5f).SetEase(Ease.Linear);
    }

    public void ExitArea()
    {
        BetPanel.Instance.QuestionComplete();
        GiveReward();
    }

    private void GiveReward()
    {
        var beltToCheck = belts[(int) correctBelt];
        var totalBetOnCorrect = beltToCheck.GetCurrentBetAmount();
        
        if (totalBetOnCorrect <= 0)
        {
            // Do bad ui, no rewards
            return;
        }
        
        beltToCheck.StartMoneyCollect();
        var moneyList = beltToCheck.getMoneyObjects;

        for (int i = 0; i < totalBetOnCorrect * Multiplier; i++)
        {
            var spawnedObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.StackMoney,
                moneyList[i % moneyList.Count].transform.position,
                Quaternion.identity, null).GetComponent<ICollectable>();
            
            Player.Instance.CollectMoney(spawnedObj,false);
        }
    }
}
