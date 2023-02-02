using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 10;
    [SerializeField] private float sidewaysSpeed = 1.2f;
    [SerializeField] private float maxXDistance = 5f;
    [SerializeField] private Transform moneyHolderParent;
    [SerializeField] private AnimationCurve xCurve, yCurveEnd, yCurveStart;

    private Vector3 tempPos;
    private Transform parentObj;
    private Transform[] moneyArray;

    private float input = 0f;
    TransformAccessArray accessArray;
    private static NativeArray<Keyframe> xCurveKeys,curve1Keys,curve2Keys;
    
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
    
    private void Awake()
    {
        parentObj = transform.parent;

        moneyArray = new Transform[moneyHolderParent.childCount];

        for (int i = 0; i < moneyHolderParent.childCount; i++)
        {
            moneyArray[i] = moneyHolderParent.GetChild(i);
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
    
    private void OnDestroy()
    {
        accessArray.Dispose();
        curve1Keys.Dispose();
        curve2Keys.Dispose();
    }    
    
    private void Update()
    {
        if (LevelManager.gamestate == GameState.Normal)
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            LevelManager.Instance.Victory();
        }
    }
    
}
