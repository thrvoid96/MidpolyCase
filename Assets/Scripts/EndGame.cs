using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Transform cameraStartPos,cameraEndPos, moneyStartPos;
    [SerializeField] private float cameraStartAnimDur,cameraEndAnimDur, moneyPopDur;
    [SerializeField] private int totalSections,sectionHeight, extraRewardSectionHeight;

    [SerializeField] private List<ParticleSystem> sectionConfettis, extraRewardSectionConfettis;

    private Vector3 nextMoneyPos;
    private float nextMoneyPosX = 0.05f;
    private float nextMoneyPosY = 0.5f;
    private Transform cameraTrans;
    
    private int totalPlacedMoneyCount, confettiIndex, extraConfettiIndex, finalHeight;

    private Tween moneyPopTwn;
    private Tweener cameraMoveTrans;

    private void Awake()
    {
        finalHeight = sectionHeight * totalSections;
        cameraTrans = Camera.main.transform;
    }

    public void FinishGame()
    {
        if (Player.Instance.PlayerStackHandler.CurrentCashCount == 0)
        {
            LevelManager.Instance.gameLost.Invoke();
            return;
        }
        
        cameraTrans.SetParent(transform);
        cameraTrans.DOLocalMove(cameraStartPos.localPosition, cameraStartAnimDur);
        cameraTrans.DOLocalRotate(cameraEndPos.localRotation.eulerAngles, cameraStartAnimDur).OnComplete(() =>
        {
            nextMoneyPos = moneyStartPos.position;
            moneyPopTwn = DOVirtual.DelayedCall(moneyPopDur, PutMoneyOnEnd).SetLoops(-1, LoopType.Restart);
            cameraTrans.DOLocalMove(cameraEndPos.localPosition, cameraEndAnimDur).OnComplete(() =>
            {
                cameraMoveTrans = cameraTrans.DOLocalMove(cameraEndPos.localPosition + new Vector3(0f, nextMoneyPosY * totalPlacedMoneyCount), moneyPopDur).OnComplete(
                        () =>
                        {
                            if (Player.Instance.PlayerStackHandler.CurrentCashCount != 0)
                            {
                                var distance = Mathf.Abs(cameraTrans.localPosition.y - cameraStartPos.localPosition.y);
                                cameraMoveTrans = cameraTrans.DOLocalMove(cameraStartPos.localPosition, distance * 0.2f)
                                    .SetEase(Ease.Linear);
                            }
                        })
                    .SetEase(Ease.Linear);
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);
    }

    private void PutMoneyOnEnd()
    {
        var nextMoney = Player.Instance.PlayerStackHandler.PopNextMoney();

        if (nextMoney == null)
        {
            moneyPopTwn.Kill();
            cameraMoveTrans?.Kill();
            LevelManager.Instance.gameWon.Invoke();
        }

        if (totalPlacedMoneyCount < finalHeight)
        {
            nextMoneyPos += new Vector3(0f, nextMoneyPosY, 0f);
            nextMoneyPosX *= -1f;
            nextMoneyPos.x = nextMoneyPosX;
        }
        
        nextMoney.transform.localScale = Vector3.one * 2f;
        nextMoney.transform.DOLocalMove(nextMoneyPos, moneyPopDur).OnComplete(() =>
        {
            totalPlacedMoneyCount++;
            
            if (totalPlacedMoneyCount < finalHeight)
            {
                if (totalPlacedMoneyCount % sectionHeight == 0)
                {
                    sectionConfettis[confettiIndex].Play();
                    confettiIndex++;
                }
            
                if (totalPlacedMoneyCount % extraRewardSectionHeight == 0)
                {
                    extraRewardSectionConfettis[extraConfettiIndex].Play();
                    extraConfettiIndex++;
                }
            }
            else
            {
                cameraMoveTrans?.Kill();
                nextMoney.SetActive(false);
            }
            
        }).SetEase(Ease.Linear);
    }
}
