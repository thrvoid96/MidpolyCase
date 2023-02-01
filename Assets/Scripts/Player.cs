using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;


    //public GameObject frontLeftWheel;
    //public GameObject frontRightWheel;

    //yatay hareketler için componentler
    Vector3 temp;
    float mouseSensivity = 1.2f;


    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (LevelManager.gamestate == GameState.Normal)
        {
            temp = transform.localPosition;
            temp.x = Mathf.Clamp(temp.x + InputPanel.valX * mouseSensivity * 10, -7, 7);
            transform.localPosition = Vector3.Lerp(transform.localPosition, temp, 0.8f);
            transform.localEulerAngles = AnglleVec();
            //frontLeftWheel.transform.localEulerAngles = AngleForWheel(frontLeftWheel);
            //frontRightWheel.transform.localEulerAngles = AngleForWheel(frontRightWheel);

            InputPanel.valX = Mathf.Lerp(InputPanel.valX, 0, 0.05f);


        }



    }

    //-----------------------------------------------------------------------//
    
    
    //-----------------------------------------------------------------------//
    Vector3 AnglleVec()
    {
        Vector3 temp = transform.localEulerAngles;
        Vector3 temp2 = transform.localEulerAngles;
        temp.y = InputPanel.valX * 1000;
        temp.y = temp.y > 180 ? temp.y - 360 : temp.y;
        if (InputPanel.valX < 0 && Mathf.Approximately(transform.localPosition.x, -7) /*&& Mathf.Approximately(transform.localRotation.y, 0.10f)*/)
        {
            temp.y /= 100;
        }
        else if (InputPanel.valX > 0 && Mathf.Approximately(transform.localPosition.x, 7))
        {
            temp.y /= 100;
        }

        temp2.y = temp2.y > 180 ? temp2.y - 360 : temp2.y;
        temp2.y = Mathf.Lerp(temp2.y, temp.y, 0.1f);
        temp2.y = Mathf.Clamp(temp2.y, -30, 30);
        return temp2;
    }

    //-----------------------------------------------------------------------//
    Vector3 AngleForWheel(GameObject obj)
    {
        Vector3 temp = obj.transform.localEulerAngles;
        Vector3 temp2 = obj.transform.localEulerAngles;
        temp.y = InputPanel.valX * 1000;
        temp.y = temp.y > 180 ? temp.y - 360 : temp.y;
        if (InputPanel.valX < 0 && Mathf.Approximately(transform.localPosition.x, -7) /*&& Mathf.Approximately(transform.localRotation.y, 0.10f)*/)
        {
            temp.y /= 100;
        }
        else if (InputPanel.valX > 0 && Mathf.Approximately(transform.localPosition.x, 7))
        {
            temp.y /= 100;
        }
        temp2.y = temp2.y > 180 ? temp2.y - 360 : temp2.y;
        temp2.y = Mathf.Lerp(temp2.y, temp.y, 0.1f);
        temp2.y = Mathf.Clamp(temp2.y, -70, 70);
        return temp2;
    }

    //-----------------------------------------------------------------------//


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            LevelManager.instance.Victory();
        }
    }
    //-----------------------------------------------------------------------//

}
