using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMoney : Collectable
{
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particlesys;
    [SerializeField] private Collider collider;
    protected override void Awake()
    {
        base.Awake();
        animator.SetTrigger(AnimatorHashes.DoIdle);
    }

    public override void Collect()
    {
        base.Collect();
        particlesys.Play();
        collider.enabled = false;
        animator.SetTrigger(AnimatorHashes.StopIdle);
    }
}
