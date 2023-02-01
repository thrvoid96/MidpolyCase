
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CashPanel : MonoBehaviour
{
    public static CashPanel instance;
    Text cashText;
    Image cashImage;
    public AnimationCurve cashSendCurve;
    public static int levelMoney;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cashText = transform.GetChild(0).GetComponent<Text>();
        cashImage = transform.GetChild(1).GetComponent<Image>();
        CashTextAnimate();
    }

    public static void SetCoin(int coin)
    {
        coin = Mathf.Clamp(coin, 50, 2999);
        levelMoney = coin;
        GameManager.TotalCoin += coin;
    }

    public void CashTextAnimate(float duration = 0.25f)
    {
        cashText.gameObject.GetComponent<RectTransform>().DOScale(Vector3.one * 1.25f, 0.25f).OnComplete(() => cashText.gameObject.GetComponent<RectTransform>().DOScale(Vector3.one, 0.3f));

        int currentCash = int.Parse(cashText.text);
        DOTween.To(() => currentCash, x => currentCash = x, GameManager.TotalCoin, duration).OnUpdate(() =>
        {
            cashText.text = currentCash.ToString();
        });
    }

    public void SendImage(GameObject startPos, int count)
    {
        AudioManager.Play(AudioClipName.VictoryPanelMovingCoin);
        RectTransform startRect = startPos.GetComponent<RectTransform>();
        for (int i = 0; i < count; i++)
        {
            GameObject createdCash = Instantiate(cashImage.gameObject, startRect.transform.position, Quaternion.identity, transform) as GameObject;
            Vector2 xydif = Random.insideUnitCircle * Random.Range(20f, 110f) * 3f;
            Vector3 posconverter = new Vector3((startRect.position.x - xydif.x), (startRect.position.y - xydif.y), 0f);
            createdCash.GetComponent<RectTransform>().DOMove(posconverter, 1f).SetEase(cashSendCurve).OnComplete(() =>
            {
                createdCash.GetComponent<RectTransform>().DOMove(cashImage.gameObject.GetComponent<RectTransform>().position, Random.Range(0.2f, .5f)).OnComplete(() => 
                {
                    createdCash.SetActive(false);
                    //Destroy(createdCash);
                });
            });
            CashTextAnimate(1f);
        }
    }
}


