using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public static Bonus instance;
    Animator myAnimator;
    Text text;
    [System.NonSerialized] public int state = 2;
    [System.NonSerialized] public int multiplyCoin;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        myAnimator = transform.GetComponent<Animator>();
        text = transform.parent.transform.GetChild(3).transform.GetComponent<Text>();
    }


    public void AnimPause()
    {
        myAnimator.speed = 0;
    }

    public void IndexUp()
    {
        state++;
        SetCoin();

    }

    public void IndexDown()
    {
        state--;
        SetCoin();
    }


    public void SetCoin()
    {
        multiplyCoin = LevelManager.instance.FindTotalScore() * state;
        text.text = multiplyCoin.ToString();       
    }
}
