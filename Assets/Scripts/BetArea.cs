using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BetArea : MonoBehaviour
{
    [SerializeField] private TextMeshPro multiplierText, questionBoxText;
    
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
        questionBoxText.text = questionToAsk;
        multiplierText.text = "BETTING ODDS <size=150%> x" + Multiplier;
        belts[0].SetMat(correctBelt == belts[0].BeltType);
        belts[1].SetMat(correctBelt == belts[1].BeltType);
    }
#endif
    
    private void Awake()
    {
        multiplierText.text = "BETTING ODDS <size=150%> x" + Multiplier;
        questionBoxText.transform.parent.gameObject.SetActive(false);
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
        var correctAnswerGiven = totalBetOnCorrect > 0;
        
        if (!correctAnswerGiven)
        {
            if (correctBelt == BeltType.BeltLeft)
            {
                CheckForFail(correctAnswerGiven,BeltType.BeltRight);
            }
            else
            {
                CheckForFail(correctAnswerGiven,BeltType.BeltLeft);
            }
            
            return;
        }
        
        AnswerPanel.Instance.ShowAnswerPanel(correctAnswerGiven, beltToCheck.answerText, Multiplier);
        beltToCheck.StartMoneyCollect();
        
        var moneyList = beltToCheck.getMoneyObjects;

        for (int i = 0; i < totalBetOnCorrect * Multiplier; i++)
        {
            var spawnedObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.GroundMoney,
                moneyList[i % moneyList.Count].transform.position,
                Quaternion.identity, null).GetComponent<ICollectable>();
            
            Player.Instance.PlayerStackHandler.CollectMoney(spawnedObj);
        }
    }

    private void CheckForFail(bool correctAnswerGiven,BeltType beltType)
    {
        var beltToCheck = belts[(int) beltType];
        if (beltToCheck.GetCurrentBetAmount() == 0)
        {
            Debug.Log("Player did not bet at all");
            return;
        }
        
        AnswerPanel.Instance.ShowAnswerPanel(correctAnswerGiven, beltToCheck.answerText, Multiplier);
    }
}
