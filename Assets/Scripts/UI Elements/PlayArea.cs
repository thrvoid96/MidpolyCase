using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public static PlayArea instance;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        LevelManager.gamestate = GameState.Normal;
        UpperPanel.instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        UpperPanel.instance.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
