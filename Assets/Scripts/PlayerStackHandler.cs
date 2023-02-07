using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class PlayerStackHandler : MonoBehaviour
{
    private TransformAccessArray accessArray;
    [SerializeField] private AnimationCurve xCurve, yCurveEnd, yCurveStart;
    private static NativeArray<Keyframe> xCurveKeys,yCurveStartKeys,yCurveEndKeys;
    private List<Transform> moneyList = new ();
    [field: SerializeField] public int CurrentCashCount { get; private set; }
    
    private struct UpdateJob : IJobParallelForTransform
    {
        public int ArrayLength;
        public float InputFactor, AmountCalculation;
        public void Execute(int i, TransformAccess transform)
        {
            float t = InputFactor * i / ArrayLength;
            transform.localPosition = new Vector3(xCurveKeys[i].value * InputFactor * AmountCalculation, 
                Mathf.Lerp(yCurveStartKeys[i].value, yCurveEndKeys[i].value, Mathf.Abs(t)), transform.localPosition.z);
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,t * -90f) * AmountCalculation);
        }
    }
    
    public void UpdateStackPositions(float input)
    {
        accessArray = new TransformAccessArray(moneyList.ToArray());

        var job = new UpdateJob
        {
            ArrayLength = moneyList.Count,
            AmountCalculation =  Mathf.Clamp(0.8f - (0.01f * CurrentCashCount),0.1f,1f),
            InputFactor = input
        };
            
        JobHandle jobHandle = job.Schedule(accessArray);
            
        jobHandle.Complete();
        accessArray.Dispose();
    }
    
    private void OnDestroy()
    {
        yCurveStartKeys.Dispose();
        yCurveEndKeys.Dispose();
    }

    private void Awake()
    {
        var xKeyFramesLength = 100;
        var yStartKeyFramesLength = 100;
        var yEndKeyFramesLength = 100;
        
        var xKeyFrames = new Keyframe[xKeyFramesLength];
        var yStartKeyFrames = new Keyframe[yStartKeyFramesLength];
        var yEndKeyFrames = new Keyframe[yEndKeyFramesLength];
        
        for (int i = 0; i < xKeyFramesLength; i++)
        {
            Keyframe xKeyFrame = new Keyframe
            {
                time = i,
                value = xCurve.Evaluate(i)
            };
            
            xKeyFrames[i] = xKeyFrame;
        }

        for (int i = 0; i < yStartKeyFramesLength; i++)
        {
            Keyframe yStartkeyframe = new Keyframe
            {
                time = i,
                value = yCurveStart.Evaluate(i)
            };
            
            yStartKeyFrames[i] = yStartkeyframe;
        }

        for (int i = 0; i < yEndKeyFramesLength; i++)
        {
            Keyframe yEndKeyFrame = new Keyframe
            {
                time = i,
                value = yCurveEnd.Evaluate(i)
            };
            
            yEndKeyFrames[i] = yEndKeyFrame;
        }
        
        yCurveStartKeys = new NativeArray<Keyframe>(yStartKeyFrames, Allocator.Persistent);
        yCurveEndKeys = new NativeArray<Keyframe>(yEndKeyFrames, Allocator.Persistent);
        xCurveKeys = new NativeArray<Keyframe>(xKeyFrames, Allocator.Persistent);
    }
    
    public void CollectMoney(ICollectable collectable)
    {
        collectable.Collect();
        var collectableTrans = collectable.GetTransform();
        var startIndex = CurrentCashCount;
        var trans = transform;

        var newEmptyObject = ObjectPool.Instance.SpawnFromPool(PoolEnums.EmptyObject, trans.position, Quaternion.identity, trans);
        var newObjectTrans = newEmptyObject.transform;
        newObjectTrans.localPosition = Vector3.zero;

        if (moneyList.Count < 100)
        {
            moneyList.Add(newObjectTrans);
        }
        else
        {
            startIndex = 99;
            newObjectTrans.SetParent(moneyList[startIndex]);
        }
        
        newObjectTrans.localScale = Vector3.one;
        collectableTrans.SetParent(newEmptyObject.transform);
        collectableTrans.localRotation = Quaternion.Euler(Vector3.zero);
        
        collectableTrans.DOLocalMove(Vector3.zero, 0.5f);
        CurrentCashCount++;
    }
    
    public GameObject PopNextMoney()
    {
        if (CurrentCashCount - 1 < 0)
        {
            return null;
        }
        
        CurrentCashCount -= 1;
        if (CurrentCashCount > 100)
        {
            var lastMoney = moneyList[99].GetChild(0);
            return lastMoney.gameObject;
        }
        
        var startIndex = CurrentCashCount;
        moneyList[startIndex].SetParent(null);
        return moneyList[startIndex].gameObject;
    }

    public int RemoveUntilValue(int value)
    {
        if (CurrentCashCount > value)
        {
            var lastCount = CurrentCashCount;
            CurrentCashCount = value + 1;
            return lastCount - CurrentCashCount;
        }

        return 0;
    }
    
    public void PutMoneyOnBelt(Belt beltToPut)
    {
        if (CurrentCashCount - 1 < 0)
        {
            Player.Instance.moneyPutTween.Kill();
            return;
        }
        
        CurrentCashCount -= 1;
        var startIndex = CurrentCashCount;
        
        var newObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.StackMoney,  moneyList[startIndex].position,  moneyList[startIndex].rotation, beltToPut.transform);
        
        moneyList[startIndex].SetParent(null);
        moneyList[startIndex].gameObject.SetActive(false);
        moneyList.RemoveAt(startIndex);
        
        newObj.transform.localScale = Vector3.one;
        newObj.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.Linear);
        newObj.transform.DOLocalMove(new Vector3(0f, -0.2f, newObj.transform.localPosition.z), 0.1f).OnComplete(() =>
        {
            var distance = Vector3.Distance(beltToPut.MoneyEnterance.transform.position, newObj.transform.position);
            newObj.transform.DOMove(beltToPut.MoneyEnterance.transform.position, distance * 0.03f).OnComplete(() =>
            {
                beltToPut.AddBetOnBelt();
                newObj.SetActive(false);
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);
    }
}
