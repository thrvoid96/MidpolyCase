using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static GameState gamestate;
    [NonSerialized] public LevelAssetCreate levelAsset;

    GameObject particlePool;


    private void Awake()
    {
        gamestate = GameState.BeforeStart;
        instance = this;
    }

    private void Start()
    {
        SetValues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Victory();
        }
    }


    //--------------------------------------------------------------------------//
    void SetValues()
    {
        levelAsset = Resources.Load<LevelAssetCreate>("Scriptables/LevelAsset");
        CreateGoldParticleHolder();
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

    //-------------------------------------------------------------------//
    void CreateGoldParticleHolder()
    {
        //particlePool = new GameObject("Particle Pool");

        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject createdParticleForPool = Instantiate(levelAsset.goldSplashParticle, particlePool.transform);
        //    createdParticleForPool.name = "Created Gold Particle_" + i;
        //    createdParticleForPool.SetActive(false);
        //}

    }

    //-------------------------------------------------------------------//

    //-------------------------------------------------------------------//
    public void Victory(float delay = 0.9f)
    {
        VictoryPanel.instance.VictoryCase(delay);
    }

    //-------------------------------------------------------------------//
    public void Fail()
    {
        Debug.Log("FAILED");
        gamestate = GameState.Failed;
        LosePanel.instance.LoseCase();
    }

    //----------------------------------------------------------------------------------------//
    public int FindTotalScore()
    {
        int totalScore = GameManager.Level * 10 + 50;
        return totalScore;
    }
}
