using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class VictoryPanel : MonoBehaviour
{
    public static VictoryPanel instance;
    Button nextButton;
    Button collectButton;
    Text scoreText;

    private void Awake()
    {
        instance = this;
        nextButton = transform.GetChild(0).GetChild(2).GetComponent<Button>();
        scoreText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        collectButton = transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Button>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            VictoryCase();
        }
    }
    public void VictoryCase(float delay = 0.9f)
    {
        StartCoroutine(OpenPanelWithDelay(delay));
       

        IEnumerator OpenPanelWithDelay(float delay2 = 0.9f)
        {
            yield return new WaitForSeconds(delay2);
            DOTween.KillAll();
            CashTextAnimate();

            AudioManager.Play(AudioClipName.VictoryPanelOpen);
            InputPanel.instance.gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Text>().text = "COLLECT " + LevelManager.instance.FindTotalScore();

            transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().transform.DOScale(12f, 0.2f).OnComplete(() =>
            {
                transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().transform.DOScale(10f, 0.2f);
            });
        }
       
    }


    public void CompleteButtonsHandleWithoutAdd()
    {
        GameManager.TotalCoin += LevelManager.instance.FindTotalScore();
        collectButton.interactable = false;
        nextButton.interactable = false;
        GameManager.Level++;
        if (GameManager.Level-1 <= LevelManager.instance.levelAsset.levelPrefabs.Count)
        {
            GameManager.PreviousLevel = GameManager.Level-2;
        }
        else
        {
            GameManager.PreviousLevel = GameManager.RandomLevel;
            GameManager.RandomLevel = 0;
        }        
        
        CashPanel.instance.SendImage(scoreText.gameObject,30);
        StartCoroutine(Delay());
    }

    public void CompleteButtonsHandleWithAdd()
    {
        GameManager.TotalCoin += Bonus.instance.multiplyCoin;
        collectButton.interactable = false;
        nextButton.interactable = false;
        GameManager.Level++;
        if (GameManager.Level - 1 <= LevelManager.instance.levelAsset.levelPrefabs.Count)
        {
            GameManager.PreviousLevel = GameManager.Level - 2;
        }
        else
        {
            GameManager.PreviousLevel = GameManager.RandomLevel;
            GameManager.RandomLevel = 0;
        }
        CashPanel.instance.SendImage(scoreText.gameObject, 50);
        Bonus.instance.AnimPause();
        StartCoroutine(Delay());
        
    }

    public void CashTextAnimate(float duration = 0.25f)
    {
        scoreText.gameObject.GetComponent<RectTransform>().DOScale(Vector3.one * 1.1f, 0.25f).OnComplete(() => scoreText.gameObject.GetComponent<RectTransform>().DOScale(Vector3.one, 0.3f));

        int currentCash = LevelManager.instance.FindTotalScore();
        DOTween.To(() => currentCash, x => currentCash = x, LevelManager.instance.FindTotalScore(), duration).OnUpdate(() =>
        {
            scoreText.text = currentCash.ToString();
        });
    }


    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


}
