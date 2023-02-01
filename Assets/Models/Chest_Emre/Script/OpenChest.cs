using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    public Animator myAnimator;
    public static OpenChest instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ChestAnimTrigger();
        }
    }

    public void ChestAnimTrigger()
    {
        AudioManager.Play(AudioClipName.PrizeChest);
        myAnimator.SetBool("toOpen", true);
    }
}
