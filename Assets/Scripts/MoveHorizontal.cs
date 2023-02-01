using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    void Update()
    {
        if (LevelManager.gamestate == GameState.Normal)
        {
            //            transform.position += InputPanel.instance.horizontal * 1000 * Vector3.right;
            //#if UNITY_EDITOR
            //            transform.position += Input.GetAxis("Horizontal") * Vector3.right;
            //            transform.position += InputPanel.instance.horizontal * 10  * Vector3.right;
            //#endif
            MoveHor();


        }
    }

    public void MoveHor()
    {
        Vector3 tempPos = transform.localPosition;
        tempPos.x += InputPanel.instance.horizontal * Time.deltaTime * 1000;
        tempPos.x = Mathf.Clamp(tempPos.x, -1 * 9, 9);
        transform.localPosition = Vector3.Lerp(transform.localPosition, tempPos, 1f);
    }
}
