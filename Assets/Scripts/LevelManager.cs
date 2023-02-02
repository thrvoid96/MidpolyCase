﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public static GameState gamestate;
    [NonSerialized] public LevelAssetCreate levelAsset;

    GameObject particlePool;


    private void Awake()
    {
        gamestate = GameState.BeforeStart;
    }

    private void Start()
    {
        SetValues();
    }
    
    //--------------------------------------------------------------------------//
    void SetValues()
    {
        levelAsset = Resources.Load<LevelAssetCreate>("Scriptables/LevelAsset");
        CreateLevel();
    }
    
    //-------------------------------------------------------------------//
    void CreateLevel()
    {
        if (GameManager.Level <= levelAsset.levelPrefabs.Count)
        {
            Instantiate(levelAsset.levelPrefabs[GameManager.Level - 1]);
        }
        else
        {
            if(GameManager.RandomLevel == 0)
            {
                List<int> gelebilecekbölümler = new List<int>();
                for (int i = 0; i < levelAsset.levelPrefabs.Count; i++)
                {
                    gelebilecekbölümler.Add(i);
                }
                gelebilecekbölümler.Remove(GameManager.PreviousLevel);
                int random = UnityEngine.Random.Range(0, gelebilecekbölümler.Count);
                GameManager.RandomLevel = gelebilecekbölümler[random];
            }
            
            Instantiate(levelAsset.levelPrefabs[GameManager.RandomLevel]);
        }
       
    }

    public void RestartSceneWithDelay(float delay)
    {
        StartCoroutine(SceneResetDelay(delay));
    }

    private IEnumerator SceneResetDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //-------------------------------------------------------------------//
    public void Victory()
    {
        gamestate = GameState.Victory;
        VictoryPanel.Instance.VictoryCase();
    }

    //-------------------------------------------------------------------//
    public void Fail()
    {
        gamestate = GameState.Failed;
        LosePanel.Instance.LoseCase();
    }
}
