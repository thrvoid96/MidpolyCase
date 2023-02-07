using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerPanel : Singleton<AnswerPanel>
{
    [SerializeField] private RectTransform rightAnswer, wrongAnswer, multipEndPos;
    [SerializeField] private CanvasGroup rightMultip, wrongMultip;
    [SerializeField] private TextMeshProUGUI multipText;
    
    private Vector3 startPosRight, startPosWrong;
    private TextMeshProUGUI rightText, wrongText;
    
    private void Awake()
    {
        startPosRight = rightMultip.transform.position;
        startPosWrong = wrongMultip.transform.position;
        rightText = rightAnswer.GetChild(0).GetComponent<TextMeshProUGUI>();
        wrongText = wrongAnswer.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ShowAnswerPanel(bool correctGuess, string text, int multiplier)
    {
        multipText.text = multiplier.ToString();
        
        if (correctGuess)
        {
            CalculateAnimation(rightText,text,rightAnswer,rightMultip, startPosRight, correctGuess);
        }
        else
        {
            CalculateAnimation(wrongText,text,wrongAnswer,wrongMultip, startPosWrong, correctGuess);
        }
    }

    private void CalculateAnimation(TextMeshProUGUI textToChange, string textToSet, RectTransform answerToScale, CanvasGroup alphaToChange, Vector3 startPos, bool correctGuess)
    {
        textToChange.text = textToSet;

        answerToScale.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            if (correctGuess)
            {
                Player.Instance.PlayConfettiEffect();
            }
            
            alphaToChange.alpha = 1f;
            float alpha = alphaToChange.alpha;
            DOTween.To(() => alpha, x => alpha = x, 0f, 1f).SetDelay(0.5f).OnUpdate(() =>
            {
                alphaToChange.alpha = alpha;
            });

            alphaToChange.transform.position = startPos;
            alphaToChange.transform.DOMove(multipEndPos.position, 1f).OnComplete(() =>
            {
                answerToScale.DOScale(Vector3.zero, 0.5f);
            });
        });
    }
}
