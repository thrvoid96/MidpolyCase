using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpperPanel : MonoBehaviour
{
    public static UpperPanel instance;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        transform.GetChild(2).GetComponent<Text>().text = "Lv. " + GameManager.Level;
    }

    public void RetryButtonHandleEvent()
    {
        //Elephant.LevelFailed(GameManager.Level);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
