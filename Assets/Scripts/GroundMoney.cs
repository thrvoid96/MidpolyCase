using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoney : MonoBehaviour,ICollectable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem particlesys;
    private static readonly int DoIdle = Animator.StringToHash("doIdle");
    private static readonly int StopIdle = Animator.StringToHash("stopIdle");

    private void Awake()
    {
        _animator.SetTrigger(DoIdle);
    }

    public void Collect()
    {
        particlesys.Play();
        _animator.SetTrigger(StopIdle);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
