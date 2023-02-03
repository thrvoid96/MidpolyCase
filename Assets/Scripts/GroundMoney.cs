using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoney : Collectable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem particlesys;
    protected override void Awake()
    {
        base.Awake();
        _animator.SetTrigger(AnimatorHashes.DoIdle);
    }

    public override void Collect()
    {
        base.Collect();
        particlesys.Play();
        _animator.SetTrigger(AnimatorHashes.StopIdle);
    }
}
