using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            var finalScale = Vector3.one * 1.2f;
            ScaleLoop(finalScale);
        }
    }

    private void ScaleLoop(Vector3 finalScale)
    {
        retryButton.transform.DOScale(finalScale, 1f).OnComplete(() =>
        {
            retryButton.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
            {
                ScaleLoop(finalScale);
            });
        });
    }
}
