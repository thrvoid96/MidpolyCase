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
    private static NativeArray<Keyframe> xCurveKeys,curve1Keys,curve2Keys;
    private Transform[] moneyArray;
    private ICollectable[] moneyCollectables;
    [field: SerializeField] public int CurrentCashCount { get; private set; }
    
    private struct UpdateJob : IJobParallelForTransform
    {
        public int ArrayLength,currentCash;
        public float InputFactor;
        public void Execute(int i, TransformAccess transform)
        {
            float t = InputFactor * i / ArrayLength;
            transform.localPosition = new Vector3(xCurveKeys[i].value * InputFactor * Mathf.Clamp(1f - (0.03f * currentCash),0.1f,1f), 
                Mathf.Lerp(curve1Keys[i].value, curve2Keys[i].value, Mathf.Abs(t) * Mathf.Clamp(1f - (0.03f * currentCash),0.1f,1f)), transform.localPosition.z);
            transform.localRotation = Quaternion.Euler(new Vector3(0f,0f,t * -100f) * Mathf.Clamp(1f - (0.03f * currentCash),0.1f,1f));
        }
    }
    
    private void OnDestroy()
    {
        accessArray.Dispose();
        curve1Keys.Dispose();
        curve2Keys.Dispose();
    }

    private void Awake()
    {
        var childCount = transform.childCount;

        moneyArray = new Transform[childCount];
        moneyCollectables = new ICollectable[childCount];

        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
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
        if (CurrentCashCount == moneyArray.Length)
        {
            Debug.Log("Max money reached");
            return;
        }
        
        collectable.Collect();
        var collectableTrans = collectable.GetTransform();
        var startIndex = CurrentCashCount;

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

        CurrentCashCount++;
    }
    
    public GameObject PopNextMoney()
    {
        if (CurrentCashCount - 1 < 0)
        {
            return null;
        }
        
        CurrentCashCount -= 1;
        var startIndex = CurrentCashCount;
        moneyCollectables[startIndex].GetTransform().GetChild(0).gameObject.SetActive(false);
        moneyArray[startIndex].gameObject.SetActive(false);
        
        var newObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.StackMoney, moneyArray[startIndex].position, moneyArray[startIndex].rotation, null);

        return newObj;
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
        moneyCollectables[startIndex].GetTransform().GetChild(0).gameObject.SetActive(false);
        moneyArray[startIndex].gameObject.SetActive(false);
        
        var newObj = ObjectPool.Instance.SpawnFromPool(PoolEnums.StackMoney, moneyArray[startIndex].position, moneyArray[startIndex].rotation, beltToPut.transform);
        
        newObj.transform.localScale = Vector3.one;
        newObj.transform.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.Linear);
        newObj.transform.DOLocalMove(new Vector3(0f, -0.2f, newObj.transform.localPosition.z), 0.1f).OnComplete(() =>
        {
            var distance = Vector3.Distance(beltToPut.moneyEnterance.transform.position, newObj.transform.position);
            newObj.transform.DOMove(beltToPut.moneyEnterance.transform.position, distance * 0.03f).OnComplete(() =>
            {
                beltToPut.AddBetOnBelt();
                newObj.SetActive(false);
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);
    }

    public void UpdateStackPositions(float input)
    {
        var job = new UpdateJob
        {
            ArrayLength = moneyArray.Length,
            currentCash =  CurrentCashCount,
            InputFactor = input
        };
            
        JobHandle jobHandle = job.Schedule(accessArray);
            
        jobHandle.Complete();
    }
}
