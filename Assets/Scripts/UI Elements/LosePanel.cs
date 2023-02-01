using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LosePanel : MonoBehaviour
{
    public static LosePanel instance;

    Button button;

    private void Awake()
    {
        instance = this;
        button = transform.GetChild(2).GetComponent<Button>();
    }
    public void CompleteButtonsHandle()
    {

        StartCoroutine(DelayForBuild2());
        button.interactable = false;
        
    }


    public void LoseCase()
    {
        StartCoroutine(LoseCaseDelay());

        IEnumerator LoseCaseDelay()
        {
            yield return new WaitForSeconds(1f);
            InputPanel.instance.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            AudioManager.Play(AudioClipName.Failed);
        }
    }

    IEnumerator DelayForBuild2()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
