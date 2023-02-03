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
    [SerializeField] private Transform moneyHolderParent;
    [SerializeField] private AnimationCurve xCurve, yCurveEnd, yCurveStart;
    [SerializeField] private Animator animator;

    private Vector3 tempPos;
    private Transform parentObj;
    private Transform[] moneyArray;
    private ICollectable[] moneyCollectables;

    private float input, savedMaxXDistance;
    TransformAccessArray accessArray;
    private static NativeArray<Keyframe> xCurveKeys,curve1Keys,curve2Keys;

    [SerializeField] private int currentCashCount;
    private Tween moneyPutTween;
    
    private struct UpdateJob : IJobParallelForTransform
    {
        public int ArrayLength;
        public float InputFactor;
        public void Execute(int i, TransformAccess transform)
        {
            float t = InputFactor * i / ArrayLength;
            transform.localPosition = new Vector3(xCurveKeys[i].value * InputFactor, Mathf.Lerp(curve1Keys[i].value, curve2Keys[i].value, Mathf.Abs(t)), transform.localPosition.z);
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,t * -100f));
        }
    }
    
    private void OnDestroy()
    {
        accessArray.Dispose();
        curve1Keys.Dispose();
        curve2Keys.Dispose();
    }  

    private void StartRun()
    {
        animator.SetTrigger(AnimatorHashes.DoRun);
        StartCoroutine(nameof(RunLoop));
    }

    private IEnumerator RunLoop()
    {
        while (true)
        {
            var localPosition = transform.localPosition;
            tempPos = localPosition;
            tempPos.x = Mathf.Clamp(tempPos.x + InputPanel.valX * sidewaysSpeed * 10, -maxXDistance, maxXDistance);
            localPosition = Vector3.Lerp(localPosition, tempPos, 0.8f);
            transform.localPosition = localPosition;
            InputPanel.valX = Mathf.Lerp(InputPanel.valX, 0, 0.05f);
            parentObj.Translate(new Vector3(0, 0, 1) * Time.deltaTime * forwardSpeed);

            input = localPosition.x / maxXDistance;

            var job = new UpdateJob
            {
                ArrayLength = moneyArray.Length,
                InputFactor = input
            };
            
            JobHandle jobHandle = job.Schedule(accessArray);
            
            jobHandle.Complete();
            
            yield return null;
        }
    }

    private void Awake()
    {
        LevelManager.Instance.gameStart += StartRun;
        savedMaxXDistance = maxXDistance;
        parentObj = transform.parent;

        var childCount = moneyHolderParent.childCount;

        moneyArray = new Transform[childCount];
        moneyCollectables = new ICollectable[childCount];

        for (int i = 0; i < childCount; i++)
        {
            var child = moneyHolderParent.GetChild(i);
            moneyArray[i] = child;
            moneyCollectables[i] = child.GetChild(0).GetComponent<ICollectable>();
        }

        accessArray = new TransformAccessArray(moneyArray);
        
        var newKeyFrames = new Keyframe[moneyArray.Length];
        var newKeyFrames2 = new Keyframe[moneyArray.Length];
        var newKeyFrames3 = new Keyframe[moneyArray.Length];

        for (int i = 0; i < moneyArray.Length; i++)
        {
            Keyframe keyframe = new Keyframe
            {
                time = i,
                value = yCurveStart.Evaluate(i)
            };
            
            Keyframe keyframe2 = new Keyframe
            {
                time = i,
                value = yCurveEnd.Evaluate(i)
            };
            
            Keyframe keyframe3 = new Keyframe
            {
                time = i,
                value = xCurve.Evaluate(i)
            };

            newKeyFrames[i] = keyframe;
            newKeyFrames2[i] = keyframe2;
            newKeyFrames3[i] = keyframe3;
        }
        
        curve1Keys = new NativeArray<Keyframe>(newKeyFrames, Allocator.Persistent);
        curve2Keys = new NativeArray<Keyframe>(newKeyFrames2, Allocator.Persistent);
        xCurveKeys = new NativeArray<Keyframe>(newKeyFrames3, Allocator.Persistent);
    }

    public void CollectMoney(ICollectable collectable, bool doScaleEffect)
    {
        if (currentCashCount == moneyArray.Length)
        {
            Debug.Log("Max money reached");
            return;
        }
        
        collectable.Collect();
        var collectableTrans = collectable.GetTransform();
        var startIndex = currentCashCount;

        moneyArray[startIndex].gameObject.SetActive(true);
        
        collectableTrans.SetParent(moneyArray[startIndex]);
        collectableTrans.localRotation = Quaternion.Euler(Vector3.zero);

        if (doScaleEffect)
        {
            collectableTrans.localScale = Vector3.one * 0.5f;
        }
        else
        {
            collectableTrans.localScale = Vector3.one * 0.45f;
        }
        
        collectableTrans.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
        {
            collectableTrans.SetParent(null);
            collectableTrans.gameObject.SetActive(false);
                
            moneyCollectables[startIndex].GetTransform().GetChild(0).gameObject.SetActive(true);
        });

        currentCashCount++;
    }

    private void PutMoneyOnBelt(Belt beltToPut)
    {
        if (currentCashCount - 1 < 0)
        {
            moneyPutTween.Kill();
            return;
        }
        
        currentCashCount -= 1;
        var startIndex = currentCashCount;
        moneyCollectables[startIndex].GetTransform().GetChild(0).gameObject.SetActive(false);
        moneyArray[startIndex].gameObject.SetActive(false);
        
        var newObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.StackMoney, moneyArray[startIndex].position, moneyArray[startIndex].rotation, beltToPut.transform);
        
        newObj.transform.localScale = Vector3.one;
        newObj.transform.DOLocalRotate(Vector3.zero, 0.25f);
        newObj.transform.DOLocalMove(new Vector3(0f, 0.5f, newObj.transform.localPosition.z), 0.25f).OnComplete(() =>
        {
            var distance = Vector3.Distance(beltToPut.moneyEnterance.transform.position, newObj.transform.position);
            newObj.transform.DOMove(beltToPut.moneyEnterance.transform.position, distance * 0.05f).OnComplete(() =>
            {
                beltToPut.AddBetOnBelt(1);
            }).SetEase(Ease.Linear);
        });

        
    }

    private void RecalculateMoney()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            CollectMoney(collectable,true);
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
            moneyPutTween = DOVirtual.DelayedCall(0.1f, delegate { PutMoneyOnBelt(belt); }).SetLoops(-1,LoopType.Restart);
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
