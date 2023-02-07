using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class Player : Singleton<Player>
{
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float sidewaysSpeed = 1.2f;
    [SerializeField] private float maxXDistance = 5f;
    [SerializeField] private float putMoneyOnBeltDelay = 0.05f;
    
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem confetti;
    [field: SerializeField] public PlayerStackHandler PlayerStackHandler { get; private set; }
    
    private Vector3 tempPos;
    private Transform parentObj;

    private float input, savedMaxXDistance;
    
    public Tween moneyPutTween;
    private Coroutine movementRoutine;

    private bool canMove = true;
    
    private void Awake()
    {
        LevelManager.Instance.gameStart += StartRun;
        savedMaxXDistance = maxXDistance;
        parentObj = transform.parent;
    }
    
    private void StartRun()
    {
        animator.SetTrigger(AnimatorHashes.DoRun);
        movementRoutine = StartCoroutine(nameof(RunLoop));
    }

    private IEnumerator RunLoop()
    {
        while (true)
        {
            var localPosition = transform.localPosition;
            if (canMove)
            {
                tempPos = localPosition;
                tempPos.x = Mathf.Clamp(tempPos.x + InputPanel.valX * sidewaysSpeed * 10, -maxXDistance, maxXDistance);
                localPosition = Vector3.Lerp(localPosition, tempPos, 0.8f);
                transform.localPosition = localPosition;
                InputPanel.valX = Mathf.Lerp(InputPanel.valX, 0, 0.05f);
                parentObj.Translate(new Vector3(0, 0, 1) * Time.deltaTime * forwardSpeed);
            }
            
            input = localPosition.x / maxXDistance;

            PlayerStackHandler.UpdateStackPositions(input);
            
            yield return null;
        }
    }

    
    public void PlayConfettiEffect()
    {
        confetti.Play();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            PlayerStackHandler.CollectMoney(collectable,true);
        }
        else if(other.TryGetComponent<BetArea>(out var betArea))
        {
            betArea.EnterArea();
            
            float randomVal = maxXDistance;
            DOTween.To(() => randomVal, x => randomVal = x, 2f, 1f).OnUpdate(() =>
            {
                maxXDistance = randomVal;
            });
        }
        else if (other.TryGetComponent<Belt>(out var belt))
        {
            moneyPutTween = DOVirtual.DelayedCall(putMoneyOnBeltDelay, delegate { PlayerStackHandler.PutMoneyOnBelt(belt); })
                .SetLoops(-1, LoopType.Restart);
        }
        else if (other.TryGetComponent<EndGame>(out var endGame))
        {
            canMove = false;
            transform.DOLocalMove(new Vector3(0f,transform.position.y,3f), 0.5f).OnComplete(() =>
            {
                StopCoroutine(movementRoutine);
                animator.SetTrigger(AnimatorHashes.DoIdle);
            });
            endGame.FinishGame();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<BetArea>(out var betArea))
        {
            betArea.ExitArea();
            
            float randomVal = maxXDistance;
            DOTween.To(() => randomVal, x => randomVal = x, savedMaxXDistance, 1f).OnUpdate(() =>
            {
                maxXDistance = randomVal;
            });
        }
        else if (other.TryGetComponent<Belt>(out var belt))
        {
            moneyPutTween.Kill();
        }
    }
}
