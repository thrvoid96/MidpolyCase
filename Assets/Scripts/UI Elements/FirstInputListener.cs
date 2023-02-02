using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstInputListener : Singleton<FirstInputListener>, IPointerDownHandler
{
    [SerializeField] private RectTransform handTrans,finalTrans;
    private void Start()
    {
        handTrans.DOMove(finalTrans.position, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LevelManager.Instance.gameStart.Invoke();
        handTrans.DOKill();
        handTrans.gameObject.SetActive(false);
        enabled = false;
    }
}
