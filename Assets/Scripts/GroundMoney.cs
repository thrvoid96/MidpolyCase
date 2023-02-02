using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoney : MonoBehaviour, ICollectable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem particlesys;
    
    private void Awake()
    {
        _animator.SetTrigger(AnimatorHashes.DoIdle);
    }

    public void Collect()
    {
        particlesys.Play();
        _animator.SetTrigger(AnimatorHashes.StopIdle);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
