using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class VictoryPanel : Singleton<VictoryPanel>
{
    [SerializeField] private Button nextButton;
    
    public void VictoryCase()
    {
        StartCoroutine(OpenPanelWithDelay());

        IEnumerator OpenPanelWithDelay()
        {
            yield return new WaitForSeconds(1f);

            AudioManager.Play(AudioClipName.VictoryPanelOpen);
            InputPanel.Instance.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    public void CompleteLevel()
    {
        nextButton.interactable = false;
        GameManager.Level++;
       
        
        if (GameManager.Level-1 <= LevelManager.Instance.levelAsset.levelPrefabs.Count)
        {
            GameManager.PreviousLevel = GameManager.Level-2;
        }
        else
        {
            GameManager.PreviousLevel = GameManager.RandomLevel;
            GameManager.RandomLevel = 0;
        }
        
        LevelManager.Instance.RestartSceneWithDelay(1f);
    }
}
