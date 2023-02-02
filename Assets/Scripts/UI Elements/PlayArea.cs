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
        gameObject.SetActive(false);
    }
}
