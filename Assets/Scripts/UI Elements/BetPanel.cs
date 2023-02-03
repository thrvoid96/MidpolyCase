using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BetPanel : Singleton<BetPanel>
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private RectTransform bgTrans, endTrans;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = bgTrans.position;
    }

    public void AskQuestion(string question)
    {
        bgTrans.gameObject.SetActive(true);
        questionText.text = question;
        bgTrans.transform.DOMove(endTrans.position, 1f).SetEase(Ease.InOutSine);
    }

    public void QuestionComplete()
    {
        bgTrans.transform.DOMove(startPos, 1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            bgTrans.gameObject.SetActive(false);
        });
    }
}
