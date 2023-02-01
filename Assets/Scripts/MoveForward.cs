using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{

    public float speed = 10;






    private void Update()
    {
        if (LevelManager.gamestate == GameState.Normal)
        {
            transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
        }

    }

    //private void Update()
    //{
    //    if(Input.GetAxis("Horizontal") != 0)
    //    {
    //        transform.position += Input.GetAxis("Horizontal") * Vector3.right;
    //        Vector3 tempPos = transform.position;
    //        tempPos.x = Mathf.Clamp(tempPos.x, -8, 8);
    //        transform.position = tempPos;
    //    }
    //}
}
