using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Vector3 temp;
    float mouseSensivity = 1.2f;
    
    private void Update()
    {
        if (LevelManager.gamestate == GameState.Normal)
        {
            temp = transform.localPosition;
            temp.x = Mathf.Clamp(temp.x + InputPanel.valX * mouseSensivity * 10, -7, 7);
            transform.localPosition = Vector3.Lerp(transform.localPosition, temp, 0.8f);
            InputPanel.valX = Mathf.Lerp(InputPanel.valX, 0, 0.05f);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            LevelManager.instance.Victory();
        }
    }
    //-----------------------------------------------------------------------//

}
