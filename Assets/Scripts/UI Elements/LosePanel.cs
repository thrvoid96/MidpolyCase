using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LosePanel : Singleton<LosePanel>
{
    [SerializeField] private Button retryButton;
    
    public void CompleteButtonsHandle()
    {
        retryButton.interactable = false;
        LevelManager.Instance.RestartSceneWithDelay(1f);
    }
    
    public void LoseCase()
    {
        StartCoroutine(LoseCaseDelay());

        IEnumerator LoseCaseDelay()
        {
            yield return new WaitForSeconds(1f);
            InputPanel.Instance.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            AudioManager.Play(AudioClipName.Failed);
        }
    }
    
}
